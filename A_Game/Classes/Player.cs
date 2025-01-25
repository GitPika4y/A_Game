using A_Game.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;

namespace A_Game.Classes
{
    internal class Player : IInputHandler, IUpdateHandler, IGameObject, ICollider
    {
        public Image GameObject => _gameObject;
        public Vector Position => _position;
        public Dictionary<string, double> ColliderBounds => _colliderBounds;


        //Interfaces realisation
        private Image _gameObject;
        private Vector _position;
        private CollisionControl _collisionControl;
        private Dictionary<string, double> _colliderBounds;

        //Прыжок
        public bool IsOnGround = false;
        private static readonly float _gravityDefault = -9.8f;
        private float _gravity = _gravityDefault;
        private float _jumpScale = 2;
        private float _maxJumpForce = 20;
        private bool _isJumping = false;

        //Ходьба
        private float moveSpeed = 5;

        //Inputs
        private Key? _input;
        private Dictionary<Key, bool> _holdingInputs = new Dictionary<Key, bool>()
        {
            [Key.D] = false,
            [Key.A] = false
        };

        public Player(ImageSource gameObject, Vector startPosition)
        {
            _position = startPosition;
            _gameObject = new Image
            {
                Source = gameObject,
                Width = gameObject.Width,
                Height = gameObject.Height,
            };

            UpdateColllisionBounds();

            _collisionControl = new CollisionControl();
        }
        public void Update()
        {
            if (IsOnGround == false)
                _position.Y += _gravity;

            //Обработчик нажатий (передвижение, прыжок)
            UpdateColllisionBounds();
            _collisionControl.HandleCollisions(this);
            HandleMovement();
            IsOnGround = false;
        }

        public void SetPosition(double x, double y)
        {
            _position = new Vector(x,y);
        }

        private void UpdateColllisionBounds()
        {
            _colliderBounds = new Dictionary<string, double>()
            {
                ["Top"] = _position.Y + _gameObject.Height,
                ["Bottom"] = _position.Y ,
                ["Left"] = _position.X,
                ["Right"] = _position.X + _gameObject.Width
            };
        }

        private void HandleMovement()
        {
            if (_input == Key.Space && !_isJumping && IsOnGround)
            {
                Jump();
            }
            // Движение влево: клавиша "A" удерживается
            if (_holdingInputs[Key.A])
            {
                _position.X -= moveSpeed;
            }
            // Движение вправо: клавиша "D" удерживается
            if (_holdingInputs[Key.D])
            {
                _position.X += moveSpeed;
            }
        }

        private async void Jump() => await Task.Run(() => DoJump());

        private async void DoJump()
        {
            if (_isJumping) return;

            _isJumping = true;
            _gravity = default;//Обнуляем гравитацию

            while (_gravity < _maxJumpForce)
            {
                _gravity += _jumpScale;
                await Task.Delay(6);
            }
            
            while (_gravity > _gravityDefault)
            {
                _gravity -= _jumpScale;
                await Task.Delay(16);
            }
            _isJumping = false;
        }


        //Inputs Methods (from IInputHandler)
        public void OnKeyDown(Key key)
        {
            _input = key;

            if (_holdingInputs.ContainsKey(key))
            {
                _holdingInputs[key] = true;
            }
        }

        public void OnKeyUp(Key key)
        {
            _input = null;

            if (_holdingInputs.ContainsKey(key))
            {
                _holdingInputs[key] = false;
            }
        }
    }
}
