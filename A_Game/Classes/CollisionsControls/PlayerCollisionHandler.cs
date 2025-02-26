using A_Game.Classes.GameObjects;
using A_Game.Classes.Helpers;
using A_Game.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using A_Game.Classes.GameObjects.Pickups;

namespace A_Game.Classes.Collisions
{
	internal class PlayerCollisionHandler : ICollisionHandler
	{
		public static event Action OnPlayerDied;

		private Task _currentTask = null;

		public void ProcessCollision(ICollider obj, ICollider collidedObject, string collisionDirection)
		{
			if (!(obj is Player player)) return;

			switch (collidedObject)
			{
				case Platform platform:
					HandlePlatformCollision(player, platform, collisionDirection);
					break;
				case DeathZone deathZone:
					HandleDeathZoneCollision(player);
					break;
				case SpawnPoint spawnPoint:
					HandleSpawnPointCollision(player, spawnPoint);
					break;
				case Pickup pickup:
					pickup.PickUp(player);
					break;
			}
		}

		private void HandlePlatformCollision(Player player, Platform platform, string collisionDirection)
		{
			switch (collisionDirection)
			{
				case "Top":
					player.SetPosition(player.Position.X, platform.ColliderBounds["Top"]);
					if (platform is MovingPlatform)
					{
						player.SetPosition(platform.Position.X, platform.ColliderBounds["Top"]);
					}
					break;
				case "Bottom":
					player.SetPosition(player.Position.X, platform.ColliderBounds["Bottom"] - player.GameObject.Height);
					break;
				case "Left":
					player.SetPosition(platform.ColliderBounds["Left"], player.Position.Y);
					break;
				case "Right":
					player.SetPosition(platform.ColliderBounds["Right"], player.Position.Y);
					break;
			}
			player.IsOnGround = true;
		}

		private void HandleDeathZoneCollision(Player player)
		{
			OnPlayerDied?.Invoke();
			player.SetPosition(CanvasParameters.SpawnPoint.Position.X, CanvasParameters.SpawnPoint.Position.Y);
		}

		private async void HandleSpawnPointCollision(Player player, SpawnPoint spawnPoint)
		{
			if (_currentTask != null || CanvasParameters.SpawnPoint.Equals(spawnPoint)) return;

			_currentTask = InteractableHelper.
				ShowMessageWindow(spawnPoint,
							"Сохраниться?(Enter)",
							new Vector(-30,10));
			await _currentTask;
			_currentTask = null;
		}


	}
}
