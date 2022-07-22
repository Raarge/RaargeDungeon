using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaargeDungeon.Classes
{
    public class BaseClass
    {
        public int proficiencyBonus { get; set; }

        public static List<string> SavingThrowProficiencies = new List<string>();

        public static Dictionary<int, int> proficiencies = new Dictionary<int, int>();
        public static int proficiencybonus { get; set; }

    }
}
