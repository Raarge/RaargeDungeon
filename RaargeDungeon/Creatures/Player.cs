using System;
using RaargeDungeon.Items;
using System.Collections.Generic;
using RaargeDungeon.Helpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using RaargeDungeon.Classes;
using RaargeDungeon.Tests;

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
        public int energy {get; set;}
        public int spellLevelDenominator { get; set; }
        public int baseEnergy { get; set; }
        public int damage = 1;
        public int armorValue { get; set; }
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

        // learning skills
        public decimal magicMastery = 0.0m;
        public decimal spellCasting = 0.0m;
        public decimal spellChanneling = 0.0m;
        public decimal weaponMastery = 0.0m;
        public decimal evasion = 0.0m;
        public decimal armorMastery = 0.0m;

        public enum skillCombatType { armor, evasion, weapon};
        public skillCombatType sct = skillCombatType.armor;

        public enum Race { Human, Elf, Dwarf, Gnome, Halfling, HalfOrc, Erudite }
        public Race race = Race.Human;
        public string RaceModComment { get; set; }

        public int mods = 0;

        public enum PlayerClass { Mage, Ranger, Warrior, Rogue, Cleric, Monk };
        public PlayerClass currentClass = PlayerClass.Warrior;

        public enum ManaType { Mana, Chi, Kri, Rage }
        public ManaType manaType = ManaType.Mana;

        public List<string> saveThrowProf { get; set; }

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
            p.baseEnergy += (Randomizer.GetRandomNumber(p.intelligence) * 3) + (Player.GetModifier(p.intelligence) * 2) + ((int)(p.spellChanneling / 2) * 2);
            p.energy = p.baseEnergy;
            p.spellLevelDenominator = GetDenominator(p);

            p = GetRacialStatMods(p);
            p.proficiencyBonus = GetProficiencyBonus(p);
            p.saveThrowProf = GetBonusSavingThrows(p);

            p.strengthSave = GetSavingThrowModifier(p, "strength");
            p.constitutionSave = GetSavingThrowModifier(p, "constitution");
            p.dexteritySave = GetSavingThrowModifier(p, "dexterity");
            p.intelligenceSave = GetSavingThrowModifier(p, "intelligence");
            p.wisdomSave = GetSavingThrowModifier(p, "wisdom");
            p.charisma = GetSavingThrowModifier(p, "charisma");
            
            // Set Armor Class
            p.armorclass = GetPlayerArmorClass(p);

            // Set Spell DC 
            p.spellDcCheck = GetSpellDcCheck(p.intelligence, p.level, (int)p.magicMastery);

            // Set Attack Die
            p = GetAttackDie(p);

            //StatTests.DisplayProficiencyAndSaveScoresCalculated(p);

            return p;
        }

        public static int GetDenominator(Player p)
        {
            int denominator = 0;

            if (p.currentClass == PlayerClass.Mage)
                denominator = 2;
            else if (p.currentClass == PlayerClass.Cleric)
                denominator = 2;
            else
                denominator = 0;

            return denominator;
        }

        public static int GetSavingThrowModifier(Player p, string typeSave)
        {
            bool proficientSave = false;
            int statScore = 0;
            int saveModifier = 0;

            foreach(string save in p.saveThrowProf)
            {
                if(save.ToLower() == typeSave)
                {
                    proficientSave = true;                    
                }
            }

            if (typeSave == "strength")
                statScore = p.strength;
            else if (typeSave == "constitution")
                statScore = p.constitution;
            else if (typeSave == "dexterity")
                statScore = p.dexterity;
            else if (typeSave == "intelligence")
                statScore = p.intelligence;
            else if (typeSave == "wisdom")
                statScore = p.wisdom;
            else if (typeSave == "charisma")
                statScore = p.charisma;


            if (proficientSave)
                saveModifier = p.proficiencyBonus + GetModifier(statScore);
            else
                saveModifier = GetModifier(statScore);

            return saveModifier;
        }

        public static List<string> GetBonusSavingThrows(Player p)
        {
            switch (p.currentClass.ToString().ToLower())
            {
                case "cleric":
                    p.saveThrowProf = ClericClass.getSaveThrowProficiencies();
                    break;
                case "mage":
                    p.saveThrowProf = MageClass.getSaveThrowProficiencies();
                    break;
                case "ranger":
                    p.saveThrowProf = RangerClass.getSaveThrowProficiencies();
                    break;
                case "warrior":
                    p.saveThrowProf = WarriorClass.getSaveThrowProficiencies();
                    break;
                case "rogue":
                    p.saveThrowProf = RogueClass.getSaveThrowProficiencies();
                    break;
                case "monk":
                    p.saveThrowProf = MonkClass.getSaveThrowProficiencies();
                    break;
                default:
                    break;
            }
            return p.saveThrowProf;
        } 

        public static int GetProficiencyBonus(Player p)
        {
            switch (p.currentClass.ToString().ToLower())
            {
                case "cleric":
                    p.proficiencyBonus = ClericClass.getProficiencyBonus(p.level);
                    break;
                case "mage":
                    p.proficiencyBonus = MageClass.getProficiencyBonus(p.level);
                    break;
                case "ranger":
                    p.proficiencyBonus = RangerClass.getProficiencyBonus(p.level);
                    break;
                case "warrior":
                    p.proficiencyBonus = WarriorClass.getProficiencyBonus(p.level);
                    break;
                case "rogue":
                    p.proficiencyBonus = RogueClass.getProficiencyBonus(p.level);
                    break;
                case "monk":
                    p.proficiencyBonus = MonkClass.getProficiencyBonus(p.level);
                    break;
                default:
                    p.proficiencyBonus = 0;
                    break;                                 
            }

            return p.proficiencyBonus;
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
        
        public int GetCoins(Monster m)
        {
            int upper = 3 * (m.xpGiven + 1);
            int lower = 1 * m.xpGiven;
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

        public int GetXP(Monster m)
        {
            return m.xpGiven;
        }

        public int GetLevelUpValue(int currentLevel)
        {
            int xpneeded = 0;

            Dictionary<int, int> xpNeeded = new Dictionary<int, int>()
            {
                {1, 300 },
                {2, 900 },
                {3, 2700 },
                {4, 6500 },
                {5, 14000 },
                {6, 23000 },
                {7, 34000 },
                {8, 48000 },
                {9, 64000 },
                {10, 85000 },
                {11, 100000 },
                {12, 120000 },
                {13, 140000 },
                {14, 165000 },
                {15, 195000 },
                {16, 225000 },
                {17, 265000 },
                {18, 305000 },
                {19, 355000 }
            };

            foreach (var index in xpNeeded)
            {
                if(index.Key == currentLevel)
                {
                    xpneeded = index.Value;
                    break;
                }
                    
            }

            return xpneeded;
        }

        public bool CanLevelUp(int currentlvl)
        {
            if (xp >= GetLevelUpValue(currentlvl))
            {
                return true;
            }
            else
                return false;
        }

        #region PlayerLevelUp
        public Player LevelUp(Player p)
        {

            while (CanLevelUp(p.level))
            {
                
                p.level++;
                p.baseHealth += Randomizer.GetRandomDieRoll(p.hitDice, 1, Player.GetModifier(p.constitution));
                p.baseEnergy += (Randomizer.GetRandomNumber(p.intelligence) * 3) + (Player.GetModifier(p.intelligence) * 2) + ((int)(p.spellChanneling / 2) * 2);

                // recalculate proficiency bonus
                p.proficiencyBonus = GetProficiencyBonus(p);

                //recalculate saves modifiers
                p.strengthSave = GetSavingThrowModifier(p, "strength");
                p.constitutionSave = GetSavingThrowModifier(p, "constitution");
                p.dexteritySave = GetSavingThrowModifier(p, "dexterity");
                p.intelligenceSave = GetSavingThrowModifier(p, "intelligence");
                p.wisdomSave = GetSavingThrowModifier(p, "wisdom");
                p.charisma = GetSavingThrowModifier(p, "charisma");
                p = LevelSpells(p);
                switch (p.level)
                {
                    case 3:
                        if (p.currentClass == PlayerClass.Warrior || p.currentClass == PlayerClass.Ranger || p.currentClass == PlayerClass.Rogue)
                            p.numberAttackDie++;
                        break;
                    case 4:
                        if (p.currentClass == PlayerClass.Warrior)
                            p.strength++;
                        if (p.currentClass == PlayerClass.Cleric)
                            p.wisdom++;
                        if (p.currentClass == PlayerClass.Monk || p.currentClass == PlayerClass.Ranger || p.currentClass == PlayerClass.Rogue)
                            p.dexterity++;
                        if (p.currentClass == PlayerClass.Mage)
                            p.intelligence++;
                        mods++;
                        break;
                    case 6:
                        if (p.currentClass == PlayerClass.Warrior)
                            p.constitution++;
                        if (p.currentClass == PlayerClass.Cleric || p.currentClass == PlayerClass.Monk || p.currentClass == PlayerClass.Mage)
                            p.numberAttackDie++;
                        break;
                    case 8:
                        if (p.currentClass == PlayerClass.Warrior)
                            p.strength++;
                        if (p.currentClass == PlayerClass.Cleric || p.currentClass == PlayerClass.Mage)
                            p.constitution++;
                        if (p.currentClass == PlayerClass.Monk || p.currentClass == PlayerClass.Ranger)
                            p.wisdom++;
                        if (p.currentClass == PlayerClass.Rogue)
                            p.intelligence++;
                        mods++;
                        break;
                    case 9:
                        break;
                    case 11:
                        if (p.currentClass == PlayerClass.Warrior || p.currentClass == PlayerClass.Ranger || p.currentClass == PlayerClass.Ranger)
                            p.numberAttackDie++;
                        break;
                    case 12:
                        if (p.currentClass == PlayerClass.Warrior)
                            p.constitution++;
                        if (p.currentClass == PlayerClass.Cleric)
                            p.wisdom++;
                        if (p.currentClass == PlayerClass.Monk || p.currentClass == PlayerClass.Ranger || p.currentClass == PlayerClass.Rogue)
                            p.dexterity++;
                        if (p.currentClass == PlayerClass.Mage)
                            p.intelligence++;
                        mods++;
                        break;
                    case 14:
                        if (p.currentClass == PlayerClass.Warrior)
                            p.strength++;
                        break;
                    case 15:
                        break;
                    case 16:
                        if (p.currentClass == PlayerClass.Warrior)
                            p.constitution++;
                        if (p.currentClass == PlayerClass.Cleric || p.currentClass == PlayerClass.Mage)
                            p.constitution++;
                        if (p.currentClass == PlayerClass.Monk || p.currentClass == PlayerClass.Ranger)
                            p.wisdom++;
                        if (p.currentClass == PlayerClass.Rogue)
                            p.intelligence++;
                        mods++;
                        break;
                    case 18:
                        if (p.currentClass == PlayerClass.Cleric || p.currentClass == PlayerClass.Monk || p.currentClass == PlayerClass.Mage)
                            p.numberAttackDie++;
                        break;
                    case 19:
                        if (p.currentClass == PlayerClass.Warrior)
                            p.strength++;
                        if (p.currentClass == PlayerClass.Cleric)
                            p.wisdom++;
                        if (p.currentClass == PlayerClass.Monk || p.currentClass == PlayerClass.Ranger || p.currentClass == PlayerClass.Rogue)
                            p.dexterity++;
                        if (p.currentClass == PlayerClass.Mage)
                            p.intelligence++;
                        break;
                    case 20:
                        if (p.currentClass == PlayerClass.Warrior || p.currentClass == PlayerClass.Ranger || p.currentClass == PlayerClass.Rogue)
                            p.numberAttackDie++;
                        mods++;
                        break;
                    default:
                        break;

                }

                
            }

            p.health = p.baseHealth;
            p.energy = p.baseEnergy;
            Console.ForegroundColor = ConsoleColor.Green;
            UIHelpers.Print($"Ding!!!! You are now level {p.level}! ");
            Console.ResetColor();

            //StatTests.DisplayProficiencyAndSaveScoresCalculated(p);

            return p;
        }
        
        public Player LevelSpells(Player p)
        {
            SpellScroll[] spells = p.spells.ToArray();

            for(var i = 0; i <= spells.Length -1 ; i++)
            {
                if(p.level % p.spellLevelDenominator == 0)
                {
                    var spellName = spells[i].Name;
                    var numDice = spells[i].spellNumberDamageDice;
                    p.spells.Find(s => s.Name == spellName).spellNumberDamageDice = numDice + 1;
                }
            }

            return p;
        
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

        #region Racial Stat Modifiers
        public static Player GetRacialStatMods(Player p)
        {
            switch (p.race.ToString())
            {
                case "Human":
                    p.strength += 1;
                    p.constitution += 1;
                    p.dexterity += 1;
                    p.intelligence += 1;
                    p.wisdom += 1;
                    p.charisma += 1;
                    p.RaceModComment = "+1 All";
                    break;
                case "Elf":
                case "Halfling":
                    p.dexterity += 2;
                    p.RaceModComment = "+2 Dex";
                    break;
                case "Gnome":
                case "Erudite":
                    p.intelligence += 2;
                    p.RaceModComment = "+2 Int";
                    break;
                case "Dwarf":
                    p.constitution += 2;
                    p.RaceModComment = "+2 Con";
                    break;
                case "HalfOrc":
                    p.strength += 2;
                    p.constitution += 1;
                    p.RaceModComment = "+2 Str, +1 Con";
                    break;
                default:
                    p.RaceModComment = "No Mod Found Error";
                    break;
            }


            return p;
        }
        #endregion
    }
}
