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
        private int currentPlayer;
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
        private bool nextStreet;
        public delegate void Del(string message);
        Random rnd = new Random();

        Player.Player player;
        ComputerRandom.ComputerRandom computer;


        public delegate void printMethod(string s);
        public delegate int readMethod(int[] possible, int callValue, int stack, int bigBlindValue);




        //TODO struct classra cserélése
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

            currentPlayer = pSCard > cSCard ? 1 : -1;
            smallBlind = intToName(currentPlayer);
            bigBlind = intToName(currentPlayer * -1);
     

            ConsoleWrite(String.Format("\n\nChoosing dealer....\nPlayer : {0} of {1}\nComputer : {2} of {3}", ValueToRank(pSCard), ValueToSuit(pSCard), ValueToRank(cSCard), ValueToSuit(cSCard) ));
            



            for (int i = 0; i < 10; i++)
            {


                StartHand(false);
                SwitchBlind();
            }
         
        }

        //leosztás indítása
        public void StartHand(bool switchButtons)
        {

            if (switchButtons)
            {
                SwitchBlind();
            }

            ConsoleWrite("------------------New Hand--------------");

            ConsoleWrite(String.Format("Small blind: {0} \nBigBlind: {1}", smallBlind, bigBlind));
            ConsoleWrite(String.Format("Posting blinds.\n"));
            ConsoleWrite(String.Format("{0} posts {1}", intToName(currentPlayer * -1), bigBlindValue));
            ConsoleWrite(String.Format("{0} posts {1}", intToName(currentPlayer), smallBlindValue));


            nextStreet = false;
            bool first = true;
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
                if (first)
                {
                    HandStatus();
                 //   CheckHandStatus(false);
                    first = false;
                }


                //adatok átadása a játékosoknak, return reakció 
                SendHandStatus();
                HandStatus();
            

  
                CheckHandStatus(true);
         

            }

            //flop
            DealFlop();
            nextStreet = false;
            first = true;

            while ((handWinner == 0) && (!nextStreet))
            {
                if (first)
                {
                    HandStatus();
                 //   CheckHandStatus(false);
                    first = false;
                }
                SendHandStatus();
                HandStatus();
                CheckHandStatus(true);
            }

            //turn
            DealOneCard();
            ConsoleWrite(String.Format("\nThe Turn: {0} of {1}", ValueToRank(theTurn), ValueToSuit(theTurn)));
            nextStreet = false;
            first = true;



            while ((handWinner == 0) && (!nextStreet))
            {
                if (first)
                {
                    HandStatus();
          //          CheckHandStatus(false);
                    first = false;
                }
                SendHandStatus();
                HandStatus();
                CheckHandStatus(true);
            }

            //river
            DealOneCard();
            ConsoleWrite(String.Format("\nThe River: {0} of {1}", ValueToRank(theRiver), ValueToSuit(theRiver)));
            nextStreet = false;
            first = true;



            while ((handWinner == 0) && (!nextStreet))
            {
                if (first)
                {
                    HandStatus();
            //        CheckHandStatus(false);
                    first = false;
                }
                SendHandStatus();
                HandStatus();
                CheckHandStatus(true);
            }
            
            
            
            

           /* 
            foreach(var item in actions)
            {
                  ConsoleWrite(String.Format("preflop"));
                  ConsoleWrite(String.Format("BB {0}", item.S_bigBlind));
                  ConsoleWrite(String.Format("SB {0}", item.S_smallBlind));
                  ConsoleWrite(String.Format("pot {0}", item.S_pot));
                  ConsoleWrite(String.Format("cstack {0}", item.S_cStack));
                  ConsoleWrite(String.Format("pstack {0}", item.S_pStack));

                  ConsoleWrite("\n\n");
                  ConsoleWrite(String.Format("flop 1 {0}", theFlop[0]));
                  ConsoleWrite(String.Format("flop 2 {0}", theFlop[1]));
                  ConsoleWrite(String.Format("flop 3 {0}", theFlop[2]));
                  ConsoleWrite(String.Format("turn {0}", theTurn));
                  ConsoleWrite(String.Format("river {0}", theRiver));
                  ConsoleWrite(String.Format("index {0}", deckIndex));
                  ConsoleWrite(String.Format("deck {0}", Deck[deckIndex]));
            }

         */

       
            

            
        }


        public void CheckHandStatus(bool display)
            {
            Round actual = actions.Last<Round>();

            if (handStatusListIndex >= 0)
            {
                if (actual.S_licit[handStatusListIndex] == 0)
                {
                    //ha rossz anyertes ezt csere
                    handWinner = currentPlayer;
                    //currentPlayer *= -1;
                    DisplayStatus();
                    StartHand(true);
                }
            }

            if (handStatusListIndex >= 1)
            {

                if (actual.S_licit[handStatusListIndex] == actual.S_licit[handStatusListIndex - 1])
                {
                    SetHandIndex();
                    nextStreet = true;
                }
                if (actual.S_licit[handStatusListIndex] == 1)
                {
                    SetHandIndex();
                }
            }


            if (display)
            {
                

                DisplayStatus();
            }

            currentPlayer *= -1;

        }

        public void DisplayStatus()
        {
            Round actual = actions.Last<Round>();
            
            if (currentPlayer == -1 ) 
            {  
                string move ;
             

                if (actual.S_licit[handStatusListIndex] == 0)   move = "fold";
                else if (actual.S_licit[handStatusListIndex] == 1)   move = "check";
                else   move = "bet " + actual.S_licit[handStatusListIndex];
                if (handStatusListIndex > 0)
                {
                    if ((actual.S_licit[handStatusListIndex] >= 20) && (actual.S_licit[handStatusListIndex]) == (actual.S_licit[handStatusListIndex - 1])) move = "call";
                }
                ConsoleWrite("Computer " + move);
            }

            if (currentPlayer == 1 )
            {
                string move;


                if (actual.S_licit[handStatusListIndex] == 0) move = "fold";
                else if (actual.S_licit[handStatusListIndex] == 1) move = "check";
                
                else move = "bet " + actual.S_licit[handStatusListIndex];
                if (handStatusListIndex > 0)
                {
                    if ((actual.S_licit[handStatusListIndex] >= 20) && (actual.S_licit[handStatusListIndex]) == (actual.S_licit[handStatusListIndex - 1])) move = "call";
                }
                ConsoleWrite("Player " + move);
            }


        }




        public void SetHandIndex()
        {
            switch (handIndex)
            {
                case 0: handIndex = -1;
                    break;
                case 1: handIndex = -2;
                    break;
                case 2: handIndex = -3;
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

            ConsoleWrite(String.Format("The Flop: {0} of {1}, {2} of {3}, {4} of {5}", ValueToRank(theFlop[0]), ValueToSuit(theFlop[0]), ValueToRank(theFlop[1]), ValueToSuit(theFlop[1]), ValueToRank(theFlop[2]), ValueToSuit(theFlop[2])));
        }

        // aktuális állapot kiküldése a soron következő játékosnak
        public void SendHandStatus()
        {

            if (currentPlayer == 1)
            {
                if (actions[handIndex].S_licit == null )
                {
                    possibleActions = CalculatePossibleActions(true);
                }
                else  possibleActions = CalculatePossibleActions(false);

                ConsoleRead(possibleActions, CalculateCallValue(), playerStack, bigBlindValue);
               
            }
            else
            {
                if (actions[handIndex].S_licit == null)
                {
                    possibleActions = CalculatePossibleActions(true);
                }
                else possibleActions = CalculatePossibleActions(false);

                //lehetőségek átadása, válasz megkapása
                int[] returnedActionArray = computer.ReturnAction(possibleActions, CalculateCallValue());
                //híváshiba lehet
                //akció kiválasztása és letárolása
                for (int i = 0; i < returnedActionArray.Length; i++)
                {
                    if ((returnedActionArray[i] == 1) && (i == 0))  returnedAction = 0;
                    if ((returnedActionArray[i] == 1) && (i == 1))  returnedAction = 1;
                    if ((returnedActionArray[i]  > 1) && (i == 2))  returnedAction = returnedActionArray[i];
                    if ((returnedActionArray[i]  > 1) && (i == 3)) returnedAction = returnedActionArray[i];
                    if ((returnedActionArray[i]  > 1) && (i == 4)) returnedAction = returnedActionArray[i];

                   
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
  
          
            if (actual.S_licit[handStatusListIndex] == 1)           
            {
                possible[0] = 1;
                possible[1] = 1;
                possible[2] = 1;
                possible[3] = 0;
                possible[4] = 0;
             
            }

            if (actual.S_licit[handStatusListIndex] >= 20)
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
            //ha lehet checkelni akkor nemkell számolni semmit
            if (possibleActions[1] == 1) return 0;
            // elelnkező esetben a hívás értéke az aktuális licit értéke
            else return actual.S_licit[handStatusListIndex];
        
        }

        public void HandStatus()
        {
            //preflop 1. akció
            if (handIndex == -10)
            {
                
                List<Round> _actions = new List<Round>();
             //   List<int> licit = new List<int>();
                Round preflop = new Round();

                preflop.S_bigBlind = currentPlayer*-1;
                preflop.S_smallBlind = currentPlayer;
                preflop.S_cStack = computerStack;
                preflop.S_pStack = playerStack;
                preflop.S_pot = pot;
                handStatusListIndex = -1;   //0ról -1re
            //    preflop.SetLicit(licit);
      
                
                _actions.Add(preflop);
                handIndex = 0;
                actions = _actions;
     
                return;
            }

            // preflop akció 
            else if (handIndex == 0)
            {

               

                actions[handIndex].S_pot=pot;
                actions[handIndex].S_pStack = playerStack;
                actions[handIndex].S_cStack =computerStack;
                if (actions[handIndex].S_licit == null)
                {
                    List<int> licit = new List<int>();

                    licit.Add(returnedAction);

                    // actions[handIndex].S_licit.Add(returnedAction);
                    actions[handIndex].S_licit = licit;//vmi cinkes
                }
                else actions[handIndex].S_licit.Add(returnedAction);
                handStatusListIndex++;
                return;

            }

            //flop 1.akció 
            else if (handIndex == -1)
            {
                
                Round flop = new Round();

    
                flop.S_cStack = computerStack;
                flop.S_pStack = playerStack;
                flop.S_flop = theFlop;
                flop.S_pot = pot;
                handStatusListIndex = -1;   //0ról -1re

          

                actions.Add(flop);
                handIndex = 1;

            }

            //flop akció
            else if (handIndex == 1)
            {
              /*  
               * copy paste előtt ezvolt
               * List<int> licit = actions[handIndex].S_licit;

                actions[handIndex].potNewValue(pot);
                actions[handIndex].pStackNewValue(playerStack);
                actions[handIndex].cStackNewValue(computerStack);
                licit.Add(returnedAction);
                actions[handIndex].SetLicit(licit);
                handStatusListIndex++;
                */
               
                actions[handIndex].S_pot = pot;
                actions[handIndex].S_pStack = playerStack;
                actions[handIndex].S_cStack = computerStack;
                if (actions[handIndex].S_licit == null)
                {
                    List<int> licit = new List<int>();

                    licit.Add(returnedAction);
                    actions[handIndex].S_licit = licit;
                }
                else actions[handIndex].S_licit.Add(returnedAction);
                handStatusListIndex++;
                return;
             
            }

            //turn 1. akció
            else if (handIndex == -2)
            {

                Round turn = new Round();

                turn.S_cStack = computerStack;
                turn.S_pStack = playerStack;
                turn.S_pot = pot;
                turn.S_turn = theTurn;
                handStatusListIndex = -1;   //0ról -1re



                actions.Add(turn);
                handIndex = 2;

            }

            //turn akció
            else if (handIndex == 2)
            {
                /*copy paste előttt
                List<int> licit = actions[handIndex].S_licit;

                actions[handIndex].potNewValue(pot);
                actions[handIndex].pStackNewValue(playerStack);
                actions[handIndex].cStackNewValue(computerStack);
                licit.Add(returnedAction);
                actions[handIndex].SetLicit(licit);
                handStatusListIndex++;
                */


                actions[handIndex].S_pot = pot;
                actions[handIndex].S_pStack = playerStack;
                actions[handIndex].S_cStack = computerStack;
                if (actions[handIndex].S_licit == null)
                {
                    List<int> licit = new List<int>();
                    licit.Add(returnedAction);

                    actions[handIndex].S_licit = licit;
                }
                else actions[handIndex].S_licit.Add(returnedAction);
                handStatusListIndex++;
                return;


            }


            //river 1. akció
            else if (handIndex == -3)
            {

                Round river = new Round();


                river.S_cStack = computerStack;
                river.S_pStack = playerStack;
                river.S_pot = pot;
                river.S_river = theRiver;
                handStatusListIndex = -1;   //0ról -1re



                actions.Add(river);
                handIndex = 3;

            }

            //river akció
            else if (handIndex == 3)
            {
                /*
                 * copy paste előttt
                List<int> licit = actions[handIndex].S_licit;

                actions[handIndex].potNewValue(pot);
                actions[handIndex].pStackNewValue(playerStack);
                actions[handIndex].cStackNewValue(computerStack);
                licit.Add(returnedAction);
                actions[handIndex].SetLicit(licit);
                handStatusListIndex++;

             */
                actions[handIndex].S_pot = pot;
                actions[handIndex].S_pStack = playerStack;
                actions[handIndex].S_cStack = computerStack;
                if (actions[handIndex].S_licit == null)
                {
                    List<int> licit = new List<int>();
                    licit.Add(returnedAction);

                    actions[handIndex].S_licit = licit;
                }
                else actions[handIndex].S_licit.Add(returnedAction);
                handStatusListIndex++;
                return;
            }




        }

        // az osztó személyétől függően saját laopk kiosztása
        public void DealHoleCards()
        {
            if (currentPlayer == 1)
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
            //kerdes kell e  
            //  currentPlayer *= -1;

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

            returnedAction = MyReadMethod.Invoke(  possible,  callValue,  stack,  bigBlindValue);
        }



        public int DealOneCard()
        {
            deckIndex++;
        
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
    
