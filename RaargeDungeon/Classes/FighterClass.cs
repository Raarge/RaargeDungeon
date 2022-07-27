using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaargeDungeon.Classes
{
    public class FighterClass : BaseClass
    {
        public static List<string> getSaveThrowProficiencies()
        {
            SavingThrowProficiencies.Add("Strength");
            SavingThrowProficiencies.Add("Constitution");

            return SavingThrowProficiencies;
        }


        public static int getProficiencyBonus(int level)
        {
            if (proficiencies.Count > 0)
            {
                return proficiencies[level];
            }
            else
            {
                proficiencies.Add(1, 2);
                proficiencies.Add(2, 2);
                proficiencies.Add(3, 2);
                proficiencies.Add(4, 2);
                proficiencies.Add(5, 3);
                proficiencies.Add(6, 3);
                proficiencies.Add(7, 3);
                proficiencies.Add(8, 3);
                proficiencies.Add(9, 4);
                proficiencies.Add(10, 4);
                proficiencies.Add(11, 4);
                proficiencies.Add(12, 4);
                proficiencies.Add(13, 5);
                proficiencies.Add(14, 5);
                proficiencies.Add(15, 5);
                proficiencies.Add(16, 5);
                proficiencies.Add(17, 6);
                proficiencies.Add(18, 6);
                proficiencies.Add(19, 6);
                proficiencies.Add(20, 6);

                return proficiencies[level];
            }
        }
    }
}
