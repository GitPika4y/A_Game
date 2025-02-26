using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Game.Classes.Interfaces
{
	internal interface IAttack : IGameObject, ICollider
	{
		int Damage { get; }
		IGameObject Parent { get; }
		bool IsAttacking { get; }
		Task Execute(int direction);
	}
}
