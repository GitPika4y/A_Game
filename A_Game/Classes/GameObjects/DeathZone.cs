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
    internal class DeathZone : IGameObject , ICollider
    {
        public Image GameObject { get; }
        public Vector Position {get;}
        public Dictionary<string, double> ColliderBounds { get; }


        public DeathZone(ImageSource gameObject, Vector position)
        {
            Position = position;
            GameObject = new Image
            {
                Source = gameObject,
                Width= gameObject.Width,
                Height= gameObject.Height,
            };
            ColliderBounds = new Dictionary<string, double>()
            {
                ["Top"] = position.Y + gameObject.Height,
                ["Bottom"] = position.Y,
                ["Left"] = position.X,
                ["Right"] = position.X + gameObject.Width,
            };
        }
	}
}
