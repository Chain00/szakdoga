using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComputerRandom
{
    public class ComputerRandom
    {
        public int Stack{ get; set;}
        private int bigBlindValue;
        public int[] HoleCards;
    
        Random rnd = new Random();

        public ComputerRandom(int startingStack, int bigBlind)
        {
            this.bigBlindValue = bigBlind;
            this.Stack = startingStack;
            this.HoleCards = new int[2];
        }


        public int[] ReturnAction()
        {
            int[] array = new int[4];
            int randAct = rnd.Next(4);

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = 0;

            }

            if(randAct > 2)
            { 
                int randCallValue = rnd.Next(3,Stack/bigBlindValue);
                int callValue = bigBlindValue * randCallValue;
                
                array[randAct] = callValue;
            }
            else array[randAct] = 1;



            return array;

        }
    }
}
