using System;
using KAI.FSA; // Use the namespace from your FSAImpl definition

namespace Miner49er
{
    /// <summary>
    /// This class implements the Miner as described by the first state transition table
    /// </summary>
    public class SimpleMiner : FSAImpl, Miner
    {
        /// Amount of gold nuggest in the miner's pockets ...
        public int gold = 0;
        /// How thirsty the miner is ...
        public int thirst = 0;
        /// How many gold nuggets the miner has in the bank ...
        public int bank = 0;

        Random myRandom = new Random(); //random number generator

        // The following variables are each oen of the defiend states the miner cna be in.
        State miningState;
        State drinkingState;
        State bankingState;
        State focusedMiningState; //New state i added that allows miner to enter a "focused" state


        // FIXED: Added : base("SimpleMiner") to resolve the FSAImpl constructor error
        public SimpleMiner() : base("SimpleMiner")
        {
            // FIXED: Using PascalCase to match your FSAImpl.MakeNewState method
            miningState = MakeNewState("Mining");
            drinkingState = MakeNewState("Drinking");
            bankingState = MakeNewState("Banking");
            focusedMiningState = MakeNewState("FocusedMining"); //Creates the new "Focused Mining" state

            // set mining transitions
            miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.parched) },
                new ActionDelegate[] { new ActionDelegate(this.incrementThirst) }, drinkingState);
            
            miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.pocketsFull) },
                new ActionDelegate[] { new ActionDelegate(this.incrementThirst) }, bankingState);

            // if thirst is <=5, set to "Focused mining"
            miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.lowThirst) },
                new ActionDelegate[] { }, focusedMiningState);


            miningState.addTransition("tick",
                new ConditionDelegate[] { }, 
                new ActionDelegate[] { new ActionDelegate(this.dig) }, miningState);


            // set focused mining transitions
            focusedMiningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.parched) },
                new ActionDelegate[] { },
                drinkingState);

            focusedMiningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.pocketsFull) },
                new ActionDelegate[] { },
                bankingState);

            // If thirst rises above 5 → return to normal mining
            focusedMiningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.notLowThirst) },
                new ActionDelegate[] { },
                miningState);

            focusedMiningState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.focusedDig) },
                focusedMiningState);
            // set drinking transitions
            drinkingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.thirsty) },
                new ActionDelegate[] { new ActionDelegate(this.takeDrink) }, drinkingState);

            drinkingState.addTransition("tick",
                 new ConditionDelegate[] { new ConditionDelegate(this.notThirsty) }, // new transition to allow debug to continue while drinking
                 new ActionDelegate[] { }, miningState);

            drinkingState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.incrementThirst) }, miningState);

            // set banking transitions
            bankingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.pocketsNotEmpty) },
                new ActionDelegate[] { new ActionDelegate(this.depositGold) }, bankingState);

            bankingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.parched) },
                new ActionDelegate[] { }, drinkingState);
            
            bankingState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { }, miningState);

            // FIXED: Using PascalCase to match your FSAImpl.SetCurrentState method
            SetCurrentState(miningState);
        }

        /// <summary>
        /// This is a condition that tests to see if the miner is so thirsty that he cannot dig
        /// </summary>
        /// 

        private Boolean parched(FSA fsa)
        {
            if (thirst >= 15)
            {
                Console.WriteLine("Too thirsty too work.");
            }
            return thirst >= 15;
        }

        /// <summary>
        /// An action that decrements the miner's thirst ...
        /// </summary>
        private void takeDrink(FSA fsa)
        {
            thirst -= 1;
            Console.WriteLine("Glug glug glug");
        }

        /// <summary>
        /// An action that decrements the gold in the miner's pockets and increments the gold in the bank ...
        /// </summary>
        private void depositGold(FSA fsa)
        {
            gold -= 1;
            bank += 1;
            Console.WriteLine("deposit a gold nugget");
        }

        /// <summary>
        /// This implements the Miner.getCurrentWealth() call ...
        /// </summary>
        public int getCurrentWealth()
        {
            return bank + gold;
        }

        // --- Previously extracted methods ---

        private void dig(FSA fsa)
        {
            gold++;
            thirst++;
            Console.WriteLine("Miner is digging.");
        }

        //Focused digging
        private void focusedDig(FSA fsa)
        {
            //normal mining
            gold++;
            thirst++;

            Console.WriteLine("Miner is mining gold while focused.");

            // 20% critical hit bonus for +3 gold
            if (thirst <8 && myRandom.Next(0,100) < 25)
            {
                gold += 2;
                Console.WriteLine("Critical hit!");
            }
        }
        private void incrementThirst(FSA fsa)
        {
            thirst++;
        }

        private Boolean pocketsFull(FSA fsa) => gold >= 5;

        private Boolean pocketsNotEmpty(FSA fsa) => gold > 0;

        private Boolean thirsty(FSA fsa) => thirst > 0;

        private Boolean notThirsty(FSA fsa) => thirst <= 0;

        private Boolean lowThirst(FSA fsa) => thirst <= 8;

        private Boolean notLowThirst(FSA fsa) => thirst > 8;
        public void printStatus()
        {
            Console.WriteLine("Thirst: "+thirst+" Gold: "+gold+" Bank: "+bank);
        }
    }
}