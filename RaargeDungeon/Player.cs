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
        public int health = 10;
        public int baseHealth = 10;
        public int damage = 1;
        public int armorValue = 0;
        public int potion = 5;
        public int weaponValue = 1;

        public int mods = 0;

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
            int upper = (4 * mods + 11);
            int lower = (2 * mods +  5);
            return rand.Next(lower, upper);
        }
    }
}
