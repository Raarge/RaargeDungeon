using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaargeDungeon.Helpers
{
    public class Randomizer
    {
        public static int GetRandomDieRoll(int typeDice, int numberDice = 1, int modifier = 0)
        {
            Random random = new Random();
            int rollTotal = 0;

            for(int i = 1; i <= numberDice; i++)
            {
                rollTotal += random.Next(1, typeDice + 1);
            }

            return rollTotal + modifier;
        }

        public static int GetRandomNumber(int upper, int lower = 1)
        {
            Random random = new Random();

            return random.Next(lower, upper);
        }

        public static int GetPlayerStats()
        {
            int roll1 = GetRandomDieRoll(6);
            int roll2 = GetRandomDieRoll(6);
            int roll3 = GetRandomDieRoll(6);
            int roll4 = GetRandomDieRoll(6);
            int totalRoll = 0;
            int tempRoll = 0;
            int[] rolls = {roll1, roll2, roll3, roll4};

            for(int i = 0; i < rolls.Length - 1; i++)
            {
                for(int j = 0; j < rolls.Length - 1; j++)
                {
                    if (rolls[j] > rolls[j + 1])
                    {
                        tempRoll = rolls[j + 1];
                        rolls[j + 1] = rolls[j];
                        rolls[j] = tempRoll;
                    }
                }

                
            }
            totalRoll = rolls[1] + rolls[2] + rolls[3];

            return totalRoll;

        }

        public static int GetHealth(int hd, int numHd, int modifier)
        {
            int health = 0;

            health = GetRandomDieRoll(hd, numHd, modifier);

            return health;
        }

        public static int GetMonsterHealth(int hd, int numHd, int modifier)
        {
            int health = 0;

            health = GetRandomDieRoll(hd, numHd, 0) + modifier;

            return health;
        }
    }
}
