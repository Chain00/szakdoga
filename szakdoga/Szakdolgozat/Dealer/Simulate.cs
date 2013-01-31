using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dealer
{
    public class Simulate
    {
        
        private const int startingStack = 2000;
        private const int bigBlindValue = 20;
        private const int smallBlindValue = 10;
         

        public int GameMode { get; set; }
        public int[] Deck { get; set; }
        public int Pot { get; set; }
        public printMethod MyPrintMethod { get; set; }
       


        private String bigBlind;
        private String smallBlind;
        private int currentPlayer;
        private int deckIndex;
        private int handIndex;
        private int gameWinner;
        private int handWinner;
        private bool allIn;
       
       
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

      
        Structures.IComputer computer1;
        Structures.IComputer computer2;


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
        public Simulate(int computer1, int computer2)
        {

            int[] deck = new int[52];

            Structures.IComputer comp1 = null;
            Structures.IComputer comp2 = null;


            switch (computer1)
            {
                case 1: comp1 = new Computer.ComputerRandom(); 
                    break;
                case 2: comp1 = new Computer.ComputerCallingStation();
                    break;
            }


            switch (computer2)
            {
                case 1: comp2 = new Computer.ComputerRandom();
                    break;
                case 2: comp2 = new Computer.ComputerCallingStation();
                    break;
            }

         

            for (int i = 0; i < deck.Length; i++)
            {
                deck[i] = i+1;
            }
                
            this.Deck = deck;
            this.computer1 = comp1;
            this.computer2 = comp2;
            this.computer1.CreateComputer(startingStack, bigBlindValue);
            this.computer2.CreateComputer(startingStack, bigBlindValue);
            
          
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
            int c1SCard, c2SCard;
           
            Shuffle(Deck);
            deckIndex = 0;
            gameWinner = 0;
            Pot = 0;
            c1SCard = DealOneCard()%13;
            c2SCard = DealOneCard()%13;
            

            currentPlayer = c1SCard > c2SCard ? 1 : -1;
            smallBlind = intToName(currentPlayer);
            bigBlind = intToName(currentPlayer * -1);
     

            ConsoleWrite(String.Format("\n\nChoosing dealer....\nPlayer : {0} of {1}\nComputer : {2} of {3}", ValueToRank(c1SCard), ValueToSuit(c1SCard), ValueToRank(c2SCard), ValueToSuit(c2SCard) ));
            //ez azért kell mert az osztó eldöntése után a vakok jól vannak kiosztva, de a SartHand() első hívása a vakcsere, így helyre kerülnek.
            SwitchBlind();


            while (gameWinner == 0)
            {
                StartHand();

            }

            ConsoleWrite(String.Format("Game Over!\n{0} Wins the Game!", intToName(gameWinner)));
         
        }

        //leosztás indítása
        public void StartHand()
        {

        
            
            SwitchBlind();
            nextStreet = false;
            bool first = true;
            handWinner = 0;
            allIn = false;
            Shuffle(Deck);
            deckIndex = 0;
            //preflop 1.action
            handIndex = -10;
            
           
            ConsoleWrite("------------------New Hand--------------");
            ConsoleWrite(String.Format("computer1 Stack: {0} \nComputer Stack: {1}\n", computer1.Stack,computer2.Stack));
            ConsoleWrite(String.Format("\nSmall blind: {0} \nBigBlind: {1}", smallBlind, bigBlind));
            ConsoleWrite(String.Format("Posting blinds."));
            ConsoleWrite(String.Format("{0} posts {1}", intToName(currentPlayer *-1), bigBlindValue));
            ConsoleWrite(String.Format("{0} posts {1}", intToName(currentPlayer), smallBlindValue));

            if ( computer1.Stack <= bigBlindValue || computer2.Stack <= bigBlindValue)
            {
                allIn = true;

                if (computer1.Stack < computer2.Stack) ChipHandling(1, 0, computer1.Stack);
                else ChipHandling(-1, 0, computer2.Stack);

            }
            else
            {
                ChipHandling(currentPlayer, 0, smallBlindValue);
                ChipHandling(currentPlayer * -1, 0, bigBlindValue);
                ConsoleWrite(String.Format("\nPot: {0}\n", Pot));
            }



            ConsoleWrite(String.Format("Player Stack: {0} \nComputer Stack: {1}\n", computer1.Stack, computer2.Stack));

        
 
            DealHoleCards();
            ConsoleWrite(String.Format("\nPlayer : {0} of {1}  : {2} of {3}", ValueToRank(computer1.getHoleCards(0)), ValueToSuit(computer1.getHoleCards(0)), ValueToRank(computer1.getHoleCards(1)), ValueToSuit(computer1.getHoleCards(1))));
            ConsoleWrite(String.Format("Computer : {0} of {1}  : {2} of {3}", ValueToRank(computer2.getHoleCards(0)), ValueToSuit(computer2.getHoleCards(0)), ValueToRank(computer2.getHoleCards(1)), ValueToSuit(computer2.getHoleCards(1))));
            ConsoleWrite("\n\n");

            if (computer1.Stack == 0 || computer2.Stack == 0) allIn = true;

            //preflop
            while ((handWinner == 0) && (!nextStreet) && (!allIn))
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
            if (nextStreet || allIn)
            {
                DealFlop();
                first = true;
                currentPlayer *= -1;
            }
            while ((handWinner == 0) && (!nextStreet) && (!allIn))
            {
                ConsoleWrite(String.Format("\nPot: {0}\n", Pot));
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
            if (nextStreet || allIn)
            {
                DealTurn();
                first = true;
            }


            while ((handWinner == 0) && (!nextStreet) && (!allIn))
            {
                ConsoleWrite(String.Format("\nPot: {0}\n", Pot));
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
            if (nextStreet || allIn)
            {
                DealRiver();
                first = true;
            }


            while ((handWinner == 0) && (!nextStreet) && (!allIn))
            {
                ConsoleWrite(String.Format("\nPot: {0}\n", Pot));
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
            ConsoleWrite(String.Format("\nPot: {0}\n", Pot));
            if (handWinner == 0 || allIn) Showdown(actions.Count);
            DisplayWinner();

            ChipHandling(0, handWinner, Pot);

            if (computer1.Stack == 0 || computer2.Stack == 0)
            {
                gameWinner = computer1.Stack > computer2.Stack ? 1 : -1;
            }
  
            
        }


        public void ChipHandling(int from, int to, int value)
        {
            if (to == 0)
            {
                Pot += value;
                if (from == 1) computer1.Stack -= value;
                else computer2.Stack -= value;
            }
            else if (to == 1)
            {
                Pot -= value;
                computer1.Stack += value;
            }
            else
            {
                Pot -= value;
                computer2.Stack += value;
            }

        }



        public void DisplayWinner()
        {
            ConsoleWrite(String.Format("\n{0} wins the pot {1}", intToName( handWinner), Pot));

            
        }





        public void CheckHandStatus(bool display)
        {
            Round actual = actions.Last<Round>();

            if ((computer1.Stack == 0) || (computer2.Stack == 0))
            {
                allIn = true;
                return;
            }

            if (handStatusListIndex >= 0)
            {
                if (actual.S_licit[handStatusListIndex] == 0)
                {

                    DisplayStatus();
                    currentPlayer *= -1;
                    handWinner = currentPlayer;
                    return;

                }
            }

            if (handStatusListIndex >= 1)
            {

                if (actual.S_licit[handStatusListIndex] == actual.S_licit[handStatusListIndex - 1])
                {
                    SetHandIndex();
                    nextStreet = true;
                    //if (actual.S_licit[handStatusListIndex] > 1)
                    //{
                    //    ChipHandling(currentPlayer, 0, actual.S_licit[handStatusListIndex]);
                    //}

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

            if (currentPlayer == -1)
            {
                string move;


                if (actual.S_licit[handStatusListIndex] == 0) move = "fold";
                else if (actual.S_licit[handStatusListIndex] == 1) move = "check";
                else move = "bet " + actual.S_licit[handStatusListIndex];
                if (handStatusListIndex > 0)
                {
                    if (((actual.S_licit[handStatusListIndex] >= 20) && ((actual.S_licit[handStatusListIndex]) == (actual.S_licit[handStatusListIndex - 1]) || ((actual.S_licit[handStatusListIndex]) < (actual.S_licit[handStatusListIndex - 1]))))) move = "call";
                }
                //preflop 1. akció esetén
                if ((handStatusListIndex == 0) && (actions[0] == actual) && (actual.S_licit[handStatusListIndex] == 1)) move = "complete";

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
                //preflop 1. akció esetén
                if ((handStatusListIndex == 0) && (actions[0] == actual) && (actual.S_licit[handStatusListIndex] == 1)) move = "complete";


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
            nextStreet = false;

            ConsoleWrite(String.Format("The Flop: {0} of {1}, {2} of {3}, {4} of {5}", ValueToRank(theFlop[0]), ValueToSuit(theFlop[0]), ValueToRank(theFlop[1]), ValueToSuit(theFlop[1]), ValueToRank(theFlop[2]), ValueToSuit(theFlop[2])));
        }

        public void DealTurn()
        {

            theTurn = DealOneCard();
            ConsoleWrite(String.Format("\nThe Turn: {0} of {1}", ValueToRank(theTurn), ValueToSuit(theTurn)));
            nextStreet = false;
            
        }

        public void DealRiver()
        {
            theRiver = DealOneCard();
            ConsoleWrite(String.Format("\nThe River: {0} of {1}", ValueToRank(theRiver), ValueToSuit(theRiver)));
            nextStreet = false;
        }

        // aktuális állapot kiküldése a soron következő játékosnak
        public void SendHandStatus()
        {
            int[] returnedActionArray = new int[5];

            if (currentPlayer == 1)
            {
                if (actions[handIndex].S_licit == null)
                {
                    possibleActions = CalculatePossibleActions(true);
                }
                else possibleActions = CalculatePossibleActions(false);

                //lehetőségek átadása, válasz megkapása
                while (true)
                {
                    returnedActionArray = computer2.ReturnAction(possibleActions, CalculateCallValue());
                    //híváshiba lehet
                    //akció kiválasztása és letárolása
                    for (int i = 0; i < returnedActionArray.Length; i++)
                    {
                        if ((returnedActionArray[i] == 1) && (i == 0)) returnedAction = 0;
                        if ((returnedActionArray[i] == 1) && (i == 1)) returnedAction = 1;
                        if ((returnedActionArray[i] > 1) && (i == 2)) returnedAction = returnedActionArray[i];
                        if ((returnedActionArray[i] > 1) && (i == 3)) returnedAction = returnedActionArray[i];
                        if ((returnedActionArray[i] > 1) && (i == 4)) returnedAction = returnedActionArray[i];
                    }
                    if (CheckReturned(returnedActionArray)) break;
                }

                if (returnedAction > 1) ChipHandling(currentPlayer, 0, returnedAction);
                
               
               
            }
            else
            {
                if (actions[handIndex].S_licit == null)
                {
                    possibleActions = CalculatePossibleActions(true);
                }
                else possibleActions = CalculatePossibleActions(false);

                //lehetőségek átadása, válasz megkapása
                while (true)
                {
                    returnedActionArray = computer2.ReturnAction(possibleActions, CalculateCallValue());
                    //híváshiba lehet
                    //akció kiválasztása és letárolása
                    for (int i = 0; i < returnedActionArray.Length; i++)
                    {
                        if ((returnedActionArray[i] == 1) && (i == 0)) returnedAction = 0;
                        if ((returnedActionArray[i] == 1) && (i == 1)) returnedAction = 1;
                        if ((returnedActionArray[i] > 1) && (i == 2)) returnedAction = returnedActionArray[i];
                        if ((returnedActionArray[i] > 1) && (i == 3)) returnedAction = returnedActionArray[i];
                        if ((returnedActionArray[i] > 1) && (i == 4)) returnedAction = returnedActionArray[i];
                    }
                    if (CheckReturned(returnedActionArray)) break;
                }
                
                if (returnedAction > 1) ChipHandling(currentPlayer, 0, returnedAction);
                
            }

        }


        public bool CheckReturned(int[] returnedArray)
        {
            Round actual = new Round();
            bool b = false; 

            for (int i = 0; i < 5; i++)
            {
                if ((possibleActions[i] == 1) && (returnedArray[i] >= 1))
                {
                    b =  true;
                }
            }
            if (b == false) return false;

            if ((returnedAction == 10) && (actions[0].S_licit != null)) return true;
            

            if (currentPlayer == -1)
            {
                if (returnedAction >= computer2.Stack)
                {
                    ConsoleWrite("You don't have enough money! CHECK5");
                    return false;
                }
                if ((handStatusListIndex > 0) && ((returnedAction == 2) || (returnedAction == 4)))
                {
                    if (actual.S_licit != null)
                    {
                        //előző check és hibás hívás( < 20)
                        if (((actual.S_licit[handStatusListIndex - 1] == 1) && (returnedAction < bigBlindValue )))
                        {
                            ConsoleWrite("Wrong bet! CHECK6");
                            return false;
                        }
                        //előző hívás és hibás hívás( < x*2)
                        if (((actual.S_licit[handStatusListIndex - 1] > 1) && (returnedAction < actual.S_licit[handStatusListIndex - 1] * 2)))
                        {
                            ConsoleWrite("Wrong bet! CHECK7");
                            return false;
                        }
                    }
                }
                // 1.akció esetén ha a bet kisebb mint BB
                if (((actual.S_licit == null) && (returnedAction < bigBlindValue ) && (returnedAction == 3)))
                {
                    ConsoleWrite("Wrong bet! CHECK8");
                    return false;
                }


               
            }
               
            return true;
        }


       



        public void Showdown(int street)
        {
            int[] c1Hand = new int[5]; 
            int[] c2Hand = new int[5];
            int index = 0;
            int c1Summ =0, c2Summ = 0;

            //if (street == 2)
            //{
            //    index = 3;
            //    pHand = theFlop;
            //}
            //if (street == 3)
            //{
            //    index = 4;
            //    pHand[0] = theFlop[0];
            //    pHand[1] = theFlop[1];
            //    pHand[2] = theFlop[2];
            //    pHand[3] = theTurn;
            //}
            //if (street == 4)
            //{
            //    index = 5;
            
            //    pHand[0] = theFlop[0];
            //    pHand[1] = theFlop[1];
            //    pHand[2] = theFlop[2];
            //    pHand[3] = theTurn;
            //    pHand[4] = theRiver;
            //}


            //if (street == 2)
            //{
            //    index = 3;
            //    cHand = theFlop;
            //}
            //if (street == 3)
            //{
            //    index = 4;
            //    cHand[0] = theFlop[0];
            //    cHand[1] = theFlop[1];
            //    cHand[2] = theFlop[2];
            //    cHand[3] = theTurn;
            //}
            //if (street == 4)
            //{
            //    index = 5;

            //    cHand[0] = theFlop[0];
            //    cHand[1] = theFlop[1];
            //    cHand[2] = theFlop[2];
            //    cHand[3] = theTurn;
            //    cHand[4] = theRiver;
            //}
           
            
            //Array.Sort(pHand);
            //Array.Sort(cHand);
            //Array.Sort(player.HoleCards);
            //Array.Sort(computer2.HoleCards);


           



            //for (int i = 0; i < pHand.Length; i++)
            //{
               
            //    pSumm += pHand[i];
            //    cSumm += cHand[i];
            //}

            c1Summ = computer2.getHoleCards(0) + computer2.getHoleCards(1);
            c2Summ = computer1.getHoleCards(0) + computer1.getHoleCards(1);

        
        handWinner =  c1Summ > c2Summ ? 1 : -1;
        ConsoleWrite(String.Format("Player: {0}\nComputer: {1}\nhandwinner: {2}", c1Summ, c2Summ, handWinner));

        }

        public int[] CalculatePossibleActions(bool isNull)
        {

            Round actual = actions.Last<Round>();
            int[] possible = new int[5];
            
            //az actions lista utolsó elemének kiválasztása után, a licitkör vizsgálata, és ez alapján a lehetséges akciókat tároló tömb feltöltése

            //preflop 1. akció
            if ((isNull) && (actions.Count == 1))
            {
                possible[0] = 1;
                possible[1] = 0;
                possible[2] = 0;
                possible[3] = 1;
                possible[4] = 1;

                return possible;
            }
            else if (isNull)
	        {
                possible[0] = 1;
                possible[1] = 1;
                possible[2] = 1;
                possible[3] = 0;
                possible[4] = 0;

                return possible;
	        }
  
          
            else if (actual.S_licit[handStatusListIndex] == 1)           
            {
                possible[0] = 1;
                possible[1] = 1;
                possible[2] = 1;
                possible[3] = 0;
                possible[4] = 0;
             
            }

            else if (actual.S_licit[handStatusListIndex] >= 20)
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
            //egészítése esetén
            else if ((actions.Count == 1) && (actual.S_licit == null)) return smallBlindValue;
            // elelnkező esetben a hívás értéke az aktuális licit értéke
            else if (handStatusListIndex > 0)
            {
                if ((actual.S_licit[handStatusListIndex - 1] > 1) && (actual.S_licit[handStatusListIndex - 1] < actual.S_licit[handStatusListIndex])) return actual.S_licit[handStatusListIndex] - actual.S_licit[handStatusListIndex-1];
            }
            return actual.S_licit[handStatusListIndex];
           
        
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
                preflop.S_cStack = computer2.Stack;
                preflop.S_pStack = computer1.Stack;
                preflop.S_pot = Pot;
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

               

                actions[handIndex].S_pot= Pot;
                actions[handIndex].S_pStack = computer1.Stack;
                actions[handIndex].S_cStack = computer2.Stack;

                if (actions[handIndex].S_licit == null)
                {
                    List<int> licit = new List<int>();
                    //nem kell?
                    if (returnedAction == 10)
                    {
                        returnedAction = 1;
                       // ChipHandling(currentPlayer, 0, 10);
                    }
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

    
                flop.S_cStack = computer2.Stack;
                flop.S_pStack = computer1.Stack;
                flop.S_flop = theFlop;
                flop.S_pot = Pot;
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
               
                actions[handIndex].S_pot = Pot;
                actions[handIndex].S_pStack = computer1.Stack;
                actions[handIndex].S_cStack = computer2.Stack;
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

                turn.S_cStack = computer2.Stack;
                turn.S_pStack = computer1.Stack;
                turn.S_pot = Pot;
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


                actions[handIndex].S_pot = Pot;
                actions[handIndex].S_pStack = computer1.Stack;
                actions[handIndex].S_cStack = computer2.Stack;
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


                river.S_cStack = computer2.Stack;
                river.S_pStack = computer1.Stack;
                river.S_pot = Pot;
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
                actions[handIndex].S_pot = Pot;
                actions[handIndex].S_pStack = computer1.Stack;
                actions[handIndex].S_cStack = computer2.Stack;
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

            int[] c1Cards = new int[2];
            int[] c2Cards = new int[2];




            if (currentPlayer == 1)
            {
                c1Cards[0] = DealOneCard();
                c2Cards[0] = DealOneCard();
                c1Cards[1] = DealOneCard();
                c2Cards[1] = DealOneCard();
            }
            else
            {
                c2Cards[0] = DealOneCard();
                c1Cards[0] = DealOneCard();
                c2Cards[1] = DealOneCard();
                c1Cards[1] = DealOneCard();   
            }

            computer1.setHoleCards(c1Cards);
            computer2.setHoleCards(c2Cards);
        }


        public void SwitchBlind()
        {
                string change;   
                change = bigBlind;
                bigBlind = smallBlind;
                smallBlind = change;
            
              currentPlayer = NameToInt(smallBlind);

        }

        

        public void ConsoleWrite(string message)
        {
            if (MyPrintMethod == null)
                return;
  
            MyPrintMethod.Invoke(message );
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

        public int NameToInt(string value)
        {
            if (value == "Player") return 1;
            else return -1;
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
    
