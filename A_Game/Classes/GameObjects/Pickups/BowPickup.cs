using A_Game.Classes.Helpers;
using A_Game.Data.ProgressEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace A_Game.Classes.GameObjects.Pickups
{
	internal class BowPickup : Pickup
	{
		public BowPickup(Vector position) : base(position)
		{
			var image = SpritesStorage.BowPickup;

			_gameObject = new Image
			{
				Source = image,
				Width = image.Width,
				Height = image.Height,
			};

			GameObject.UpdateColllisionBounds(position, out _colliderBounds);
		}

		public override void PickUp(Player player)
		{
			base.PickUp(player);
			ProgressEventsData.SetEvent(Events.BowEquipped, true);
			player.SetNewAttack(this);
			_ = InteractableHelper.ShowMessageWindow(this, "Чтобы стрелять нажми K", new Vector(-30, 10));
			CanvasParameters.RemoveGameObject(this);
		}
	}
}
