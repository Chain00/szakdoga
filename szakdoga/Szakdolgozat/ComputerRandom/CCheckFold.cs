using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Computer
{
    public class CCheckFold : Structures.IComputer
    {
        public int Stack { get; set; }
        private int bigBlindValue;
        public int[] HoleCards;


        public void CreateComputer(int startingStack, int bigBlind)
        {
            this.bigBlindValue = bigBlind;
            this.Stack = startingStack;
            this.HoleCards = new int[2];
        }

        public int[] ReturnAction(int[] possible, int value)
        {
            int[] array = new int[5];

            if (possible[3] == bigBlindValue / 2)
            {
                array[3] = value;
                return array;
            }
            else if (possible[1] == 1)
            {
                array[1] = 1;
                return array;
            }
            else
            {
                array[0] = 1;
            }
            return array;
        }



        public void setHoleCards(int[] cards)
        {
            HoleCards = cards;
        }

        public int getHoleCards(int index)
        {
            return HoleCards[index];
        }
    }
}
