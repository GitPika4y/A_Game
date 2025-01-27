using A_Game.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace A_Game.Classes.GameObjects
{
    internal class SpawnPoint : IGameObject
    {
        public Image GameObject { get; }
        public Vector Position { get; }
        public (int x, int y) CurrentScene { get; }

        public SpawnPoint(ImageSource gameObject,Vector position, (int x, int y) currentScene)
        {
            GameObject = new Image
            {
                Source = gameObject,
                Width = gameObject.Width,
                Height = gameObject.Height,
            };
            Position = position;
            CurrentScene = currentScene;
        }
    }
}
