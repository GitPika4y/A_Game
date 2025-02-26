using A_Game.Classes.Interfaces;
using A_Game.Classes.SceneControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace A_Game.Classes.GameObjects.PlayerControl
{
	internal class Range : IAttack, IMovable
	{
		public Image GameObject => _gameObject;
		public Vector Position => _position;
		public int Damage => _damage;
		public IGameObject Parent => _parent;
		public Dictionary<string, double> ColliderBounds => _colliderBounds;
		public bool IsMoving { get; set; }
		public bool IsAttacking => _isAttacking;


		private Image _gameObject;
		private Vector _position;
		private int _damage = 1;
		private IGameObject _parent;
		private Dictionary<string, double> _colliderBounds;
		private int _deltaX = 16; //расстояние пикселей (за 16мс)
		private bool _isAttacking = false;

		public Range(IGameObject parent)
		{
			var image = SpritesStorage.RangeAttack;

			_gameObject = new Image 
			{
				Source = image,
				Width = image.Width,
				Height = image.Height,
			};

			_parent = parent;

			SceneManager.SceneChanged += () =>
			{
				_isAttacking = false;
				CanvasParameters.RemoveGameObject(this);
			};
		}

		public async Task Execute(int direction) // Лучше использовать Task вместо void
		{
			

			_isAttacking = true;

			_position = new Vector(
				_parent.Position.X + (direction == 1 ? _parent.GameObject.Width : 0),
				_parent.Position.Y + (direction == 1 ? 5 : -10)
			);

			GameObject.UpdateColllisionBounds(Position, out _colliderBounds);
			// Применяем поворот
			_gameObject.RenderTransform = new RotateTransform(direction == -1 ? 180 : 0);

			if (GameObject.Parent == null)
			{
				CanvasParameters.AddGameObject(this);
			}

			await MoveForward(direction);

			CanvasParameters.RemoveGameObject(this);

			_isAttacking = false;
		}

		private async Task MoveForward(int direction)
		{
			while (true)
			{
				if (this.IsObjectOutOfScreen()
					/* || Проверка на коллизии (если объект сталкивается с чем-то) */)
				{
					return; // Завершаем метод
				}

				_position.X += _deltaX * direction;
				GameObject.UpdateColllisionBounds(Position, out _colliderBounds);

				await Task.Delay(16); // 16 мс ~ 60 FPS
			}
		}

		public void SetPosition(double x, double y) { }
	}
}
