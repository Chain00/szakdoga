using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console
{

   
    class Program
    {
       
         public struct Round
        {
           public int s_pStack;
           public int s_cStack;
           public int s_pot;
           public List<int> s_licit;
           public int s_smallBlind;
           public int s_bigBlind;
           public int[] s_flop;
           public int s_turn;
           public int s_river;
            


           public void potNewValue(int value)
           {
               s_pot = value;
           }

           public void pStackNewValue(int value)
           {
               s_pStack = value;
           }
           public void cStackNewValue(int value)
           {
               s_cStack = value;
           }
        
         }

        private static void kiir(int[] tomb)
        {
            foreach(var item in tomb)
                System.Console.WriteLine(item);


        }

        public static void write(string message)
        {
                
            System.Console.WriteLine(message);

        }


        public static int getAction(  int[] possible, int callValue, int stack, int bigBlindValue)
        {
            String read; 
            int ret = -1, option, bet;
            bool b = true;

            System.Console.WriteLine();

            if(possible[0] == 1)    System.Console.WriteLine("0. Fold");
            if(possible[1] == 1)    System.Console.WriteLine("1. Check");
            if(possible[2] == 1)    System.Console.WriteLine("2. Call");
            if(possible[3] == 1)    System.Console.WriteLine("3. Bet");
            if(possible[4] == 1)    System.Console.WriteLine("4. Raise");


            while (b)
            {
                read = System.Console.ReadLine();
                option = Convert.ToInt32(read);

                switch (option)
                {
                    case 0: if (possible[option] == 1)
                        {
                            ret = 0;
                            b = false;
                        }
                        break;

                    case 1: if (possible[option] == 1)
                        {
                            ret = 1;
                            b = false;
                        }
                        break;

                    case 2: if (possible[option] == 1)
                        {
                            ret = 2;
                            b = false;
                        }
                        break;

                    case 3: if (possible[option] == 1)
                        {
                            bet = readBet(stack, bigBlindValue);
                            if (bet != 0)
                            {
                                ret = bet;
                                b = false;
                            }
                        }
                        break;

                    case 4: if (possible[option] == 1)
                        {
                            bet = raiseBet(stack, bigBlindValue, callValue);
                            if (bet != 0)
                            {
                                ret = bet;
                                b = false;
                            }
                        }
                        break;

                }
            }

            return ret;
        }


        public static int raiseBet(int stack, int bigBlindValue, int callValue)
        {
            int ret = 0, bet;
            bool b = true;
            String read;

            System.Console.WriteLine("Enter your bet!");
            System.Console.WriteLine("0. Cancel.");

            while (b)
            {
                read = System.Console.ReadLine();
                bet = Convert.ToInt32(read);

                if (bet == 0)
                {
                    b = false;
                    ret = 0;
                }

                if ((bet >= callValue*2) || (bet <= stack))
                {
                    b = false;
                    ret = bet;
                }


            }


            return ret;
        }




        public static int readBet(int stack, int bigBlindValue)
        {
            int  ret = 0, bet;
            bool b = true ;
            String read;

            System.Console.WriteLine("Enter your bet!");
            System.Console.WriteLine("0. Cancel.");

            while (b)
            {
                read = System.Console.ReadLine();                
                bet = Convert.ToInt32(read);

                if (bet == 0)
                {
                    b = false;
                    ret = 0;
                }

                if ((bet >= bigBlindValue) && (bet <= stack))
                {
                    b = false;
                    ret = bet;
                }
                
            
            }


            return ret;
        }
        
        
        
        static void Main(string[] args)
        {
            int gameMode = 0;
            bool b = true;
            string read;
      
          

            System.Console.WriteLine("Welcome!");
           
 

            while (b)
            {

                System.Console.WriteLine("Please choose a game option!");
                System.Console.WriteLine("(1). Player vs RandomComp ");
                read = System.Console.ReadLine();
                gameMode = Convert.ToInt32(read);

                switch (gameMode)
                {
                    case 1:  b = false;
                            break;
                    default:
                            break;
                }
            }

            Dealer.Dealer dealer = new Dealer.Dealer(gameMode);
            dealer.MyPrintMethod = write;
            dealer.MyReadMethod = getAction;
            dealer.StartGame();     
     
            




            System.Console.ReadKey();

        }
    }
}
