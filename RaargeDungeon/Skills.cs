using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaargeDungeon.Creatures;
using RaargeDungeon.Helpers;


namespace RaargeDungeon
{
    public static class Skills
    {
        public static void CombatStealing(string nm)
        {
            // add skillxp gains for using this at a greatly reduced xp gain.

            // Combat Stealing
            if (Program.currentPlayer.race == Player.Race.Halfling && Randomizer.GetRandomNumber(11) == 5)
            {
                int purseCoins = Randomizer.GetRandomNumber(15) * ((Program.currentPlayer.mods * 1) + 10);

                Console.ForegroundColor = ConsoleColor.Yellow;
                UIHelpers.Print($"You notice a small purse hanging from {nm}'s belt");
                Console.WriteLine($"You reach out and slit the purse swiftly catching {purseCoins} gold coins! ");
                Console.ResetColor();
                Program.currentPlayer.coins += purseCoins;
            }
            else if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && Randomizer.GetRandomNumber(11) == 5)
            {
                int purseCoins = Randomizer.GetRandomNumber(15) * ((Program.currentPlayer.mods * 1) + 10);

                Console.ForegroundColor = ConsoleColor.Yellow;
                UIHelpers.Print($"You notice a small purse hanging from {nm}'s belt");
                Console.WriteLine($"You reach out and slit the purse swiftly catching {purseCoins} gold coins! ");
                Console.ResetColor();
                Program.currentPlayer.coins += purseCoins;
            }
        }

        public static Monster BackStab(Monster m, Player p, ref string leader, ref string style)
        {
            if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && Program.currentPlayer.rand.Next(1, 6) == 5)
            {
                int backStabDamage = 0;

                backStabDamage = Randomizer.GetRandomDieRoll(p.attackDie, p.numberAttackDie, Player.GetModifier(p.dexterity)) + (p.level/3) + (p.weaponValue/2);

                leader = TextHelpers.GetAttackStart(backStabDamage);
                style = TextHelpers.GetWeaponAttackStyle();

                if (Program.currentPlayer.rand.Next(1, 4) == 3)
                {                    
                    backStabDamage = backStabDamage * 3;

                    // attack
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("** Critical Backstab! **");
                    Console.ResetColor();                    
                    
                }
                
                m.health -= backStabDamage;
                if (m.health <= 0)
                {

                    m.IsAlive = false;
                }

                p.magicMastery += Checkers.GetSkillXPGain(m.level, 3, 1, "magic");
                p.spellChanneling += Checkers.GetSkillXPGain(m.level, 3, 1, "channel");
                p.spellCasting += Checkers.GetSkillXPGain(m.level, 3, 1, "cast");

                Console.ForegroundColor = ConsoleColor.Yellow;
                UIHelpers.Print($"Quickly stepping behind, {style} at {m.name} dealing {backStabDamage} damage.");
                Console.ResetColor();
            }
            return m;
        }


        public static Monster SpellBlast(Monster m, Player p, ref string leader, ref string style, ref string spellType)
        {
            int spellDamage = 0;
            int regSpellCritTry = Program.currentPlayer.rand.Next(1, 6);
            int gnomeEruditeCritTry = Program.currentPlayer.rand.Next(1, 4);

            // mage spell blast 25% chance
            if (p.currentClass == Player.PlayerClass.Mage && Randomizer.GetRandomNumber(5) == 3)
            {
                spellDamage = Randomizer.GetRandomDieRoll(p.attackDie, p.numberAttackDie, Player.GetModifier(p.intelligence) + (int)p.magicMastery + (Program.currentPlayer.level / 2));

                if (regSpellCritTry == 3 || ((Program.currentPlayer.race == Player.Race.Gnome || Program.currentPlayer.race == Player.Race.Erudite) &&
                    gnomeEruditeCritTry == 2))
                {
                    if (Program.currentPlayer.race == Player.Race.Gnome || Program.currentPlayer.race == Player.Race.Erudite)
                    {
                        spellDamage += (int)p.magicMastery;
                        spellDamage = spellDamage * 4;
                    }
                    else
                    {
                        spellDamage = spellDamage * 4;
                    }

                    leader = TextHelpers.GetAttackStart(spellDamage);
                    style = TextHelpers.GetWeaponAttackStyle();
                    spellType = TextHelpers.GetSpellType();

                    // attack
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"** Critical {spellType} spell attack! **");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    UIHelpers.Print($"You gesture chanting a cantrip, a pulse of arcane energy strikes a {m.name} for ");
                    UIHelpers.Print($"{spellDamage} damage.");
                    Console.ResetColor();

                    p.magicMastery += Checkers.GetSkillXPGain(m.level, 3, 1, "magic");
                    p.spellChanneling += Checkers.GetSkillXPGain(m.level, 3, 1, "channel");
                    p.spellCasting += Checkers.GetSkillXPGain(m.level, 3, 1, "cast");
                    m.health -= spellDamage;
                    if (m.health <= 0)
                    {
                        //Console.WriteLine($"{nm} was Slain!!");
                        m.IsAlive = false;
                    }
                }
                else
                {
                    if (Program.currentPlayer.race == Player.Race.Gnome || Program.currentPlayer.race == Player.Race.Erudite)
                    {
                        spellDamage += (int)p.magicMastery;
                    }
                    
                    leader = TextHelpers.GetAttackStart(spellDamage);
                    style = TextHelpers.GetWeaponAttackStyle();
                    spellType = TextHelpers.GetSpellType();

                    // attack
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    UIHelpers.Print($"You gesture chanting loudly, a {spellType} materializes striking a {m.name} dealing ");
                    UIHelpers.Print($"{spellDamage} damage.");
                    Console.ResetColor();
                    m.health -= spellDamage;
                    if (m.health <= 0)
                    {
                        //Console.WriteLine($"{nm} was Slain!!");
                        m.IsAlive = false;
                    }
                }
            }
            return m;
        }

        public static Monster AnimalCall(Monster m, Player p, ref string leader, ref string style, ref string companion)
        {
            if (p.currentClass == Player.PlayerClass.Ranger && Randomizer.GetRandomNumber(5) == 3)
            {
                int animalCallDamage = 0;

                animalCallDamage = Randomizer.GetRandomDieRoll(p.attackDie, p.numberAttackDie, Player.GetModifier(p.intelligence)) + (Program.currentPlayer.level / 2) + (p.weaponValue / 2);

                if (Randomizer.GetRandomNumber(4) == 3)
                {
                    animalCallDamage = animalCallDamage * 2;

                    leader = TextHelpers.GetAttackStart(animalCallDamage);
                    style = TextHelpers.GetWeaponAttackStyle();
                    companion = TextHelpers.GetAnimalCompanion();

                    // attack
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"** Critical {companion} attack! **");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    UIHelpers.Print($"You whistle loudly, a {companion} comes out of nowhere attacking {m.name} dealing ");
                    UIHelpers.Print($"{animalCallDamage} damage.");
                    Console.ResetColor();
                    m.health -= animalCallDamage;
                    if (m.health <= 0)
                    {
                        // Console.WriteLine($"{nm} was Slain!!");
                        m.IsAlive = false;
                    }
                }
                else
                {
                    leader = TextHelpers.GetAttackStart(animalCallDamage);
                    style = TextHelpers.GetWeaponAttackStyle();
                    companion = TextHelpers.GetAnimalCompanion();

                    // attack
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    UIHelpers.Print($"You whistle loudly, a {companion} comes out of nowhere attacking {m.name} dealing ");
                    UIHelpers.Print($"{animalCallDamage} damage.");
                    Console.ResetColor();

                    p.magicMastery += Checkers.GetSkillXPGain(m.level, 3, 1, "magic");
                    p.spellChanneling += Checkers.GetSkillXPGain(m.level, 3, 1, "channel");
                    p.spellCasting += Checkers.GetSkillXPGain(m.level, 3, 1, "cast");
                    if (animalCallDamage > m.health)
                        animalCallDamage = m.health;
                    m.health -= animalCallDamage;
                    if (m.health <= 0)
                    {
                        //Console.WriteLine($"{nm} was Slain!!");
                        m.IsAlive = false;
                    }
                }
            }
            return m;
        }

        public static Monster ChiStrike(Monster m, Player p, ref string leader, ref string style)
        {
            if (p.currentClass == Player.PlayerClass.Monk && Randomizer.GetRandomNumber(6) == 5)
            {
                int chiStrikeDamage = 0;

                chiStrikeDamage = Randomizer.GetRandomDieRoll(p.attackDie, p.numberAttackDie, Player.GetModifier(p.dexterity)) + (p.level / 2) + (p.weaponValue / 2);

                leader = TextHelpers.GetAttackStart(chiStrikeDamage);
                style = TextHelpers.GetWeaponAttackStyle();

                if (Program.currentPlayer.rand.Next(1, 4) == 3)
                {
                    chiStrikeDamage = chiStrikeDamage * 3;

                    // attack
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("** Critical Chi Strike! **");
                    Console.ResetColor();

                    m.health -= chiStrikeDamage;
                    if (m.health <= 0)
                    {

                        m.IsAlive = false;
                    }
}
                if (chiStrikeDamage > m.health)
                    chiStrikeDamage = m.health;
                m.health -= chiStrikeDamage;
                if (m.health <= 0)
                {

                    m.IsAlive = false;
                    
                }

                p.magicMastery += Checkers.GetSkillXPGain(m.level, 3, 1, "magic");
                p.spellChanneling += Checkers.GetSkillXPGain(m.level, 3, 1, "channel");
                p.spellCasting += Checkers.GetSkillXPGain(m.level, 3, 1, "cast");

                Console.ForegroundColor = ConsoleColor.Yellow;
                UIHelpers.Print($"In a blur of motion, {style} at {m.name} dealing {chiStrikeDamage} damage.");
                Console.ResetColor();
            }
            return m;
        }

        public static Monster HolyStrike(Monster m, Player p, ref string leader, ref string style)
        {
            if (p.currentClass == Player.PlayerClass.Cleric && Randomizer.GetRandomDieRoll(6) == 5)
            {
                int holyStrikeDamage = 0;

                holyStrikeDamage = Randomizer.GetRandomDieRoll(p.attackDie, p.numberAttackDie, Player.GetModifier(p.wisdom)) + (Program.currentPlayer.level / 2) + (p.weaponValue / 2);

                leader = TextHelpers.GetAttackStart(holyStrikeDamage);
                style = TextHelpers.GetWeaponAttackStyle();

                if (Program.currentPlayer.rand.Next(1, 4) == 3)
                {
                    holyStrikeDamage = holyStrikeDamage * 3;

                    // attack
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("** Critical Holy Strike! **");
                    Console.ResetColor();
                    
                }

                if (holyStrikeDamage > m.health)
                    holyStrikeDamage = m.health;
                m.health -= holyStrikeDamage;

                if (m.health <= 0)
                {

                    m.IsAlive = false;
                }

                p.magicMastery += Checkers.GetSkillXPGain(m.level, 3, 1, "magic");
                p.spellChanneling += Checkers.GetSkillXPGain(m.level, 3, 1, "channel");
                p.spellCasting += Checkers.GetSkillXPGain(m.level, 3, 1, "cast");

                Console.ForegroundColor = ConsoleColor.Yellow;
                UIHelpers.Print($"You thrust your chest forward light erupting from you, {style} at {m.name} dealing {holyStrikeDamage} damage.");
                Console.ResetColor();
            }
            return m;
        }
    }
}
