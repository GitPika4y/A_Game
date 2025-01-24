using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace A_Game.Classes.Interfaces
{
    internal interface IInputHandler
    {
        void OnKeyDown(Key key);
        void OnKeyUp(Key key);
    }
}
