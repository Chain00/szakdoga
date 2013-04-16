using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Computer
{
    public class C1BetCall : Structures.IComputer
    {
        public int Stack { get; set; }
        private int bigBlindValue;
        public int[] HoleCards;
        private bool bet = true;


        public void CreateComputer(int startingStack, int bigBlind)
        {
            this.bigBlindValue = bigBlind;
            this.Stack = startingStack;
            this.HoleCards = new int[2];
        }

        public int[] ReturnAction(int[] possible, int value)
        {
            int[] array = new int[5];


            if (possible[2] == 1  && bet)
	        {
                
                if (bigBlindValue * 3 < Stack)
                {
                    array[2] = bigBlindValue * 3;
                    bet = false;
                }
                else  array[1] = 1;

                               
                return array;
		 
	        }

            else if (possible[3] == 1 )
            {
                if (value > Stack) array[3] = Stack;
                else array[3] = value;
                bet = true;

                return array;
            }
            else if (possible[1] == 1)
            {
                array[1] = 1;
                return array;
            }
            array[0] = 1;
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
