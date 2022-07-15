using System;
using RaargeDungeon.Items;
using System.Collections.Generic;
using RaargeDungeon.Helpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace RaargeDungeon.Creatures
{
    [XmlRoot("Player", Namespace = "RaargeDungeon, IsNullable = false")]
    public class Player : BaseCreature
    {

        public Random rand = new Random();

        public List<SpellScroll> spells = new List<SpellScroll>();

        public int id;
        public int coins = 25;
        public int level = 1;
        public int xp = 0;
        public int health = 0;
        public int baseHealth = 0;
        public int energy = 30;
        public int baseEnergy = 30;
        public int damage = 1;
        public int armorValue = 0;
        public int armorclass { get; set; }
        public int potion = 5;
        public int manaPotion = 5;
        public int weaponValue = 1;
        public int damageResit = 0;
        public int favors = 0;
        public int lifetimeFavors = 0;
        public int deaths = 0;
        public int attackDie { get; set; }
        public int numberAttackDie { get; set; }
        

        public int hitDice { get; set; }
        public int numberHitDice { get; set; }

        public decimal magicMastery = 0.0m;
        public decimal spellCasting = 0.0m;
        public decimal spellChanneling = 0.0m;

        public enum Race { Human, Elf, Dwarf, Gnome, Halfling, HalfOrc, Erudite }
        public Race race = Race.Human;

        public int mods = 0;

        public enum PlayerClass { Mage, Ranger, Warrior, Rogue, Cleric, Monk };
        public PlayerClass currentClass = PlayerClass.Warrior;

        public enum ManaType { Mana, Chi, Kri, Rage }
        public ManaType manaType = ManaType.Mana;

        public static Player BuildOutPlayer(Player p)
        {
            // Get Stats
            p.strength = Randomizer.GetPlayerStats();
            p.constitution = Randomizer.GetPlayerStats();
            p.dexterity = Randomizer.GetPlayerStats();
            p.intelligence = Randomizer.GetPlayerStats();
            p.wisdom = Randomizer.GetPlayerStats();
            p.charisma = Randomizer.GetPlayerStats();

            // Hit Points Setter
            p.hitDice = GetPlayerHitDice(p);
            p.numberHitDice = p.level;
            p.baseHealth = Randomizer.GetHealth(p.hitDice, p.numberHitDice, Player.GetModifier(p.constitution));
            p.health = p.baseHealth;

            // Set Armor Class
            p.armorValue = GetPlayerArmorClass(p);

            // Set Spell DC 
            p.spellDcCheck = GetSpellDcCheck(p.intelligence, p.level, (int)p.magicMastery);

            // Set Attack Die
            p = GetAttackDie(p);

            return p;
        }

        public static Player GetAttackDie(Player p)
        {
            

            switch (p.currentClass.ToString())
            {
                case "Mage":
                    p.attackDie = 6;
                    p.numberAttackDie = 1;
                    break;
                case "Ranger":
                    p.attackDie = 8;
                    p.numberAttackDie = 1;
                    break;
                case "Warrior":
                    p.attackDie = 6;
                    p.numberAttackDie = 2;
                    break;
                case "Rogue":
                    p.attackDie = 4;
                    p.numberAttackDie = 2;
                    break;
                case "Cleric":
                    p.attackDie = 6;
                    p.numberAttackDie = 1;
                    break;
                case "Monk":
                    p = GetMonkAttackDie(p);
                    break;
                default:
                    p.attackDie = 6;
                    p.numberAttackDie = 1;
                    break;
            }
            return p;
        }

        public static Player GetMonkAttackDie(Player p)
        {
            switch (p.level)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    p.attackDie = 4;
                    p.numberAttackDie = 1;
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    p.attackDie = 6;
                    p.numberAttackDie = 1;
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                    p.attackDie = 8;
                    p.numberAttackDie = 1;
                    break;
                case 17:
                case 18:
                case 19:
                case 20:
                    p.attackDie = 10;
                    p.numberAttackDie = 1;
                    break;
                default:
                    p.attackDie = 12;
                    p.numberAttackDie = 1 + (p.level / 2);
                    break;
            }
            return p;
        }

        public static int GetPlayerHitDice(Player p)
        {
            int hd = 0;

            switch (p.currentClass.ToString())
            {
                case "Mage":
                    hd = 6;
                    break;
                case "Ranger":
                    hd = 10;
                    break;
                case "Warrior":
                    hd = 10;
                    break;
                case "Rogue":
                    hd = 8;
                    break;
                case "Cleric":
                    hd = 8;
                    break;
                case "Monk":
                    hd = 8;
                    break;
                default:
                    hd = 6;
                    break;
            }

            return hd;
        }

        public static int GetPlayerArmorClass(Player p)
        {
            int ac = 0;

            switch (p.currentClass.ToString())
            {
                case "Mage":
                    ac = 11;
                    break;
                case "Ranger":
                    ac = 12;
                    break;
                case "Warrior":
                    ac = 16;
                    break;
                case "Rogue":
                   ac = 12;
                    break;
                case "Cleric":
                    ac = 13;
                    break;
                case "Monk":
                    ac = 10;
                    break;
                default:
                    ac = 12;
                    break;
            }

            return ac;
        }
        
        public int GetCoins()
        {
            int upper = 25 * mods + 101;
            int lower = 15 * mods + 21;
            return rand.Next(lower, upper);
        }

        public int GetPotionHealValue(string typePotion)
        {
            int upper = 0;
            int lower = 0;

            if (typePotion == "health")
            {
                upper = 4 * mods + 11 + (currentClass == PlayerClass.Mage ? +3 : 0); // if a mage add extra heal
                lower = 2 * mods + 5 + (currentClass == PlayerClass.Mage ? +3 : 0);
            }
            else if (typePotion == "mana")
            {
                upper = 4 * mods + 11 + level * 2;
                lower = 2 * mods + 5 + level * 2;
            }

            return rand.Next(lower, upper);
        }

        public int GetXP()
        {
            int upper = 20 * mods + 50 + level;
            int lower = 15 * mods + 10 + level;
            return rand.Next(lower, upper);
        }

        public int GetLevelUpValue()
        {
            return 100 * level + 400;
        }

        public bool CanLevelUp()
        {
            if (xp >= GetLevelUpValue())
            {
                return true;
            }
            else
                return false;
        }

        #region PlayerLevelUp
        public void LevelUp()
        {
            while (CanLevelUp())
            {
                xp -= GetLevelUpValue();
                level++;
                baseHealth++;

                switch (level)
                {
                    case 3:
                        baseHealth++;
                        break;
                    case 4:
                        mods++;
                        break;
                    case 6:
                        baseHealth++;
                        break;
                    case 8:
                        mods++;
                        break;
                    case 9:
                        baseHealth++;
                        break;
                    case 12:
                        mods++;
                        baseHealth++;
                        break;
                    case 15:
                        baseHealth++;
                        break;
                    case 16:
                        mods++;
                        break;
                    case 18:
                        baseHealth++;
                        break;
                    case 20:
                        mods++;
                        break;
                    default:
                        break;

                }


            }

            health = baseHealth;
            Console.ForegroundColor = ConsoleColor.Green;
            UIHelpers.Print($"Ding!!!! You are now level {level}! ");
            Console.ResetColor();
        }
        #endregion

        #region GetFavor()
        public int GetFavor()
        {
            int favorEarned = 0;
            int favCheckRand1 = rand.Next(1, 3);
            int favCheckRand2 = rand.Next(1, 4);
            int favCheckRand3 = rand.Next(1, 5);
            int favCheckRand4 = rand.Next(1, 6);
            int favCheckRand5 = rand.Next(1, 7);
            int favCheckRand6 = rand.Next(1, 8);

            if (favors == 0)
            {
                favorEarned = 1;
                favors++;
                lifetimeFavors++;
            }
            else if (favors == 1)
            {
                if (favCheckRand1 == 1)
                {
                    favorEarned = 1;
                    favors++;
                    lifetimeFavors++;
                }
                else
                    favorEarned = 0;
            }
            else if (favors == 2)
            {
                if (favCheckRand2 == 1)
                {
                    favorEarned = 1;
                    favors++;
                    lifetimeFavors++;
                }
                else
                    favorEarned = 0;
            }
            else if (favors == 3)
            {
                if (favCheckRand3 == 1)
                {
                    favorEarned = 1;
                    favors++;
                    lifetimeFavors++;
                }
                else
                    favorEarned = 0;
            }
            else if (favors == 4)
            {
                if (favCheckRand4 == 1)
                {
                    favorEarned = 1;
                    favors++;
                    lifetimeFavors++;
                }
                else
                    favorEarned = 0;
            }
            else if (favors == 5)
            {
                if (favCheckRand5 == 1)
                {
                    favorEarned = 1;
                    favors++;
                    lifetimeFavors++;
                }
                else
                    favorEarned = 0;
            }
            else
            {
                if (favCheckRand6 == 1)
                {
                    favorEarned = 1;
                    favors++;
                    lifetimeFavors++;
                }
                else
                    favorEarned = 0;
            }

            return favorEarned;
        }
        #endregion
    }
}
