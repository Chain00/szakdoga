using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Dealer
{

   
    public class Dealer
    {
      
        private const int startingStack = 2000;
        private const int bigBlindValue = 20;
        private const int smallBlindValue = 10;
         

        public int GameMode { get; set; }
        public int[] Deck { get; set; }
        public int Pot { get; set; }
        public printMethod MyPrintMethod { get; set; }
        public readMethod MyReadMethod { get; set; }


        private String bigBlind;
        private String smallBlind;
        private int next;
        private int deckIndex;
        private int handIndex;
        private int gameWinner;
        private int handWinner;
        private int pot;
        private int computerStack = startingStack;
        private int playerStack = startingStack;
        private List<Round> actions;
        private int returnedAction;
        private int handStatusListIndex;
        private int[] theFlop;
        private int theTurn;
        private int theRiver;
        private int[] possibleActions;
        public delegate void Del(string message);
        Random rnd = new Random();

        Player.Player player;
        ComputerRandom.ComputerRandom computer;


        public delegate void printMethod(string s);
        public delegate int readMethod(int[] possible, int callValue, int stack, int bigBlindValue);




        //TODO struct classra cserélése
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
           public void SetLicit(List<int> list)
           {
               s_licit = list;
           }
           
          
         

          
        }

        //kontruktor, játékosok, pakli inicializálása
        public Dealer(int gameMode)
        {
            Player.Player player_ = new Player.Player(startingStack);
            ComputerRandom.ComputerRandom computer_ = new ComputerRandom.ComputerRandom(startingStack, bigBlindValue);
            int[] deck = new int[52];

            for (int i = 0; i < deck.Length; i++)
            {
                deck[i] = i+1;
            }
                
            this.Deck = deck;
            this.player = player_;
            this.computer = computer_;
          
        }

        // pakli keverés
        public void Shuffle(int[] deck)
        {
            int change, index;
            for (int i = 0; i < deck.Length; i++)
            {
                index = rnd.Next(0,52);
                change = deck[i];
                deck[i] = deck[index];
                deck[index] = change;

            }
        }

        //játék indítása, alapértékek inicializálása
        public void StartGame()
        {
            int pSCard, cSCard;
            Shuffle(Deck);
            deckIndex = 0;
            gameWinner = 0;
            pSCard = DealOneCard()%13;
            cSCard = DealOneCard()%13;

            next = pSCard > cSCard ? 1 : -1;
            smallBlind = intToName(next);
            bigBlind = intToName(next * -1);
            next*= -1; 

            ConsoleWrite(String.Format("Choosing dealer....\nPlayer : {0} of {1}\nComputer : {2} of {3}", ValueToRank(pSCard), ValueToSuit(pSCard), ValueToRank(cSCard), ValueToSuit(cSCard) ));
            ConsoleWrite(String.Format("Small blind: {0} \nBigBlind: {1}", smallBlind, bigBlind));
         
            
         
               
                StartHand();
         
        }

        //leosztás indítása
        public void StartHand()
        {
            bool nextStreet = false;
            handWinner = 0;
            Shuffle(Deck);
            deckIndex = 0;
            //preflop 1.action
            handIndex = -10;
 
            DealHoleCards();
            ConsoleWrite(String.Format("Player : {0} of {1}  : {2} of {3}", ValueToRank(player.HoleCards[0]), ValueToSuit(player.HoleCards[0]), ValueToRank(player.HoleCards[1]), ValueToSuit(player.HoleCards[1])));
            ConsoleWrite(String.Format("Computer : {0} of {1}  : {2} of {3}", ValueToRank(computer.HoleCards[0]), ValueToSuit(computer.HoleCards[0]), ValueToRank(computer.HoleCards[1]), ValueToSuit(computer.HoleCards[1])));
            ConsoleWrite("\n\n");

            int asd = 0;
            //preflop
            while ((handWinner == 0) && (!nextStreet))
            {
                //hand állapot elkészítése
                HandStatus();

   
         
          

                //adatok átadása a játékosoknak, return reakció 
                SendHandStatus();
            

  
                CheckHandStatus();
                asd++;
                if (asd > 1)
                {
              /*      foreach (var item in actions[handIndex].s_licit)
                    {
                        ConsoleWrite(String.Format("item {0}", item));
                    }*/
                }

            }

            //flop
            DealFlop();
            nextStreet = false;
            while ((handWinner == 0) && (!nextStreet))
            {
                HandStatus();
                SendHandStatus();
                CheckHandStatus();
            }

            //turn
            DealOneCard();
            nextStreet = false;
            while ((handWinner == 0) && (!nextStreet))
            {
                HandStatus();
                SendHandStatus();
                CheckHandStatus();
            }

            //river
            DealOneCard();
            nextStreet = false;
            while ((handWinner == 0) && (!nextStreet))
            {
                HandStatus();
                SendHandStatus();
                CheckHandStatus();
            }
            
            
            
            

            
            foreach(var item in actions)
            {
                  ConsoleWrite(String.Format("preflop"));
                  ConsoleWrite(String.Format("BB {0}", item.s_bigBlind));
                  ConsoleWrite(String.Format("SB {0}", item.s_smallBlind));
                  ConsoleWrite(String.Format("pot {0}", item.s_pot));
                  ConsoleWrite(String.Format("cstack {0}", item.s_cStack));
                  ConsoleWrite(String.Format("pstack {0}", item.s_pStack));

                  ConsoleWrite("\n\n");
                  ConsoleWrite(String.Format("flop 1 {0}", theFlop[0]));
                  ConsoleWrite(String.Format("flop 2 {0}", theFlop[1]));
                  ConsoleWrite(String.Format("flop 3 {0}", theFlop[2]));
                  ConsoleWrite(String.Format("index {0}", deckIndex));
                  ConsoleWrite(String.Format("deck {0}", Deck[deckIndex]));
            }

         

       
            

            
        }


        public void CheckHandStatus()
        {
            Round actual = actions.Last<Round>();
            
            if (handStatusListIndex >= 2)
            {

                if (actual.s_licit[handStatusListIndex] == -1)
                {
                    handWinner = next;
                }
                if (actual.s_licit[handStatusListIndex] == actual.s_licit[handStatusListIndex - 1])
                {
                    SetHandIndex();
                }
                if (actual.s_licit[handStatusListIndex] == 0)
                {
                    SetHandIndex();
                }
            }

        }



        public void SetHandIndex()
        {
            switch (handIndex)
            {
                case 0: handIndex = -10;
                    break;
                case 1: handIndex = -20;
                    break;
                case 2: handIndex = -30;
                    break;
               
            }
        }


        public void DealFlop()
        {
            int[] flop = new int[3]; 
            deckIndex++;
            flop[0] = Deck[deckIndex++];
            flop[1] = Deck[deckIndex++];
            flop[2] = Deck[deckIndex++];
            theFlop = flop;
        }

        // aktuális állapot kiküldése a soron következő játékosnak
        public void SendHandStatus()
        {

            if (next == 1)
            {
                if (actions[handIndex].s_licit == null)
                {
                    possibleActions = CalculatePossibleActions(true);
                }
                else  possibleActions = CalculatePossibleActions(false);

                ConsoleRead(possibleActions, CalculateCallValue(), playerStack, bigBlindValue);
               
            }
            else
            {
                int[] returnedActionArray = computer.ReturnAction();
                foreach (var item in returnedActionArray)
                {
                    if (item > 0) returnedAction = item;
      
                }
                
            }

        }

        public int[] CalculatePossibleActions(bool isNull)
        {

            Round actual = actions.Last<Round>();
            int[] possible = new int[5];
            
            //az actions lista utolsó elemének kiválasztása után, a licitkör vizsgálata, és ez alapján a lehetséges akciókat tároló tömb feltöltése

            if (isNull)
	        {
                possible[0] = 1;
                possible[1] = 1;
                possible[2] = 1;
                possible[3] = 0;
                possible[4] = 0;

                return possible;
	        }
  
          
            if (actual.s_licit[handStatusListIndex] == 0)
            {
                possible[0] = 1;
                possible[1] = 1;
                possible[2] = 1;
                possible[3] = 0;
                possible[4] = 0;
             
            }

            if (actual.s_licit[handStatusListIndex] >= 20)
            {
                possible[0] = 1;
                possible[1] = 0;
                possible[2] = 0;
                possible[3] = 1;
                possible[4] = 1;
             
            }

            return possible;

        }

        public int CalculateCallValue()
        {
            Round actual = actions.Last<Round>();

            if (possibleActions[1] == 1) return 0;
            else return actual.s_licit[handStatusListIndex];
        
        }

        public void HandStatus()
        {
            //preflop 1. akció
            if (handIndex == -10)
            {
                
                List<Round> _actions = new List<Round>();
             //   List<int> licit = new List<int>();
                Round preflop = new Round();

                preflop.s_bigBlind = next*-1;
                preflop.s_smallBlind = next;
                preflop.s_cStack = computerStack;
                preflop.s_pStack = playerStack;
                preflop.s_pot = pot;
                handStatusListIndex = 0;
            //    preflop.SetLicit(licit);
      
                
                _actions.Add(preflop);
                handIndex = 0;
                actions = _actions;
                return;
            }

            // preflop akció 
            if (handIndex == 0)
            {
                List<int> licit = new List<int>();
                actions[handIndex].potNewValue(pot);
                actions[handIndex].pStackNewValue(playerStack);
                actions[handIndex].cStackNewValue(computerStack);
                licit.Add(returnedAction);
                actions[handIndex].SetLicit(licit);
               // actions[handIndex].s_licit.Add(returnedAction);//vmi cinkes
                handStatusListIndex++;
                return;

            }

            //flop 1.akció 
            if (handIndex == -1)
            {
                
                Round flop = new Round();

                flop.s_bigBlind = next * -1;
                flop.s_smallBlind = next;
                flop.s_cStack = computerStack;
                flop.s_pStack = playerStack;
                flop.s_flop = theFlop;
                flop.s_pot = pot;
                handStatusListIndex = 0;

          

                actions.Add(flop);
                handIndex = 1;

            }

            //flop akció
            if (handIndex == 1)
            {
                List<int> licit = actions[handIndex].s_licit;

                actions[handIndex].potNewValue(pot);
                actions[handIndex].pStackNewValue(playerStack);
                actions[handIndex].cStackNewValue(computerStack);
                licit.Add(returnedAction);
                actions[handIndex].SetLicit(licit);
                handStatusListIndex++;

             
            }

            //turn 1. akció
            if (handIndex == -2)
            {

                Round turn = new Round();

                turn.s_bigBlind = next * -1;
                turn.s_smallBlind = next;
                turn.s_cStack = computerStack;
                turn.s_pStack = playerStack;
                turn.s_pot = pot;
                turn.s_turn = theTurn;
                handStatusListIndex = 0;



                actions.Add(turn);
                handIndex = 2;

            }

            //turn akció
            if (handIndex == 2)
            {
                List<int> licit = actions[handIndex].s_licit;

                actions[handIndex].potNewValue(pot);
                actions[handIndex].pStackNewValue(playerStack);
                actions[handIndex].cStackNewValue(computerStack);
                licit.Add(returnedAction);
                actions[handIndex].SetLicit(licit);
                handStatusListIndex++;


            }


            //river 1. akció
            if (handIndex == -3)
            {

                Round river = new Round();

                river.s_bigBlind = next * -1;
                river.s_smallBlind = next;
                river.s_cStack = computerStack;
                river.s_pStack = playerStack;
                river.s_pot = pot;
                river.s_river = theRiver;
                handStatusListIndex = 0;



                actions.Add(river);
                handIndex = 1;

            }

            //river akció
            if (handIndex == 3)
            {
                List<int> licit = actions[handIndex].s_licit;

                actions[handIndex].potNewValue(pot);
                actions[handIndex].pStackNewValue(playerStack);
                actions[handIndex].cStackNewValue(computerStack);
                licit.Add(returnedAction);
                actions[handIndex].SetLicit(licit);
                handStatusListIndex++;


            }




        }

        // az osztó személyétől függően saját laopk kiosztása
        public void DealHoleCards()
        {
            if (next == 1)
            {
                player.HoleCards[0] = DealOneCard();
                computer.HoleCards[0] = DealOneCard();
                player.HoleCards[1] = DealOneCard();
                computer.HoleCards[1] = DealOneCard();
            }
            else
            {
                computer.HoleCards[0] = DealOneCard();
                player.HoleCards[0] = DealOneCard();
                computer.HoleCards[1] = DealOneCard();
                player.HoleCards[1] = DealOneCard();
                
            }
        }


        public void SwitchBlind()
        {
                string change;   
                change = bigBlind;
                bigBlind = smallBlind;
                smallBlind = change;
                next *= -1;

        }

        

        public void ConsoleWrite(string message)
        {
            if (MyPrintMethod == null)
                return;
  
            MyPrintMethod.Invoke(message );
        }


        public void ConsoleRead(int[] possible, int callValue, int stack, int bigBlindValue)
        {
            if (MyReadMethod == null)
                return;

            MyReadMethod.Invoke(  possible,  callValue,  stack,  bigBlindValue);
        }



        public int DealOneCard()
        {
            deckIndex++;
            ConsoleWrite("asd" + deckIndex);
            return Deck[deckIndex-1];
        }

        public string intToName(int value)
        {
            if (value == 1) return "Player";
            else            return "Computer";
        }


        public string ValueToRank (int value)
        {
            int iRank = value%13;
            string rank;
            
            switch (iRank)
            {
                case 9: rank = "J";
                    break;
                case 10: rank = "Q";
                    break;
                case 11: rank = "K";
                    break;
                case 12: rank = "A";
                    break;
                default: rank = (iRank +2).ToString();
                    break;
            }

            return rank;
        }

         public string ValueToSuit (int value)
        {
            int iSuit = value/13;
            string suit = null;
            
            switch (iSuit)
            {
                case 0: suit = "Clubs";
                    break;
                case 1: suit = "Diamonds";
                    break;
                case 2: suit = "Hearts";
                    break;
                case 3: suit = "Spades";
                    break;
            }

            return suit;
        }



    }

        


       
    


}
    
