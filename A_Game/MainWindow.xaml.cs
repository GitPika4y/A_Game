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

namespace A_Game
{
    public partial class MainWindow : Window
    {

        //Основная логика игры, Canvas, handlers, gameobjects, timer, старт игры инициализация объектов

        //Объекты
        private CanvasParameters _canvas;
        private Player _player;
        private Camera _camera;
        private DispatcherTimer _timer;


        //Доп.Элементы
        private string _playerImgPath= "D:/asesprite_Img/My first Game(Dream)/Img/Player_Idle.png";


        //Handlers
        private List<IInputHandler> _inputHandlers = new List<IInputHandler>();
        private List<IUpdateHandler> _updateHandlers = new List<IUpdateHandler>();

        
        //Инициализация всего
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeObjects();
            InitializeTimer();
            // Инициализировать центр Canvas,объекты,таймер после загрузки окна
        }
        private void InitializeObjects()
        {

            _canvas = new CanvasParameters(Canvas);
            _player = new Player(_playerImgPath.GetImageSource(), _canvas.Center);
            _camera = new Camera(_player, _canvas);

            _canvas.AddGameObjects(_player);

            _inputHandlers.Add(_player);
            _updateHandlers.AddRange(new List<IUpdateHandler> { _player, _camera, _canvas});

            Canvas.Children.Add(_player.GameObject); 
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(16);// ~ 60 FPS
            _timer.Tick += Update;
            _timer.Start();
        }



        //Handlers methods
        private void Update(object sender, EventArgs e)
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
