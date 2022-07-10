using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RaargeDungeon
{
    public static class Skills
    {
        public static void CombatStealing(string nm)
        {
            // Combat Stealing
            if (Program.currentPlayer.race == Player.Race.Halfling && Helpers.rand.Next(1, 11) == 5)
            {
                int purseCoins = Helpers.rand.Next(1, 15) * ((Program.currentPlayer.mods * 1) + 10);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.Print($"You notice a small purse hanging from {nm}'s belt");
                Console.WriteLine($"You reach out and slit the purse swiftly catching {purseCoins} gold coins! ");
                Console.ResetColor();
                Program.currentPlayer.coins += purseCoins;
            }
            else if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && Helpers.rand.Next(1, 11) == 5)
            {
                int purseCoins = Helpers.rand.Next(1, 15) * ((Program.currentPlayer.mods * 1) + 10);

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
                if (Program.currentPlayer.rand.Next(1, 4) == 3)
                {
                    backStabDamage = Helpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level;
                    backStabDamage = backStabDamage * 3;

                    leader = Helpers.GetAttackStart(attack);
                    style = Helpers.GetWeaponAttackStyle();

                    // attack
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("** Critical Backstab! **");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Program.Print($"Quickly stepping behind, {style} at {nm} dealing {backStabDamage} damage.");
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
                    backStabDamage = Helpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level;

                    leader = Helpers.GetAttackStart(attack);
                    style = Helpers.GetWeaponAttackStyle();

                    // attack
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Program.Print($"Quickly stepping behind, {style} at {nm} dealing {backStabDamage} damage.");
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

        public static void SpellBlast(string nm, ref int hlt, ref bool monsterAlive, int attack, ref string leader, ref string style, ref string spellType)
        {
            // mage spell blast 25% chance
            if (Program.currentPlayer.currentClass == Player.PlayerClass.Mage && Helpers.rand.Next(1, 5) == 3)
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

                        spellDamage = Helpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level + (Program.currentPlayer.level / 2);
                        spellDamage = spellDamage * 4;
                    }
                    else
                    {
                        spellDamage = Helpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level;
                        spellDamage = spellDamage * 4;
                    }

                    leader = Helpers.GetAttackStart(attack);
                    style = Helpers.GetWeaponAttackStyle();
                    spellType = Helpers.GetSpellType();

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
                        spellDamage = Helpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level + (Program.currentPlayer.level / 2);
                    }
                    else
                    {
                        spellDamage = Helpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level;
                    }


                    leader = Helpers.GetAttackStart(attack);
                    style = Helpers.GetWeaponAttackStyle();
                    spellType = Helpers.GetSpellType();

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
            if (Program.currentPlayer.currentClass == Player.PlayerClass.Ranger && Helpers.rand.Next(1, 5) == 3)
            {
                int animalCallDamage = 0;
                if (Program.currentPlayer.rand.Next(1, 4) == 3)
                {
                    animalCallDamage = Helpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level;
                    animalCallDamage = animalCallDamage * 2;

                    leader = Helpers.GetAttackStart(attack);
                    style = Helpers.GetWeaponAttackStyle();
                    companion = Helpers.GetAnimalCompanion();

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
                    animalCallDamage = Helpers.rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level;

                    leader = Helpers.GetAttackStart(attack);
                    style = Helpers.GetWeaponAttackStyle();
                    companion = Helpers.GetAnimalCompanion();

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
    }
}
