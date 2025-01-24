using A_Game.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using  System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;

namespace A_Game.Classes
{
    internal class Player: IInputHandler, IUpdateHandler, IGameObject
    {
        public Image GameObject => _gameObject;
        public Vector Position => _position;


        private Image _gameObject;
        private Vector _position;

        //Прыжок
        private static readonly float _gravityDefault = -9.8f;
        private float _gravity = _gravityDefault;
        private float _jumpScale = 2;
        private float _maxJumpForce = 16;
        private bool isJumping = false;

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
            _gameObject = new Image {
                Source = gameObject,
                Width = gameObject.Width,
                Height = gameObject.Height,
            };

            _position = startPosition;
        }
        public void Update()
        {
            // Гравитация всегда действует
            _position.Y += _gravity;
            //Обработчик нажатий (передвижение, прыжок)
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (_input == Key.Space && !isJumping)
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

        private async void Jump() => await Task.Run(() =>DoJump());

        private async void DoJump()
        {
            isJumping = true;

            _gravity = default;//Обнуляем гравитацию
            while(_gravity < _maxJumpForce)
            {
                _gravity += _jumpScale;
                await Task.Delay(16);
            }
            //await Task.Delay(100);
            while(_gravity > _gravityDefault)
            {
                _gravity -= _jumpScale;
                await Task.Delay(16);
            }

            isJumping = false;
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
