using A_Game.Classes;
using A_Game.Classes.GameObjects;
using A_Game.Classes.SceneControls;
using A_Game.Data.Saves;
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

namespace A_Game.Pages
{
	/// <summary>
	/// Логика взаимодействия для Page2.xaml
	/// </summary>
	public partial class PausePage : Page
	{

		public static event Action GameExited;

		public PausePage()
		{
			InitializeComponent();
			Focus();
		}

		private void ResumeGame()
		{
			GamePage.GameTimer.Start();
			NavigationService.GoBack();
		}

		private void NavigateToStartMenu()
		{
			NavigationService.Navigate(new StartMenuPage());
		}

		private void Input_GetKeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Escape)
			{
				ResumeGame();
			}
		}
		private void btnClick_ResumeGame(object sender, RoutedEventArgs e)
		{
			ResumeGame();
		}

		private void btnClick_SaveAndExit(object sender, RoutedEventArgs e)
		{
			SpawnPoint spawnPoint = CanvasParameters.SpawnPoint;

			SaveManager.SaveGame( 
				(spawnPoint.Position.X, spawnPoint.Position.Y) ,
				spawnPoint.CurrentScene);


			GameExited?.Invoke(); //Вызываем события на закрытие игры
			NavigateToStartMenu();
		}

		private void btnClick_Exit(object sender, RoutedEventArgs e)
		{
			// Отображаем диалоговое окно для подтверждения выхода
			MessageBoxResult result = 
				MessageBox.Show("Вы уверены, что хотите выйти без сохранения?",
									"Подтверждение выхода",
									MessageBoxButton.YesNo,
									MessageBoxImage.Question);

			// Если пользователь выбрал "Yes", переходим в меню
			if (result == MessageBoxResult.Yes)
			{
				GameExited?.Invoke(); //Вызываем события на закрытие игры
				NavigateToStartMenu();
			}

		}



	}
}
