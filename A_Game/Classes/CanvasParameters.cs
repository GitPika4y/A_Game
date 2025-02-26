using A_Game.Classes.GameObjects;
using A_Game.Classes.GameObjects.Pickups;
using A_Game.Classes.Interfaces;
using A_Game.Data.ProgressEvents;
using A_Game.Pages;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;


namespace A_Game.Classes
{
    [Serializable]
    internal class CanvasParameters : IUpdateble
    {
		[NonSerialized] public static Vector Center;
		[NonSerialized] public static Dictionary<string, double> Bounds;
        public static SpawnPoint SpawnPoint;
        [NonSerialized]public static List<IMovable> MovableGameObjects = new List<IMovable>();


		[NonSerialized]public static Canvas Instance;

		[NonSerialized]private static GamePage _mainWindow;
		[NonSerialized] private Player _player;
		[NonSerialized] private static List<IGameObject> _gameObjects = new List<IGameObject>();

        public CanvasParameters(Canvas instance, Player player) 
        {
            Instance = instance;
            GetCanvasCenter();
            Bounds = new Dictionary<string, double>{
                ["Top"] = Instance.ActualHeight,
                ["Bottom"] = 0,
                ["Left"] = 0,
                ["Right"] = instance.ActualWidth,
            };

            PausePage.GameExited += RemoveGameObjects;
            _mainWindow = GamePage.Instance;
            _player = player;
            Instance.Children.Add(_player.GameObject);
            SpawnPoint = new SpawnPoint(SpritesStorage.SpawnPoint, new Vector(Center.X, 32), (0,0));

        }

        public void Update()
        {
            UpdateMovableObjects();
        }


        //Рисуем объект на Canvas
        public static void DrawOnCanvas(IGameObject Object)
        {
            Canvas.SetLeft(Object.GameObject, Object.Position.X);
            Canvas.SetBottom(Object.GameObject, Object.Position.Y);
        }

        //Добавляем объект (в children) и рисуем его
        public static void AddGameObject(IGameObject gameObject)
        { 
            _gameObjects.Add(gameObject); //Добавляем в массив элементов
			Instance.Children.Add(gameObject.GameObject); //Добавляем в Canvas.Children
            CategoryGameObjectAdd(gameObject);//Распределяем по интерфейсам (ICOllider, IMovable...)
            ProgressDataCheck(gameObject);
            DrawOnCanvas(gameObject);//Отрисовываем его на Canvas
        }

		private static void ProgressDataCheck(IGameObject gameObject)
		{
            var progressEvents = ProgressEventsData.EventFlags;

            if(progressEvents.ContainsKey(Events.BowEquipped) && gameObject is BowPickup)
                RemoveGameObject(gameObject);
            if(progressEvents.ContainsKey(Events.SwordEquipped) && gameObject is SwordPickup)
                RemoveGameObject(gameObject);
		}

		//Удаляем объект (из Children)
		public static void RemoveGameObject(IGameObject gameObject)
        {
            _gameObjects.Remove(gameObject);
            Instance.Children.Remove(gameObject.GameObject);
            CategoryGameObjectRemove(gameObject);
        }

		private static void CategoryGameObjectAdd(IGameObject gameObject)
		{
			if (gameObject is ICollider collider)
				CollisionManager.Colliders.Add(collider);

			if (gameObject is IUpdateble updateHandler)
				_mainWindow.AddUpdateHandler(updateHandler);

			if (gameObject is IInputable inputHandler)
				_mainWindow.AddInputHandler(inputHandler);

			if (gameObject is IMovable movable)
				MovableGameObjects.Add(movable);

            if (gameObject is SpawnPoint spawnPoint)
                spawnPoint.UpdateSpawnPoint += HandleSpawnPointUpdate;
		}
		private static void CategoryGameObjectRemove(IGameObject gameObject)
		{
			if (gameObject is ICollider collider)
				CollisionManager.Colliders.Remove(collider);

			if (gameObject is IUpdateble updateHandler)
				_mainWindow.RemoveUpdateHandler(updateHandler);

			if (gameObject is IInputable inputHandler)
				_mainWindow.RemoveInputHandler(inputHandler);

			if (gameObject is IMovable movable)
				MovableGameObjects.Remove(movable);
		}


		//Множественное добавление объектов (и определение spawnPoint) и их рисовка (Для Load_Scene)
		public void AddGameObjects(List<IGameObject> gameObjects)
        {
			foreach (var Object in gameObjects)
			{
				AddGameObject(Object);
			}
		}
        public void RemoveGameObjects()
        {
            for (int i =0; i<_gameObjects.Count; i++)
            {
                RemoveGameObject(_gameObjects[i]);
                i--;
            }
        }

        //Обновление двигающихся объектов
        public void UpdateMovableObjects()
        {
            //Обновление игрока
            DrawOnCanvas(_player);

            //Обновление движущихся элементов
            foreach (var Object in MovableGameObjects)
            {
                DrawOnCanvas(Object);
            }
        }

		private static void HandleSpawnPointUpdate()
		{
			_gameObjects.TryGetObject(out SpawnPoint);
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
