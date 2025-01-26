using A_Game.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace A_Game.Classes
{
    internal class CanvasParameters : IUpdateHandler
    {
        public Canvas Instance;
        public static Vector Center;
        public static Dictionary<string, double> Bounds;
        private Player _player;

        private List<IGameObject> _gameObjects = new List<IGameObject>();

        public CanvasParameters(Canvas instance, Player player) 
        {
            Instance = instance;
            Bounds = new Dictionary<string, double>{
                ["Top"] = Instance.ActualHeight,
                ["Bottom"] = 0,
                ["Left"] = 0,
                ["Right"] = instance.ActualWidth,
            };
            _player = player;
            Instance.Children.Add(_player.GameObject);
            GetCanvasCenter();
        }

        public void Update()
        {
            UpdateCanvas();
        }

        public void AddGameObjects(List<IGameObject> gameObjects)
        {
            _gameObjects.AddRange(gameObjects); //Добавляем элементы в класс
            ChildrenAddGameObjects();
            UpdateCanvas();
        }
        public void RemoveGameObjects()
        {
            foreach (var gameObject in _gameObjects)
            {
                Instance.Children.Remove(gameObject.GameObject);
            }
            _gameObjects.Clear(); //убираем элементы из класса
        }

        private void ChildrenAddGameObjects()
        {
            foreach (var Object in _gameObjects)
            {
                Instance.Children.Add(Object.GameObject);
            }
        }

        public void UpdateCanvas()
        {
            foreach(var Object in _gameObjects)
            {
                Canvas.SetLeft(Object.GameObject, Object.Position.X);
                Canvas.SetBottom(Object.GameObject, Object.Position.Y);
            }
            
            if(_player != null)
            {
                Canvas.SetLeft(_player.GameObject, _player.Position.X);
                Canvas.SetBottom(_player.GameObject, _player.Position.Y);
            }
        }
        public void GetCanvasCenter()
        {
            // Убедитесь, что размеры доступны
            if (Instance.ActualWidth > 0 && Instance.ActualHeight > 0)
            {
                Center = new Vector(Instance.ActualWidth / 2, Instance.ActualHeight / 2);
            }
        }

    }
}
