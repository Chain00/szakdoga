using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console
{

   
    class Program
    {
       /* classoltatva
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
        * */

        public class Round
        {
            private int s_pStack;

            public int S_pStack
            {
                get { return s_pStack; }
                set { s_pStack = value; }
            }
            private int s_cStack;

            public int S_cStack
            {
                get { return s_cStack; }
                set { s_cStack = value; }
            }
            private int s_pot;

            public int S_pot
            {
                get { return s_pot; }
                set { s_pot = value; }
            }
            private List<int> s_licit;

            public List<int> S_licit
            {
                get { return s_licit; }
                set { s_licit = value; }
            }
            private int s_smallBlind;

            public int S_smallBlind
            {
                get { return s_smallBlind; }
                set { s_smallBlind = value; }
            }
            private int s_bigBlind;

            public int S_bigBlind
            {
                get { return s_bigBlind; }
                set { s_bigBlind = value; }
            }
            private int[] s_flop;

            public int[] S_flop
            {
                get { return s_flop; }
                set { s_flop = value; }
            }
            private int s_turn;

            public int S_turn
            {
                get { return s_turn; }
                set { s_turn = value; }
            }
            private int s_river;

            public int S_river
            {
                get { return s_river; }
                set { s_river = value; }
            }


            public Round()
            {
                List<int> s_licit = new List<int>();

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
            

            using (StreamWriter w = File.AppendText("HandHistory.txt"))
            {
                w.WriteLine(message);
                w.Flush();
                w.Close();
            }


          

        }


        public static int getAction( int[] possible, int callValue, int stack, int bigBlindValue)
        {
            String read; 
            int ret = -1, option, bet;
            bool b = true;

            System.Console.WriteLine();

            if (possible[0] == 1) System.Console.WriteLine("\t0. Fold");
            if (possible[1] == 1) System.Console.WriteLine("\t1. Check");
            if (possible[2] == 1) System.Console.WriteLine("\t2. Bet");
            if (possible[3] == 1) System.Console.WriteLine("\t3. Call");
            if (possible[4] == 1) System.Console.WriteLine("\t4. Raise");


            while (b)
            {
                
                try
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

                        case 3: if (possible[option] == 1)
                            {
                                ret = callValue;
                                b = false;
                            }
                            break;

                        case 2: if (possible[option] == 1)
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
                                    ret = bet + callValue;
                                    b = false;
                                }
                            }
                            break;

                    }
                    
                }

                catch (Exception e)
                {
                    System.Console.WriteLine("Wrong input! Please try again!");
                }
            
            }

            if (ret > stack) ret = stack;
           
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
                try
                {
                    read = System.Console.ReadLine();
                    bet = Convert.ToInt32(read);

                    if (bet == 0)
                    {
                        b = false;
                        ret = 0;
                    }

                    else if ((bet >= callValue * 2) || (bet == stack))
                    {
                        b = false;
                        ret = bet;
                    }
                    else System.Console.WriteLine("Wrong input! Please try again!");

                }

                catch (Exception e)
                {
                    System.Console.WriteLine("Wrong input! Please try again!");
                }

                if (ret > stack)
                {
                    b = true;
                    System.Console.WriteLine("You don't have enough money!");
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
                try
                {
                    read = System.Console.ReadLine();
                    bet = Convert.ToInt32(read);

                    if (bet == 0)
                    {
                        b = false;
                        ret = 0;
                    }

                    if ((bet >= bigBlindValue))
                    {
                        b = false;
                        ret = bet;
                    }

                }

                catch (Exception e)
                {
                    System.Console.WriteLine("Wrong input! Please try again!");
                }

                if (ret > stack)
                {
                    b = true;
                    System.Console.WriteLine("You don't have enough money!");
                }
            }


            return ret;
        }





        private static int readComputer()
        {

            int computer = 0;
            bool b = true;
            string read;

            while (b)
            {

                System.Console.WriteLine("Please choose a computer!");
                System.Console.WriteLine("(1). Random(Computer) ");
                System.Console.WriteLine("(2). Calling Station(Computer) ");

                try
                {
                    read = System.Console.ReadLine();
                    computer = Convert.ToInt32(read);
                    //új játékos esetén bővíteni
                    switch (computer)
                    {
                        case 1: b = false;
                            break;
                        case 2: b = false;
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Wrong Input! Please try again!");
                }
            }

            return computer;

        }

        private static int readPlayer()
        {

            int  playerMode = 0;
            bool b = true;
            string read; 

            while (b)
            {

                System.Console.WriteLine("Please choose a game option!");
                System.Console.WriteLine("(1). Player vs Random(Computer) ");
                System.Console.WriteLine("(2). Player vs Calling Station(Computer) ");

                try
                {
                    read = System.Console.ReadLine();
                    playerMode = Convert.ToInt32(read);
                    //új játékos esetén bővíteni
                    switch (playerMode)
                    {
                        case 1: b = false;
                            break;
                        case 2: b = false;
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Wrong Input! Please try again!");
                }
            }

            return playerMode;
     
        }

        
        private static int readMode()
        {

            int gameMode = 0;
            bool b = true;
            string read;

            while (b)
            {

                System.Console.WriteLine("Please choose a game mode!");
                System.Console.WriteLine("(1). Player vs Computer ");
                System.Console.WriteLine("(2). Computer vs Computer ");

                try
                {
                    read = System.Console.ReadLine();
                    gameMode = Convert.ToInt32(read);

                    if (gameMode == 1 ||gameMode == 2) b = false;

                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Wrong Input! Please try again!");
                }
            }

            return gameMode;
   
        }

      
       
        
        static void Main(string[] args)
        {
            int gameMode = 0, player = 0, computer1 = 0, computer2 = 0;

           
            

            System.Console.WriteLine("Welcome!");

            gameMode = readMode();
            StringBuilder str = new StringBuilder();
            switch (gameMode)
            {
                case 1:
                    {
                        player = readPlayer();

                        Dealer.Dealer dealer = new Dealer.Dealer(player);
                        dealer.MyPrintMethod = write;
                        dealer.MyReadMethod = getAction;
                         dealer.StartGame();
                        //test









                        //int[] t2 = { 12, 24, 23, 3, 0, 14, 2 };
                        //Random rnd = new Random();
                        //for (int j = 0; j < 10000; j++)
                        //{
                        //    int[] t = new int[7]; //= { 10, 38, 25, 39, 22, 44, 3 };
                        //    for (int h = 0; h < 7; h++)
                        //    {

                        //        t[h] = rnd.Next(0, 51);
                        //        str.AppendLine(String.Format("{0} | {1}, {2} ", (t[h]), dealer.ValueToRank(Convert.ToInt32(t[h])), dealer.ValueToSuit(Convert.ToInt32(t[h]))));

                        //    }
                        //    string[] result = dealer.BestHand(t);


                        //    for (int i = 0; i < result.Length - 1; i++)
                        //    {
                        //        str.AppendLine(String.Format("{0} of {1} ", dealer.ValueToRank(Convert.ToInt32(result[i])), dealer.ValueToSuit(Convert.ToInt32(result[i]))));
                        //    }

                        //    str.AppendLine(result[5]);
                        //    str.AppendLine("-------------------------------------------------");






                        //int[] t2 = { 4, 17, 40, 27, 5, 18, 3 };

                        //for (int j = 0; j < 1; j++)
                        //{

                        //    for (int h = 0; h < 7; h++)
                        //    {


                        //        write(String.Format("{0}, {1} ", (t2[h]), dealer.ValueToRank(Convert.ToInt32(t2[h]))));

                        //    }
                        //    string[] result = dealer.BestHand(t2);


                        //    for (int i = 0; i < result.Length - 1; i++)
                        //    {
                        //        write(String.Format("{0} of {1} ", dealer.ValueToRank(Convert.ToInt32(result[i])), dealer.ValueToSuit(Convert.ToInt32(result[i]))));
                        //    }

                        //    write(result[5]);
                        //    write("-------------------------------------------------");
                            








                        
                    }
                    break;
                case 2:
                    {
                        computer1 = readComputer();
                        computer2 = readComputer();


                        Dealer.Simulate simulate = new Dealer.Simulate(computer1, computer2);
                        simulate.MyPrintMethod = write;
                        simulate.StartGame();     
     
                    }
                    break;

                default:
                    break;
            }

            write(str.ToString());
            System.Console.WriteLine("Game Over!");
            System.Console.ReadKey();

        }

        
    }
}
