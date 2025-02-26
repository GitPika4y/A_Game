using A_Game.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace A_Game.Classes.GameObjects.Pickups
{
	public abstract class Pickup : IGameObject, ICollider
	{
		public Image GameObject => _gameObject;
		public Vector Position => _position;
		public Dictionary<string, double> ColliderBounds => _colliderBounds;

		public static event Action OnPickup;


		protected Image _gameObject;
		protected Vector _position;
		protected Dictionary<string, double> _colliderBounds = new Dictionary<string, double>();

		protected Pickup (Vector position)
		{
			_position = position;
		}

		public virtual void PickUp(Player player)
		{
			OnPickup?.Invoke();
		}
	}
}
