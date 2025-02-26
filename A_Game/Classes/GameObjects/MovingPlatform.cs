using A_Game.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace A_Game.Classes.GameObjects
{
    internal class MovingPlatform : Platform, IMovable, IUpdateble
    {
        public bool IsMoving { get ; set ; }


        private Vector _moveTo;
        private float _timeDuration;
        private bool _isLooping;

        public MovingPlatform(ImageSource gameObject, Vector position, Vector moveTo, float timeDuration, bool isLooping) : base(gameObject, position)
        {
            _moveTo = moveTo;
            _timeDuration = timeDuration;
            _isLooping = isLooping;
        }

        public void Update()
        {
            Move();
        }

        public void SetPosition(double x, double y)
        {
            _position = new Vector(x, y);
        }

        private void Move(){
            _ = this.MoveTo(_moveTo, _timeDuration, _isLooping);
            GameObject.UpdateColllisionBounds(Position, out _colliderBounds);
        }


    }
}
