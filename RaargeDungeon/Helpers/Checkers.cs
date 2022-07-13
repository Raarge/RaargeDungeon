using System;
using RaargeDungeon.Items;
using RaargeDungeon.Creatures;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaargeDungeon.Helpers
{
    public class Checkers
    {
        public static int GetSpellFailUpper()
        {
            int result = 0;

            return result = Program.currentPlayer.level + (int)(Math.Ceiling(((decimal)11 + Program.currentPlayer.spellChanneling)));
        }

        public static int GetSpellCost(SpellScroll spell)
        {
            int spellCost = (int)Math.Floor((decimal)spell.SpellCost - Program.currentPlayer.spellChanneling);
            if (spellCost < spell.minCost)
                spellCost = spell.minCost;
            return spellCost;
        }

        public static decimal GetSkillXPGain(int monsterPower, int spellCost, int reqLvl, string xpType)
        {
            decimal skillXPGain = 0;
            skillXPGain = (decimal) monsterPower / (20.0m + (decimal)monsterPower);

            if(xpType == "cast")
            {
                skillXPGain += skillXPGain + ((decimal)reqLvl / 24.0m);
            }
            else if(xpType == "channel")
            {
                skillXPGain += skillXPGain + ((decimal)spellCost / 120.0m);
            }
            else if(xpType == "magic")
            {
                skillXPGain += skillXPGain + (((Program.currentPlayer.spellCasting + Program.currentPlayer.spellChanneling) / 2) * .125m);
            }

            return skillXPGain;
        }

        public static Player GetStartingHealth(Player p)
        {
            // add a section to get con modifier to enhance hp

            if(p.currentClass == Player.PlayerClass.Mage)
            {
                p.baseHealth = Randomizer.GetRandomDieRoll(6);                
            }
            else if (p.currentClass == Player.PlayerClass.Ranger || p.currentClass == Player.PlayerClass.Warrior)
            {
                p.baseHealth = Randomizer.GetRandomDieRoll(10);

            }
            else if(p.currentClass == Player.PlayerClass.Rogue || p.currentClass == Player.PlayerClass.Cleric || p.currentClass == Player.PlayerClass.Monk)
            {
                p.baseHealth = Randomizer.GetRandomDieRoll(8);
            }


            p.health = p.baseHealth;

            return p;
        }
    }
}
