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

namespace A_Game
{
    internal class Player
    {
        public Image gameObject => _gameObject;
        public Vector position => _position;


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

        public Player(ImageSource gameObject, Vector startPosition)
        {
            _gameObject = new Image {
                Source = gameObject,
                Width = gameObject.Width,
                Height = gameObject.Height,
            };

            _position = startPosition;
        }
        public void Update(Key? input, Dictionary<Key, bool> holdingInputs)
        {
            // Гравитация всегда действует
            _position.Y += _gravity;

            // Прыжок: если клавиша "Space" нажата и персонаж не находится в состоянии прыжка
            if (input == Key.Space && !isJumping)
            {
                Jump();
            }

            // Движение влево: клавиша "A" удерживается
            if (holdingInputs.ContainsKey(Key.A) && holdingInputs[Key.A])
            {
                _position.X -= moveSpeed;
            }

            // Движение вправо: клавиша "D" удерживается
            if (holdingInputs.ContainsKey(Key.D) && holdingInputs[Key.D])
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
            isJumping = false ;
        }
    }
}
