using A_Game.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using A_Game.Classes;
using A_Game.Classes.SceneControls;
using System.Runtime.CompilerServices;
using A_Game.Pages;


namespace A_Game
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();


            MainFrame.Navigate(new StartMenuPage());
        }
        
    }
}
