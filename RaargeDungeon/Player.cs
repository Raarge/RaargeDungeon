using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace RaargeDungeon
{
    [XmlRootAttribute("Player", Namespace = "RaargeDungeon, IsNullable = false")]
    public class Player
    {
        
        public Random rand = new Random();

        public string name;
        public int id;
        public int coins = 25;
        public int level = 1;
        public int xp = 0;
        public int health = 0;
        public int baseHealth = 0;
        public int damage = 1;
        public int armorValue = 0;
        public int potion = 5;
        public int weaponValue = 1;
        public int damageResit = 0;
        
        public enum Race {Human, Elf, Dwarf, Gnome, Halfling, HalfOrc, Erudite}
        public Race race = Race.Human;

        public int mods = 0;

        public enum PlayerClass {Mage, Ranger, Warrior, Rogue};
        public PlayerClass currentClass = PlayerClass.Warrior;

        public int GetHealth()
        {
            int upper = (4 * mods + 12);
            int lower = (2 * mods + 5);
            return rand.Next(lower, upper);
        }
        public int GetPower()
        {
            int upper = (4 * mods + 4);
            int lower = (2 *mods + 2);
            int retVal = rand.Next(lower, upper);
            return retVal;
        }
        public int GetCoins()
        {
            int upper = (25 * mods + 101);
            int lower = (15 * mods + 21);
            return rand.Next(lower, upper);
        }

        public int GetPotionHealValue()
        {
            int upper = (4 * mods + 11) + ((currentClass == PlayerClass.Mage) ? +3 : 0); // if a mage add extra heal
            int lower = (2 * mods +  5) + ((currentClass == PlayerClass.Mage)? + 3:0);
            return rand.Next(lower, upper);
        }

        public int GetXP()
        {
            int upper = (20 * mods + 50);
            int lower = (15 * mods + 10);
            return rand.Next(lower, upper);
        }

        public int GetLevelUpValue()
        {
            return (100 * level) + 400;
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
            Program.Print($"Ding!!!! You are now level {level}! ");
            Console.ResetColor();
        }
        #endregion
    }
}
