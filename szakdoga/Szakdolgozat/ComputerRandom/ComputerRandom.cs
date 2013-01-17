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


        public int[] ReturnAction( int[] possible, int value)
        {
            int[] array = new int[5];
            int rand;
            int randBetValue;
            int betValue;

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = 0;
            }
            
            
            while (true)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (possible[i] == 1)
                    {
                        rand = rnd.Next(5);
                        {
                            if ((i == 2) && (rand == 2))
                            {
                                if ((Stack / bigBlindValue) <= 3) array[i] = Stack;
                                else
                                {
                                    randBetValue = rnd.Next(3, Stack / bigBlindValue);
                                    betValue = bigBlindValue * randBetValue;
                                    array[i] = betValue ;
                                }
                                return array;
                            }
                            else if ((i == 4) && (rand == 4))
                            {
                                if (((Stack - value) / value) <= 3) array[i] = Stack;
                                else
                                {
                                    randBetValue = rnd.Next(2, (Stack - value) / value);
                                    betValue = value * randBetValue;
                                    array[i] = betValue + value;
                                }
                                return array;
                            }
                            else if ((i == 3) && (rand == 3))
                            {
                                array[i] = Stack >= value ? value : Stack;
                                return array;
                            }
                            else if ((i == 0) && (rand == 0))
                            {
                                array[i] = 1;
                                return array;
                            }

                            else if ((i == 1) && (rand == 1))
                            {
                                array[i] = 1;
                                return array;

                            }//eredeti 
                        }
                    }
                           

                            ////teszt miatt
                            //array[4] =600;
                            //return array;
                        
                    
                }
            }
        }



    }
}
