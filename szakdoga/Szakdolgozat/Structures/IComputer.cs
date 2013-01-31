using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structures
{
    public interface IComputer
    {
        void CreateComputer(int startingStack, int bigBlind);
        int[] ReturnAction(int[] possible, int value);
        int Stack { get; set; }
        void setHoleCards(int[] cards);
        int getHoleCards(int index);
    }
}
