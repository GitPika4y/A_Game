using A_Game.Classes;
using A_Game.Classes.GameObjects.PlayerControl;
using A_Game.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


internal class RangeAttackCollisionHandler : ICollisionHandler
{
	public void ProcessCollision(ICollider obj, ICollider collidedObject, string collisionDirection)
	{
		if (!(obj is Range range)) return;
	}
}

