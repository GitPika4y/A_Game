using A_Game.Classes.Collisions;
using A_Game.Classes.GameObjects.Pickups;
using A_Game.Classes.GameObjects.PlayerControl;
using A_Game.Classes.Interfaces;
using A_Game.Data.ProgressEvents;
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
    public class Player : IInputable, IUpdateble, IGameObject, ICollider
    {
        public Image GameObject => _gameObject;
        public Vector Position => _position;
        public Dictionary<string, double> ColliderBounds => _colliderBounds;


        //Interfaces realisation
        private Image _gameObject;
        private Vector _position;
        private PlayerCollisionHandler _collisionHandler;
        private Dictionary<string, double> _colliderBounds;
        private float _collisionOffSet = 5;

        //Прыжок
        public bool IsOnGround = false;
        private static readonly float _gravityDefault = -9.8f;
        private float _gravity = _gravityDefault;
        private float _jumpScale = 2;
        private float _maxJumpForce = 16;
        private bool _isJumping = false;

        //Ходьба
        private float moveSpeed = 5;
        public int _moveDirection = 1;

        //Inputs
        private Key? _input;
        private Dictionary<Key, bool> _holdingInputs = new Dictionary<Key, bool>()
        {
            [Key.D] = false,
            [Key.A] = false
        };

        //ATTACK
        private bool _isAttacking = false;
        private Dictionary<Key?, IAttack> _attackButtons = new Dictionary<Key?, IAttack>() 
        {
            [Key.J] = null,
            [Key.K] = null,
        };


        public Player(Vector startPosition)
        {
            var image = SpritesStorage.Player;
            _gameObject = new Image
            {
                Source = image,
                Width = image.Width,
                Height = image.Height,
            };
            _position = startPosition;

            //Устанавливаем игрока на передний план навсегда
            Panel.SetZIndex(_gameObject, 1);

            _collisionHandler = new PlayerCollisionHandler();

            _gameObject.UpdateColllisionBounds(Position, out _colliderBounds,_collisionOffSet);

            ProgressEventsData.OnLoadEventFlags += CheckAttackButtons;
        }

		

		public void Update()
        {
            if (IsOnGround == false)
                _position.Y += _gravity;

            
            GameObject.UpdateColllisionBounds(Position, out _colliderBounds, _collisionOffSet);

            CollisionManager.HandleCollisions(this, _collisionHandler);

            HandleInputs();
            IsOnGround = false;
        }

        public void SetPosition(double x, double y)
        {
            _position = new Vector(x,y);
        }
        private void HandleInputs()
        {

            if (_input == Key.Space && !_isJumping && IsOnGround)
            {
                Jump();
            }
            // Движение влево: клавиша "A" удерживается
            if (_holdingInputs[Key.A])
            {
                _position.X -= moveSpeed;
                _moveDirection = -1;
            }
            // Движение вправо: клавиша "D" удерживается
            if (_holdingInputs[Key.D])
            {
                _position.X += moveSpeed;
                _moveDirection = 1;
            }
            if(_input != null &&
                _isAttacking == false &&
                _attackButtons.TryGetValue(_input, out var attack)
                )

            {
                if(attack != null)
                    Attack(attack);
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


		private void Attack(IAttack attack)
		{
			if (attack.IsAttacking) return; // Если эта атака уже выполняется, ничего не делаем

			_ = attack.Execute(_moveDirection); // Запускаем атаку в отдельном потоке
		}

        public void SetNewAttack(Pickup attackPickup)
        {
            switch (attackPickup)
            {
				case var n when n is SwordPickup:
					_attackButtons[Key.J] = new Melee(this);
					break;
				case var n when n is BowPickup:
                    _attackButtons[Key.K] = new Range(this);
                    break;
                
            }
        }
		private void CheckAttackButtons()
		{
			if (ProgressEventsData.GetEvent(Events.SwordEquipped))
			{
				_attackButtons[Key.J] = new Melee(this);
			}
			if (ProgressEventsData.GetEvent(Events.BowEquipped))
			{
				_attackButtons[Key.K] = new Range(this);
			}

            ProgressEventsData.OnLoadEventFlags -= CheckAttackButtons; //Отписываемся от этого события
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
