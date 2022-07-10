﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaargeDungeon.Helpers;


namespace RaargeDungeon
{
    public static class Skills
    {
        public static void CombatStealing(string nm)
        {
            // Combat Stealing
            if (Program.currentPlayer.race == Player.Race.Halfling && TextHelpers.rand.Next(1, 11) == 5)
            {
                int purseCoins = TextHelpers.rand.Next(1, 15) * ((Program.currentPlayer.mods * 1) + 10);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.Print($"You notice a small purse hanging from {nm}'s belt");
                Console.WriteLine($"You reach out and slit the purse swiftly catching {purseCoins} gold coins! ");
                Console.ResetColor();
                Program.currentPlayer.coins += purseCoins;
            }
            else if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && TextHelpers.rand.Next(1, 11) == 5)
            {
                int purseCoins = TextHelpers.rand.Next(1, 15) * ((Program.currentPlayer.mods * 1) + 10);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.Print($"You notice a small purse hanging from {nm}'s belt");
                Console.WriteLine($"You reach out and slit the purse swiftly catching {purseCoins} gold coins! ");
                Console.ResetColor();
                Program.currentPlayer.coins += purseCoins;
            }
        }

        public static void BackStab(string nm, ref int hlt, ref bool monsterAlive, int attack, ref string leader, ref string style)
        {
            if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && Program.currentPlayer.rand.Next(1, 6) == 5)
            {
                int backStabDamage = 0;

                backStabDamage = TextHelpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level;

                leader = TextHelpers.GetAttackStart(attack);
                style = TextHelpers.GetWeaponAttackStyle();

                if (Program.currentPlayer.rand.Next(1, 4) == 3)
                {                    
                    backStabDamage = backStabDamage * 3;

                    // attack
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("** Critical Backstab! **");
                    Console.ResetColor();
                    
                    hlt -= attack;
                    if (hlt <= 0)
                    {
                        
                        monsterAlive = false;
                    }
                }
                
                hlt -= attack;
                if (hlt <= 0)
                {

                    monsterAlive = false;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.Print($"Quickly stepping behind, {style} at {nm} dealing {backStabDamage} damage.");
                Console.ResetColor();
            }
        }

        public static void SpellBlast(string nm, ref int hlt, ref bool monsterAlive, int attack, ref string leader, ref string style, ref string spellType)
        {
            // mage spell blast 25% chance
            if (Program.currentPlayer.currentClass == Player.PlayerClass.Mage && TextHelpers.rand.Next(1, 5) == 3)
            {
                int spellDamage = 0;
                int regSpellCritTry = Program.currentPlayer.rand.Next(1, 6);
                int gnomeEruditeCritTry = Program.currentPlayer.rand.Next(1, 4);

                if (regSpellCritTry == 3 || ((Program.currentPlayer.race == Player.Race.Gnome || Program.currentPlayer.race == Player.Race.Erudite) &&
                    gnomeEruditeCritTry == 2))
                {
                    if (Program.currentPlayer.race == Player.Race.Gnome || Program.currentPlayer.race == Player.Race.Erudite)
                    {
                        //Console.WriteLine("Inside the gnome/erudie Spellblast Critical Loop");
                        //Console.ReadKey();

                        spellDamage = TextHelpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level + (Program.currentPlayer.level / 2);
                        spellDamage = spellDamage * 4;
                    }
                    else
                    {
                        spellDamage = TextHelpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level;
                        spellDamage = spellDamage * 4;
                    }

                    leader = TextHelpers.GetAttackStart(attack);
                    style = TextHelpers.GetWeaponAttackStyle();
                    spellType = TextHelpers.GetSpellType();

                    // attack
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"** Critical {spellType} spell attack! **");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Program.Print($"You gesture chanting loudly, a {spellType} materializes striking a {nm} dealing ");
                    Program.Print($"{spellDamage} damage.");
                    Console.ResetColor();
                    hlt -= attack;
                    if (hlt <= 0)
                    {
                        //Console.WriteLine($"{nm} was Slain!!");
                        monsterAlive = false;
                    }
                }
                else
                {
                    if (Program.currentPlayer.race == Player.Race.Gnome || Program.currentPlayer.race == Player.Race.Erudite)
                    {
                        spellDamage = TextHelpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level + (Program.currentPlayer.level / 2);
                    }
                    else
                    {
                        spellDamage = TextHelpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level;
                    }


                    leader = TextHelpers.GetAttackStart(attack);
                    style = TextHelpers.GetWeaponAttackStyle();
                    spellType = TextHelpers.GetSpellType();

                    // attack
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Program.Print($"You gesture chanting loudly, a {spellType} materializes striking a {nm} dealing ");
                    Program.Print($"{spellDamage} damage.");
                    Console.ResetColor();
                    hlt -= attack;
                    if (hlt <= 0)
                    {
                        //Console.WriteLine($"{nm} was Slain!!");
                        monsterAlive = false;
                    }
                }
            }
        }

        public static void AnimalCall(string nm, ref int hlt, ref bool monsterAlive, int attack, ref string leader, ref string style, ref string companion)
        {
            if (Program.currentPlayer.currentClass == Player.PlayerClass.Ranger && TextHelpers.rand.Next(1, 5) == 3)
            {
                int animalCallDamage = 0;
                if (Program.currentPlayer.rand.Next(1, 4) == 3)
                {
                    animalCallDamage = TextHelpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level;
                    animalCallDamage = animalCallDamage * 2;

                    leader = TextHelpers.GetAttackStart(attack);
                    style = TextHelpers.GetWeaponAttackStyle();
                    companion = TextHelpers.GetAnimalCompanion();

                    // attack
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"** Critical {companion} attack! **");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Program.Print($"You whistle loudly, a {companion} comes out of nowhere attacking {nm} dealing ");
                    Program.Print($"{animalCallDamage} damage.");
                    Console.ResetColor();
                    hlt -= attack;
                    if (hlt <= 0)
                    {
                        // Console.WriteLine($"{nm} was Slain!!");
                        monsterAlive = false;
                    }
                }
                else
                {
                    animalCallDamage = TextHelpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level;

                    leader = TextHelpers.GetAttackStart(attack);
                    style = TextHelpers.GetWeaponAttackStyle();
                    companion = TextHelpers.GetAnimalCompanion();

                    // attack
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Program.Print($"You whistle loudly, a {companion} comes out of nowhere attacking {nm} dealing ");
                    Program.Print($"{animalCallDamage} damage.");
                    Console.ResetColor();
                    hlt -= attack;
                    if (hlt <= 0)
                    {
                        //Console.WriteLine($"{nm} was Slain!!");
                        monsterAlive = false;
                    }
                }
            }
        }

        public static void ChiStrike(string nm, ref int hlt, ref bool monsterAlive, int attack, ref string leader, ref string style)
        {
            if (Program.currentPlayer.currentClass == Player.PlayerClass.Monk && Program.currentPlayer.rand.Next(1, 6) == 5)
            {
                int chiStrikeDamage = 0;

                chiStrikeDamage = TextHelpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level;

                leader = TextHelpers.GetAttackStart(attack);
                style = TextHelpers.GetWeaponAttackStyle();

                if (Program.currentPlayer.rand.Next(1, 4) == 3)
                {
                    chiStrikeDamage = chiStrikeDamage * 3;

                    // attack
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("** Critical Chi Strike! **");
                    Console.ResetColor();

                    hlt -= attack;
                    if (hlt <= 0)
                    {

                        monsterAlive = false;
                    }
}

                hlt -= attack;
                if (hlt <= 0)
                {

                    monsterAlive = false;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.Print($"In a blur of motion, {style} at {nm} dealing {chiStrikeDamage} damage.");
                Console.ResetColor();
            }
        }

        public static void HolyStrike(string nm, ref int hlt, ref bool monsterAlive, int attack, ref string leader, ref string style)
        {
            if (Program.currentPlayer.currentClass == Player.PlayerClass.Cleric && Program.currentPlayer.rand.Next(1, 6) == 5)
            {
                int holyStrikeDamage = 0;

                holyStrikeDamage = TextHelpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level;

                leader = TextHelpers.GetAttackStart(attack);
                style = TextHelpers.GetWeaponAttackStyle();

                if (Program.currentPlayer.rand.Next(1, 4) == 3)
                {
                    holyStrikeDamage = holyStrikeDamage * 3;

                    // attack
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("** Critical Holy Strike! **");
                    Console.ResetColor();

                    hlt -= attack;
                    if (hlt <= 0)
                    {

                        monsterAlive = false;
                    }
                }

                hlt -= attack;
                if (hlt <= 0)
                {

                    monsterAlive = false;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.Print($"You thrust your chest forward light erupting from you, {style} at {nm} dealing {holyStrikeDamage} damage.");
                Console.ResetColor();
            }
        }
    }
}
