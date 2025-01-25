using A_Game.Classes.Interfaces;
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
using A_Game.Classes;
using A_Game.Classes.Platforms;


namespace A_Game
{
    public partial class MainWindow : Window
    {

        //Основная логика игры, Canvas, handlers, gameobjects, timer, старт игры инициализация объектов

        //Объекты
        private CanvasParameters _canvas;
        private Player _player;
        private Platform _platform;
        private DispatcherTimer _timer;

        //Доп.Элементы
        private string _mainImgPath = "D:/_GitHubData/A_Game/sprites/";
        private string _playerImgName = "Player.png";
        private string _platformImgName = "Platform.png";


        //Lists
        private List<IInputHandler> _inputHandlers = new List<IInputHandler>();
        private List<IUpdateHandler> _updateHandlers = new List<IUpdateHandler>();
        private List<IGameObject> _gameObjects = new List<IGameObject>();
        private List<ICollider> _colliders = new List<ICollider>();

        //Инициализация всего
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeObjects();
            InitializeTimer();
        }
        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(16);// ~ 60 FPS
            _timer.Tick += UpdateAll;
            _timer.Start();
        }

        private void InitializeObjects()
        {
            _canvas = new CanvasParameters(Canvas);

            _player = new Player(_mainImgPath.GetImageSource(_playerImgName),
                _canvas.Center);
            _platform = new Platform(_mainImgPath.GetImageSource(_platformImgName),
                new Vector(_canvas.Center.X, 30));



            _inputHandlers.Add(_player);
            _updateHandlers.AddRange(new List<IUpdateHandler> { _player, _canvas,});
            _gameObjects.AddRange(new List<IGameObject> {_player, _platform,});
            _colliders.AddRange(new List<ICollider> {_player, _platform});

            CollisionControl.Colliders = _colliders;

            _canvas.AddGameObjects(_gameObjects);
        }




        //Handlers methods
        private void UpdateAll(object sender, EventArgs e)
        {
            foreach(var handler in _updateHandlers)
            {
                handler?.Update();
            }

        }
        private void InputGetKeyDown(object sender, KeyEventArgs e)
        {
            foreach(var handler in _inputHandlers)
            {
                handler?.OnKeyDown(e.Key);
            }
        }
        private void InputGetKeyUp(object sender, KeyEventArgs e)
        {
            foreach(var handler in _inputHandlers)
            {
                handler?.OnKeyUp(e.Key);
            }
        }
    }
}
