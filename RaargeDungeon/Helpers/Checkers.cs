using System;
using RaargeDungeon.Items;
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
    }
}
