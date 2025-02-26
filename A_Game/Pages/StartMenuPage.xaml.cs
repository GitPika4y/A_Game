using A_Game.Classes;
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
	/// Логика взаимодействия для Page1.xaml
	/// </summary>
	public partial class StartMenuPage : Page
	{
		public StartMenuPage()
		{
			InitializeComponent();

			CheckSave();
		}

		private void btnClick_StartGame(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new GamePage());
		}

		private void btnClick_ShutDown(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
        }

		private void CheckSave()
		{
			//Сохранение (объект)
			SaveData saveData = SaveManager.LoadGame();

			if (saveData != null)
			{
				//Если сохранение есть, показываем "Продолжить"
				btnContinue.Visibility = Visibility.Visible;
				txblock_SaveDate.Visibility = Visibility.Visible;

				txblock_SaveDate.Text = $"Последнее сохранение:\t{saveData.DateTime}";
			}
		}

		private void btnClick_Continue(object sender, RoutedEventArgs e)
		{
			//Говорим что игра была загружена
			GamePage.GameLoaded = true;

			//Перемещаемся в окно с игрой
			NavigationService.Navigate(new GamePage());
		}
	}
}
