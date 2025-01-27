using A_Game.Classes.GameObjects;
using A_Game.Classes.Interfaces;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace A_Game.Classes
{
    internal class CanvasParameters : IUpdateHandler
    {
        public static Vector Center;
        public static Dictionary<string, double> Bounds;
        public static SpawnPoint SpawnPoint;

        public Canvas Instance;

        private Player _player;
        private SpawnPoint SpawnPointOnCurrentScene;
        private List<IMovable> _movableGameObjects = new List<IMovable>();//TODO
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
            UpdateMovableObjects();
        }

        public void AddGameObjects(List<IGameObject> gameObjects)
        {
            _gameObjects.AddRange(gameObjects); //Добавляем элементы в лист
            if(_gameObjects.TryGetObject(out SpawnPointOnCurrentScene))
            {
                SpawnPoint = SpawnPointOnCurrentScene;
            }

            AddGameObjectsToCanvasChildren();
            DrawGameObjectsOnCanvas();
        }
        public void RemoveGameObjects()
        {
            foreach (var gameObject in _gameObjects)
            {
                Instance.Children.Remove(gameObject.GameObject);
            }
            _gameObjects.Clear(); //убираем элементы из класса
        }

        private void AddGameObjectsToCanvasChildren()
        {
            foreach (var Object in _gameObjects)
            {
                Instance.Children.Add(Object.GameObject);
            }
        }

        private void DrawGameObjectsOnCanvas()
        {
            foreach (var Object in _gameObjects) //Обновление объектов на сцене
            {
                Canvas.SetLeft(Object.GameObject, Object.Position.X);
                Canvas.SetBottom(Object.GameObject, Object.Position.Y);
            }
        }
        public void UpdateMovableObjects()
        {
            //Обновление игрока
            Canvas.SetLeft(_player.GameObject, _player.Position.X);
            Canvas.SetBottom(_player.GameObject, _player.Position.Y);

            //Обновление движущихся элементов
            foreach (var Object in _movableGameObjects)
            {

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
