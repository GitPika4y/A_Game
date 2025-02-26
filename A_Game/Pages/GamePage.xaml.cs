using A_Game.Classes.Interfaces;
using A_Game.Classes.SceneControls;
using A_Game.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using A_Game.Data.Saves;

namespace A_Game.Pages
{
	/// <summary>
	/// Логика взаимодействия для Page1.xaml
	/// </summary>
	public partial class GamePage : Page
	{
		public static bool IsNewGame = false;
		public static bool GameLoaded = false;
		public static GamePage Instance;
		public static DispatcherTimer GameTimer;

		//Основные Объекты
		private CanvasParameters _canvas;
		private Player _player;
		private SceneManager _sceneManager;

		//Lists
		private List<IInputable> _inputHandlers = new List<IInputable>();
		private List<IUpdateble> _updateHandlers = new List<IUpdateble>();

		//Инициализация всего
		public GamePage()
		{
			InitializeComponent();
			Instance = this;

			//Подписка, что мы вышли из игры(не окна)
			PausePage.GameExited += SetDefault;
		}

		private void SetDefault()
		{
			IsNewGame = false;
			GameLoaded = false;

			_player = null;
			_canvas = null;
			_sceneManager = null;
			ProgressEventsData.EventFlags.Clear();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Canvas.Focus();

			//Игра не новая -> загружаем объекты, таймер
			if(IsNewGame == false)
			{
				InitializeObjects();
				InitializeTimer();
			}

		}
		private void InitializeTimer()
		{
			GameTimer = new DispatcherTimer();
			GameTimer.Interval = TimeSpan.FromMilliseconds(16);// ~ 60 FPS
			GameTimer.Tick += UpdateAll;
			GameTimer.Start();
		}

		private void InitializeObjects()
		{

			//Стандартные инициализации объектов
			_player = new Player(new Vector(CanvasParameters.Center.X, 80));

			_canvas = new CanvasParameters(Canvas, _player);

			_sceneManager = new SceneManager(_canvas, _player);

			_sceneManager.LoadScene((0, 0));

			//Если игра загружена
			if(GameLoaded == true)
			{
				//Берем значения из сохранения
				SaveData saveData = SaveManager.LoadGame();
				//Загружаем (изменяем текущие) объекты из сохранения
				LoadSavedData(saveData);
			}

			IsNewGame = true;
		}
		public void LoadSavedData(SaveData saveData)
		{
			//Восстанавливаем прогресс
			ProgressEventsData.LoadEventFlags(saveData.EventFlags);
			// Восстанавливаем позицию игрока
			_player.SetPosition(saveData.SpawnPoint.Position.X, saveData.SpawnPoint.Position.Y);
			// Восстанавливаем сцену
			_sceneManager.LoadScene(saveData.SpawnPoint.CurrentScene);
			//Восстанавливаем SpawnPoint
			CanvasParameters.SpawnPoint = saveData.SpawnPoint;
		}


		private void SetPause()
		{
			GameTimer.Stop();//Останавливаем таймер (игру) - возобновится из PausePage
			NavigationService.Navigate(new PausePage());
		}

		

		public void RemoveHandlersAndColliders()
		{
			_inputHandlers.Clear();
			_updateHandlers.Clear();
			CollisionManager.Colliders.Clear();
		}
		public void AddUpdateHandler(IUpdateble handler)
		{
			_updateHandlers.Add(handler);
		}
		public void RemoveUpdateHandler(IUpdateble handler)
		{
			_updateHandlers.Remove(handler);
		}
		public void AddInputHandler(IInputable handler)
		{
			_inputHandlers.Add(handler);
		}
		public void RemoveInputHandler(IInputable handler)
		{
			_inputHandlers.Remove(handler);
		}

		//Handlers methods
		private void UpdateAll(object sender, EventArgs e)
		{
			foreach (var handler in _updateHandlers)
			{
				handler?.Update();
			}
			_canvas?.Update();
			_sceneManager?.Update();
			_player?.Update();
		}
		private void InputGetKeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Escape)
			{
				SetPause();
				return;
			}

			foreach (var handler in _inputHandlers)
			{
				handler?.OnKeyDown(e.Key);
			}
			_player?.OnKeyDown(e.Key);

		}

		private void InputGetKeyUp(object sender, KeyEventArgs e)
		{
			foreach (var handler in _inputHandlers)
			{
				handler?.OnKeyUp(e.Key);
			}
			_player.OnKeyUp(e.Key);
		}
	}
}
