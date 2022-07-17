using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RaargeDungeon.Creatures;
using RaargeDungeon.Helpers;

namespace RaargeDungeon.Helpers
{
    public static class UIHelpers
    {
        #region ExperianceBar
        public static void ProgressBar(string fillerChar, string backgroundChar, decimal value, int size)
        {
            int diff = (int)(value * size);

            for (int i = 0; i < size; i++)
            {
                if (i < diff)
                    Console.Write(fillerChar);
                else
                    Console.Write(backgroundChar);
            }
        }
        #endregion
        public static void GenerateStatusBar(string barLabel, string statusMarker, string emptyMarker, ConsoleColor consoleColor, int stat, int baseStat)
        {
            Console.WriteLine($" {barLabel} Bar: ");
            Console.Write(" [");
            Console.ForegroundColor = consoleColor;
            ProgressBar($"{statusMarker}", $"{emptyMarker}", stat / (decimal)baseStat, 25);
            Console.ResetColor();
            Console.WriteLine("]");
        }

        #region SpecialPrint
        public static void Print(string text, int speed = 10)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(speed);
            }
            Console.WriteLine("");
        }
        #endregion

        #region Display Stats
        public static void DisplayStats(Player p)
        {
            Console.Clear();
            Console.WriteLine("==============================================");
            Console.WriteLine($" Player: {p.name}  Level: {p.level}");
            Console.WriteLine("==============================================");
            Console.WriteLine($" Strength:     {p.strength} Modifier: {Player.GetModifier(p.strength)}");
            Console.WriteLine($" Constitution: {p.constitution} Modifier: {Player.GetModifier(p.constitution)}");
            Console.WriteLine($" Dexterity:    {p.dexterity} Modifier: {Player.GetModifier(p.dexterity)}");
            Console.WriteLine($" Intelligence: {p.intelligence} Modifier: {Player.GetModifier(p.intelligence)}");
            Console.WriteLine($" Wisdom:       {p.wisdom} Modifier: {Player.GetModifier(p.wisdom)}");
            Console.WriteLine($" Charisma:     {p.charisma} Modifier: {Player.GetModifier(p.charisma)}");
            Console.WriteLine("==============================================");
            Console.WriteLine("        Armor and Weapons ");
            Console.WriteLine($"  Armor Class: {p.armorclass}");
            Console.WriteLine($"  Weapon Damage: {p.attackDie * p.numberAttackDie} ");
            Console.WriteLine("==============================================");
            Console.WriteLine("        Learned Skills                        ");
            Console.WriteLine($" {p.manaType} Casting:    {p.spellCasting:0.####}    ");
            Console.WriteLine($" {p.manaType} Mastery:    {p.magicMastery:0.####}    ");
            Console.WriteLine($" {p.manaType} Channeling: {p.spellChanneling:0.####}");
            Console.WriteLine($" Armor Mastery:  {p.armorMastery:0.####}");
            Console.WriteLine($" Weapon Mastery: {p.weaponMastery:0.####}");
            Console.WriteLine($" Evasion:       {p.evasion:0.####}");
            Console.WriteLine("==============================================");
            Console.ReadKey();

            Console.Clear();
            
        }
        #endregion
    }
}
