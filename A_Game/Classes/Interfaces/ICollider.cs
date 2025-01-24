using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace A_Game.Classes.Interfaces
{
    internal interface ICollider
    {
        Vector Position { get;}
        Vector Size { get;}

        bool CheckCollision (ICollider other);
        void OnCollision(ICollider other);

    }
}
