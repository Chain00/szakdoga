using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Computer
{
    public class ComputerBet: Structures.IComputer
    {
        public int Stack{ get; set;}
        private int bigBlindValue;
        public int[] HoleCards;
        private bool bet = true;
        Random rnd = new Random();

        public int[] ReturnAction(int[] possible, int value)
        {
            int[] array = new int[5];
            int index = 0;

            //for (int i = 0; i < possible.Length; i++)
            //{
            //    if (possible[i] == 1) index = i;
            //}

            //if (possible[3] == 1)
            //{
            //    if (bet)
            //    {
            //        if (Stack >= 500) array[3] = 500;
            //        else array[3] = Stack;
            //        bet = false;
            //    }
            //    else
            //    {
            //        if (possible[1] == 1) array[1] = 1;
            //        bet = true;
            //    }
            //}
            //else if (possible[1] == 1) array[1] = 1;
            //else array[index] = value;

            if (possible[1] == 1) array[1] = 1;
            else  array[3] = value;
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
    





        public void CreateComputer(int startingStack, int bigBlind)
        {
            this.bigBlindValue = bigBlind;
            this.Stack = startingStack;
            this.HoleCards = new int[2];
        }

      

     

      
    }
}
