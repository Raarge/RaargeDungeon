using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaargeDungeon.Items;

namespace RaargeDungeon.Helpers
{
    public class Pickers
    {
        public static SpellScroll GetChosenSpell()
        {
            SpellScroll spellChosen = new SpellScroll();
            List<string> shortNames = new List<string>();
            bool check = false;

            Console.Write("Available Spells: ");
            foreach (SpellScroll spell in Program.currentPlayer.spells)
            {
                Console.Write($"{spell.Name} ({spell.ShortName}), ");
                shortNames.Add(spell.ShortName);
            }

            Console.WriteLine(" ");
            Console.WriteLine("Which spell will you cast?");
            string input = Console.ReadLine().ToLower();

            while (check == false)
            {
                foreach(string shortName in shortNames)
                {
                    if(shortName == input)
                    {
                        check = true;
                        break;
                    }
                }

                if(check == false)
                {
                    Console.WriteLine("You didn't choose a valid spell. Enter spell.");
                    input = Console.ReadLine().ToLower();
                }
            }

            foreach(SpellScroll currentSpell in Program.currentPlayer.spells)
            {
                if(currentSpell.ShortName == input)
                {
                    spellChosen = currentSpell;
                    break;
                }
            }

            return spellChosen;
        }
    }
}
