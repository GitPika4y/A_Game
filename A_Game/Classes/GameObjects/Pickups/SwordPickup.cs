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
	internal class SwordPickup : Pickup
	{
		public SwordPickup(Vector position) : base(position)
		{
			var image = SpritesStorage.SwordPickup;

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
			ProgressEventsData.SetEvent(Events.SwordEquipped, true);
			player.SetNewAttack(this);
			_ = InteractableHelper.ShowMessageWindow(this, "Чтобы ударить нажми J", new Vector(-30, 10));
			CanvasParameters.RemoveGameObject(this);
			base.PickUp(player);
		}
	}
}
