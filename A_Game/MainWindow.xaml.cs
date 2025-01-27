using A_Game.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using A_Game.Classes;
using A_Game.Classes.SceneManager;


namespace A_Game
{
    public partial class MainWindow : Window
    {
        //Запуск игры, сцены

        public static MainWindow Instance;

        //Объекты
        private CanvasParameters _canvas;
        private Player _player;
        private SceneManager _sceneManager;
        private DispatcherTimer _timer;
        

        //Lists
        private List<IInputHandler> _inputHandlers = new List<IInputHandler>();
        private List<IUpdateHandler>  _updateHandlers = new List<IUpdateHandler>();

        //Инициализация всего
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
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
            _player = new Player(SpritesStorage.Player,
                new Vector(CanvasParameters.Center.X, 80));

            _canvas = new CanvasParameters(Canvas, _player);
            _sceneManager = new SceneManager(_canvas, _player);

            _sceneManager.LoadScene((0,0));
        }


        public void RemoveHandlersAndColliders()
        {
            _inputHandlers.Clear();
            _updateHandlers.Clear();
            CollisionControl.Colliders.Clear();
        }
        public void AddUpdateHandler(IUpdateHandler handler)
        {
            _updateHandlers.Add(handler);
        }
        public void AddInputHandler(IInputHandler handler)
        {
            _inputHandlers.Add(handler);
        }

        //Handlers methods
        private void UpdateAll(object sender, EventArgs e)
        {
            foreach(var handler in _updateHandlers)
            {
                handler?.Update();
            }
            _canvas?.Update();
            _sceneManager?.Update();
            _player?.Update();
        }
        private void InputGetKeyDown(object sender, KeyEventArgs e)
        {
            foreach(var handler in _inputHandlers)
            {
                handler?.OnKeyDown(e.Key);
            }
            _player?.OnKeyDown(e.Key);
        }
        private void InputGetKeyUp(object sender, KeyEventArgs e)
        {
            foreach(var handler in _inputHandlers)
            {
                handler?.OnKeyUp(e.Key);
            }
            _player.OnKeyUp(e.Key);
        }
    }
}
