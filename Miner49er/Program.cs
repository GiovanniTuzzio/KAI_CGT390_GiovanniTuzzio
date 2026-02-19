using System;

namespace Miner49er
{
    class MainClass
    {
        public static void Main(string[] args) {
            //Set up the variables 
            Miner miner = new SimpleMiner();
            int secsPerTick = 1;
            Random myRandom = new Random(Environment.TickCount);
            int gameLengthInTicks = 61;
            //game lasts for 60 ticks. I did 61 because tick #0 just shows the starting values
            for (int tick = 0; tick < gameLengthInTicks; tick++)
            {
                Console.WriteLine("Tick # " + tick);
                miner.DoEvent("tick");
                miner.printStatus();
                Console.WriteLine();
                Console.WriteLine("");
                System.Threading.Thread.Sleep(secsPerTick * 1000);
            }

            Console.WriteLine("Ending Wealth: " + miner.getCurrentWealth());
        }
    }
}