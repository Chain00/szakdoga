using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Computer
{

    public class ComputerRandomV2 : Structures.IComputer
    {
        public int Stack { get; set; }
        private int bigBlindValue;
        public int[] HoleCards;

        Random rnd = new Random();


        public void CreateComputer(int startingStack, int bigBlind)
        {

            this.bigBlindValue = bigBlind;
            this.Stack = startingStack;
            this.HoleCards = new int[2];
        }

        public void setHoleCards(int[] cards)
        {
            HoleCards = cards;
        }

        public int getHoleCards(int index)
        {
            return HoleCards[index];
        }

        //0: fold	1: check	2: call		3: bet		4: raise
        //mi micsoda
        public int[] ReturnAction(int[] possible, int value)
        {
            int[] array = new int[5];
            int rand;
            int randBet;
            int betValue;
            int possibleNumber = 0;
            int remainingStack = Stack - value;
            List<int> possibleList = new List<int>();

            possibleNumber = 0;
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = 0;
                if (possible[i] == 1)
                {
                    possibleList.Add(i);
                    possibleNumber++;
                }
            }
            rand = rnd.Next(possibleNumber);


            switch (possibleList[rand])
            {
                case 0: array[0] = 1;
                    break;

                case 1: array[1] = 1;
                    break;

                case 2: if (remainingStack / bigBlindValue > 0)
                    {
                        randBet = rnd.Next(1, remainingStack / bigBlindValue);
                        if (randBet * bigBlindValue > Stack) array[2] = 60;
                        else if (randBet * bigBlindValue < Stack) array[2] = randBet * bigBlindValue;
                    }
                    else array[0] = 1;
                 
                    break;

                case 3: if (value >= Stack) array[3] = Stack;
                    else array[3] = value;
                    break;

                case 4: if (remainingStack - value < Stack && remainingStack - value > 0)
                    {
                        randBet = rnd.Next(1,(remainingStack - value) / 2);
                        betValue = 2 * value + bigBlindValue * randBet;
                        if (betValue <= Stack) array[4] = betValue;
                        else array[0] = 1;
                    }
                    else array[0] = 1;
                    break;
            }




            return array;



        }


    }

}

