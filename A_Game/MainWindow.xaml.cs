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

namespace A_Game
{
    public partial class MainWindow : Window
    {
        //ОБъекты
        private Canvas _canvas;
        private Player _player;
        private Vector _canvasCentre;
        //Доп.Элементы
        private DispatcherTimer _timer;
        private Key? _input;
        private Dictionary<Key, bool> _holdingInputs = new Dictionary<Key, bool>()
            {   
                [Key.D] = false,
                [Key.A] = false
            };


        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void InitializeObjects()
        {
            _player = new Player(GetImage("D:/asesprite_Img/My first Game(Dream)/Img/Player_Idle.png"),
                _canvasCentre);
            Canvas.Children.Add(_player.gameObject);


            UpdateCanvas();
        }
        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(16);// ~ 60 FPS
            _timer.Tick += Update;
            _timer.Start();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Инициализировать центр Canvas,объекты,таймер после загрузки окна
            UpdateCanvasCentre();
            InitializeObjects();
            InitializeTimer();
        }
        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Обновлять центр Canvas при изменении его размеров
            _canvas = Canvas;
            UpdateCanvasCentre();
        }



        private void Update(object sender, EventArgs e)
        {
            UpdateCanvas();
            _player.Update(_input, _holdingInputs);

            _input = null;
        }
        private void UpdateCanvas()
        {
            // Устанавливаем координаты игрока так, чтобы он оказался по центру Canvas
            Canvas.SetLeft(_player.gameObject, _player.position.X);
            Canvas.SetBottom(_player.gameObject, _player.position.Y);
        }
        private void UpdateCanvasCentre()
        {
            // Убедитесь, что размеры доступны
            if (_canvas.ActualWidth > 0 && _canvas.ActualHeight > 0)
            {
                _canvasCentre = new Vector(_canvas.ActualWidth / 2, _canvas.ActualHeight / 2);
            }
        }


        private ImageSource GetImage(string imgPath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imgPath, UriKind.Absolute); // Путь к файлу
            bitmap.EndInit();
            return bitmap;
        }
        private void InputGetKeyDown(object sender, KeyEventArgs e)
        {
            _input = e.Key;
            if (_holdingInputs.ContainsKey(e.Key))
            {
                _holdingInputs[e.Key] = true;
            }
        }
        private void InputGetKeyUp(object sender, KeyEventArgs e)
        {
            _input = e.Key;
            if (_holdingInputs.ContainsKey(e.Key))
            {
                _holdingInputs[e.Key] = false;
            }
        }






    }
}
