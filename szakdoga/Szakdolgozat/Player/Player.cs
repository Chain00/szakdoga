using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Player
{
    public class Player
    {
        public int Stack { get; set; }
        public int[] HoleCards;

        public Player(int startingStack)
        {
            this.Stack = startingStack;
            this.HoleCards = new int[2];
        }

    }
}
