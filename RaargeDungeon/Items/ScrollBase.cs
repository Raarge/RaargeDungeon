using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaargeDungeon.Items
{
    public class ScrollBase
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public int RequiredLevel { get; set; }
        public int SpellCost { get; set; }
        public int CurrentSpellCost { get; set; }
        public int MinCost { get; set; }
        public string FlavorText { get; set; }
        public bool savingThrowAllowed { get; set; }


    }
}
