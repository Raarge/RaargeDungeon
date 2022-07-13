using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaargeDungeon.Creatures
{
    public class BaseCreature
    { 
        public string name { get; set; }
        public int strength { get; set; }
        public int constitution { get; set; }
        public int dexterity { get; set; }
        public int intelligence { get; set; }
        public int wisdom { get; set; }
        public int charisma { get; set; }

        public int GetModifier(int stat)
        {
            int modifier = 0;

            switch (stat)
            {
                case 1:
                    modifier = -5;
                    break;
                case 2:
                case 3:
                    modifier = -4;
                    break;
                case 4:
                case 5:
                    modifier = -3;
                    break;
                case 6:
                case 7:
                    modifier = -2;
                    break;
                case 8:
                case 9:
                    modifier = -1;
                    break;
                case 10:
                case 11:
                    modifier = 0;
                    break;
                case 12:
                case 13:
                    modifier = +1;
                    break;
                case 14:
                case 15:
                    modifier = +2;
                    break;
                case 16:
                case 17:
                    modifier = +3;
                    break;
                case 18:
                case 19:
                    modifier = +4;
                    break;
                case 20:
                case 21:
                    modifier = +5;
                    break;
                case 22:
                case 23:
                    modifier = +6;
                    break;
                case 24:
                case 25:
                    modifier = +7;
                    break;
                case 26:
                case 27:
                    modifier = +8;
                    break;
                case 28:
                case 29:
                    modifier = +9;
                    break;
                case 30:
                case > 30:
                    modifier = +10;
                    break;
                default:
                    break;
            }

            return modifier;
        }
    }
}
