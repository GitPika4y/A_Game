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
	internal class Melee : IAttack, ICollider
	{
		public Image GameObject => _gameObject;
		public Vector Position => _position;
		public int Damage => _damage;
		public IGameObject Parent => _parent;
		public Dictionary<string, double> ColliderBounds => _colliderBounds;
		public bool IsAttacking => _isAttacking;

		private Image _gameObject;
		private Vector _position;
		private int _damage = 2;
		private IGameObject _parent;
		private Dictionary<string, double> _colliderBounds;
		private bool _isAttacking = false;

		public Melee(IGameObject parent)
		{
			var image = SpritesStorage.MeleeAttack;

			_gameObject = new Image 
			{
				Source  = image,
				Width= image.Width,
				Height= image.Height,
			};

			_parent = parent;

			SceneManager.SceneChanged += () =>
			{
				_isAttacking = false;
				CanvasParameters.RemoveGameObject(this);
			};
		}

		public async Task Execute(int direction)
		{
			_isAttacking = true;

			_position = new Vector(
				_parent.Position.X + (direction == 1 ? _parent.GameObject.Width : 0),
				_parent.Position.Y + (direction == 1 ? 5: -10)
			);

			GameObject.UpdateColllisionBounds(Position, out _colliderBounds);

			// Применяем поворот (если направление -1, вращаем на 180 градусов)
			_gameObject.RenderTransform = new RotateTransform(direction == -1 ? 180 : 0);

			// Убедимся, что объект еще не добавлен в Canvas
			if (GameObject.Parent == null)
			{
				CanvasParameters.AddGameObject(this);
			}

			await Task.Delay(250); //Время атаки

			CanvasParameters.RemoveGameObject(this);

			await Task.Delay(150); //Задержка между атаками

			_isAttacking  = false;
		}

	}
}
