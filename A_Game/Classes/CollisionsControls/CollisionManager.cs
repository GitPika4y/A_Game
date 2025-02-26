using System;
using System.Collections.Generic;
using A_Game.Classes.Interfaces;
using A_Game.Classes.GameObjects;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using A_Game.Classes.Collisions;
using System.Windows.Media;
using A_Game.Classes.GameObjects.Pickups;

namespace A_Game.Classes
{
	internal class CollisionManager
	{
		public static List<ICollider> Colliders { get; private set; } = new List<ICollider>();

		private static bool _shouldBreakLoop;

		static CollisionManager()
		{
			PlayerCollisionHandler.OnPlayerDied += HandleShouldBreakLoop;
			Pickup.OnPickup += HandleShouldBreakLoop;
		}

		private static void HandleShouldBreakLoop()
		{
			_shouldBreakLoop = true;
		}

		/// <summary>
		/// Проверяет столкновения указанного объекта и вызывает соответствующий обработчик.
		/// </summary>
		public static void HandleCollisions(ICollider obj, ICollisionHandler handler)
		{
			if (Colliders.Count == 0 || obj == null || handler == null) return;

			foreach (var collider in Colliders)
			{
				if (collider != obj && CheckCollision(obj, collider, out string collisionDirection))
				{
					handler.ProcessCollision(obj, collider, collisionDirection);

				}
				if (_shouldBreakLoop) break;
			}

			_shouldBreakLoop = false;
		}

		/// <summary>
		/// Проверяет, пересекаются ли два объекта.
		/// </summary>
		private static bool CheckCollision(ICollider a, ICollider b, out string collisionDirection)
		{
			collisionDirection = null;

			bool isColliding =
				a.ColliderBounds["Left"] < b.ColliderBounds["Right"] &&
				a.ColliderBounds["Right"] > b.ColliderBounds["Left"] &&
				a.ColliderBounds["Top"] > b.ColliderBounds["Bottom"] &&
				a.ColliderBounds["Bottom"] < b.ColliderBounds["Top"];

			if (!isColliding) return false;

			// Определение направления коллизии
			double overlapTop = b.ColliderBounds["Bottom"] - a.ColliderBounds["Top"];
			double overlapBottom = a.ColliderBounds["Bottom"] - b.ColliderBounds["Top"];
			double overlapLeft = b.ColliderBounds["Right"] - a.ColliderBounds["Left"];
			double overlapRight = a.ColliderBounds["Right"] - b.ColliderBounds["Left"];

			double smallestOverlap = Math.Min(Math.Min(overlapTop, overlapBottom), Math.Min(overlapLeft, overlapRight));

			if (smallestOverlap == overlapTop) collisionDirection = "Top";
			else if (smallestOverlap == overlapBottom) collisionDirection = "Bottom";
			else if (smallestOverlap == overlapLeft) collisionDirection = "Left";
			else if (smallestOverlap == overlapRight) collisionDirection = "Right";

			return true;
		}
	}
}
