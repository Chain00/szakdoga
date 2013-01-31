using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Computer
{
    public class ComputerCallingStation: Structures.IComputer
    {

        public int Stack{ get; set;}
        private int bigBlindValue;
        public int[] HoleCards;
    
        Random rnd = new Random();


        public void CreateComputer(int startingStack, int bigBlind)
       
        {
            this.bigBlindValue = bigBlind;
            this.Stack = startingStack;
            this.HoleCards = new int[2];
        }


        public int[] ReturnAction(int[] possible, int value)
        {
            int[] array = new int[5];
            int index = 0;
            
            for (int i = 0; i < possible.Length; i++)
            {
                if (possible[i] == 1) index = i;
            }

            if (possible[1] == 1) array[1] = 1;
            else array[index] = value;

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
