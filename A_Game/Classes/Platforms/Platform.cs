using A_Game.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace A_Game.Classes.Platforms
{
    internal class Platform : IGameObject, ICollider
    {
        public Image GameObject => _gameObject;
        public Vector Position => _position;
        public Dictionary<string, double> ColliderBounds => _colliderBounds;



        private Image _gameObject;
        private Vector _position;
        private Dictionary<string, double> _colliderBounds;

        public Platform(ImageSource gameObject, Vector position)
        {
            _position = position;

            _gameObject = new Image
            {
                Source = gameObject,
                Width = gameObject.Width,
                Height = gameObject.Height,
            };

            _colliderBounds = new Dictionary<string, double>()
            {
                ["Top"] = _position.Y + _gameObject.Height,
                ["Bottom"] = _position.Y ,
                ["Left"] = _position.X,
                ["Right"] = _position.X + _gameObject.Width
            };
        }


    }
}
