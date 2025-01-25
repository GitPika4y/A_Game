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
        public Vector Center;

        private List<IGameObject> _gameObjects = new List<IGameObject>();

        public CanvasParameters(Canvas instance) 
        {
            Instance = instance;
            UpdateCanvasCentre();
        }

        public void Update()
        {
            UpdateCanvas();
        }

        public void AddGameObjects(List<IGameObject> gameObjects)
        {
            _gameObjects.AddRange(gameObjects);
            ChildrenAddGameObjects();
        }

        private void ChildrenAddGameObjects()
        {
            foreach (var Object in _gameObjects)
            {
                if(Instance.Children.Contains(Object.GameObject) == false)
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
        }
        public void UpdateCanvasCentre()
        {
            // Убедитесь, что размеры доступны
            if (Instance.ActualWidth > 0 && Instance.ActualHeight > 0)
            {
                Center = new Vector(Instance.ActualWidth / 2, Instance.ActualHeight / 2);
            }
        }

    }
}
