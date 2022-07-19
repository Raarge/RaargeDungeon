using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaargeDungeon.Creatures;
using RaargeDungeon.Combat;

namespace RaargeDungeon.Tests
{
    public class CombatTests
    {
        public static void DisplayMstrHitTry(Player p, Monster m, HitChecks mstrHit, string MstrAtkType)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("********************************************************************");
            Console.WriteLine("                    Display Monster Hit Try Test On                 ");
            Console.WriteLine("     To Disable Comment out Call in MartialCombat.MstrHitTry        ");
            Console.WriteLine($"  Type of Attack: {MstrAtkType}       Attack Roll: {mstrHit.ToHitRoll}");
            Console.WriteLine($"  Player AC Unmodified {p.armorclass} Learned SKills Modifier: {(int)(p.evasion + p.armorMastery) / 4m}");
            Console.WriteLine($"  Player AC Modified: {p.armorclass + (int)(p.evasion + p.armorMastery) / 4m}  Is A Hit: {mstrHit.AttackHits}");
            Console.WriteLine("*********************************************************************");
            Console.ResetColor();
            Console.ReadKey();

        }
    }
}
