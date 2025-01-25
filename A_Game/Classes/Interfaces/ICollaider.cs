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
        Image GameObject { get; }
        Vector Position { get; }
        Dictionary<string, double> ColliderBounds {get;}
    }
}
