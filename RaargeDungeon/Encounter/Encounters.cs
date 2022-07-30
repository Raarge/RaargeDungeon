using RaargeDungeon.Combat;
using RaargeDungeon.Creatures;
using RaargeDungeon.Helpers;
using RaargeDungeon.Items;
using RaargeDungeon.Shops;
using System;
using System.Collections.Generic;

namespace RaargeDungeon.Encounter
{

    public class Encounters
    {
        #region EncounterGeneric
        static Random rand = new Random();
        // Encounter Generic

        #endregion

        #region Encounter
        public static void FirstEncounter(Player plyr)
        {
            //Player plyr = new Player();

            string weapon = TextHelpers.GetWeapon();
            if (plyr.currentClass == Player.PlayerClass.Wizard || plyr.currentClass == Player.PlayerClass.Cleric)
            {
                var scrollName = "";

                if (plyr.currentClass == Player.PlayerClass.Wizard)
                    plyr = SpellScroll.GetSpellScroll(plyr, "Magic Missile");
                else if (plyr.currentClass == Player.PlayerClass.Cleric)
                    plyr = SpellScroll.GetSpellScroll(plyr, "Inflict Wounds");

                foreach (var spell in plyr.spells)
                    scrollName = spell.Name;

                UIHelpers.Print($"You throw open the door, grab a {weapon} and a Spell Scroll from a table.");
                UIHelpers.Print("As your hand touches the scroll it vanishes and you realize you know a spell.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"You learn {scrollName}");
                Console.ResetColor();
                Console.WriteLine("You quickly regain control and charge you captor...");
                Console.WriteLine(" ");

                

                // test
                //Console.WriteLine("You know the following spells:");

                //foreach (SpellScroll spell in Program.currentPlayer.spells)
                //{
                //    Console.WriteLine($"{spell.Name} spell");
                //}
                //Console.ReadKey();
                // end of test
            }
            else
            {
                UIHelpers.Print($"You throw open the door, grab a {weapon} ");
                UIHelpers.Print("laying on the table by the door and charge your captor...");
            }




            Console.ReadKey();
            Combat(false, plyr);
        }

        public static void BasicFightEncounter(Player plyr)
        {
            Console.Clear();
            UIHelpers.Print("You turn the corner and there you see a ....");
            Console.ReadKey();
            Combat(true, plyr);
        }

        public static void WizardEncounter(Player p)
        {
            Console.Clear();
            UIHelpers.Print("The door slowly creaks open as you peer into the dark room.  You see a tall man with a");
            UIHelpers.Print("long beard looking at a large tome.  He looks up from his tome and stares at you menacingly....");
            Console.ReadKey();
            Combat(false, p);
        }
        #endregion

        #region EncounterTools
        //Encounter Tools
        public static void RandomEncounter(Player player)
        {
            //switch(rand.Next(0, 2))
            //{
            //case 0:
            BasicFightEncounter(player);
            //break;
            //case 1:
            //  WizardEncounter(player);
            //  break;
            //}
        }

        public static void Combat(bool random, Player plyr)
        {
            Monster mstr = new Monster();

            var encounterLvls = GetXPThresholdsByLevel(plyr.level);

            int encounterLevelChosen = GetCurrentEncounterXPLevel(encounterLvls);

            mstr = Monster.SpawnMonster(mstr, random, plyr.level, encounterLevelChosen);

            
            string leaderMob = TextHelpers.GetLeader(mstr.level);


            while (mstr.health > 0)
            {
                Console.Clear();
                Console.WriteLine($" A {leaderMob} {mstr.name} is here, looking menacingly at you");
                Console.Write($" {mstr.name}'s Health Bar: ");
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Red;
                Program.MonsterBar("<", "-", mstr.health / (decimal)mstr.baseHealth, 25);
                Console.ResetColor();
                Console.WriteLine("]");
                Console.WriteLine(" ");
                Console.WriteLine(" =====================");
                Console.WriteLine(" | (A)ttack  (D)efend |");

                if (plyr.currentClass == Player.PlayerClass.Wizard ||
                    plyr.currentClass == Player.PlayerClass.Cleric)
                {
                    Console.WriteLine(" | (C)ast            |");
                }


                Console.WriteLine(" | (R)un              |");
                if(plyr.currentClass == Player.PlayerClass.Monk)
                    Console.WriteLine(" | (H)eal             |");
                else
                    Console.WriteLine(" | (H)eal    (M)Mana  |");
                Console.WriteLine(" | (S)tats    Sa(v)e  |"   );
                Console.WriteLine(" |                    |");
                Console.WriteLine(" =====================");
                Console.WriteLine($" Level: {plyr.level} Class: {plyr.currentClass} Coins: {plyr.coins}");
                if (plyr.currentClass == Player.PlayerClass.Monk)
                    Console.WriteLine($" Potions: {plyr.potion} ");
                else
                    Console.WriteLine($" Potions: {plyr.potion} {plyr.manaType} Potions: {plyr.manaPotion}");

                // -- Health Bar --
                UIHelpers.GenerateStatusBar("Health", "<", "-", ConsoleColor.Red, plyr.health, plyr.baseHealth);

                // -- Experiance Bar --
                UIHelpers.GenerateStatusBar("XP", ">", "-", ConsoleColor.Yellow, plyr.xp, plyr.GetLevelUpValue(plyr.level));

                // -- Mana Bar --
                UIHelpers.GenerateStatusBar(plyr.manaType.ToString(), "*", " ", ConsoleColor.Blue, plyr.energy, plyr.baseEnergy);

                //Console.WriteLine($"XP:{Program.currentPlayer.xp} Needed XP: {Program.currentPlayer.GetLevelUpValue()} ");
                string action = Console.ReadLine().ToLower();

                if (Program.currentPlayer.currentClass != Player.PlayerClass.Wizard)
                {
                    while (action != "a" && action != "d" && action != "r" && action != "h" && action != "m" && action != "v" && action.Length > 1)
                    {
                        Console.WriteLine("Invalid selection, choose again");
                        action = Console.ReadLine();
                    }
                }
                else
                {
                    while (action != "a" && action != "d" && action != "r" && action != "h" && action != "m" && action != "c" && action != "v" && action.Length > 1)
                    {
                        Console.WriteLine("Invalid selection, choose again");
                        action = Console.ReadLine();
                    }
                }




                if (action.ToLower() == "a")
                {

                    string leader = "";
                    string style = "";
                    string companion = "";
                    string spellType = "";


                    Combatants c = new Combatants();

                    MartialCombat.DoAttack(plyr, mstr, "melee", action);

                    if (plyr.currentClass == Player.PlayerClass.Monk && plyr.level > 1 && plyr.energy > 0)
                    {
                        var input = "";
                        KiAbilities ability = new KiAbilities();

                        Console.WriteLine("Use Chi Ability? (y or n)");
                        input = Console.ReadLine();

                        while(input != "y" && input != "n" && input.Length != 1 && input == null)
                        {
                            Console.WriteLine("You entered a wrong entry.  Try again.");
                            Console.WriteLine("Use Chi Ability? (y or n)");
                            input = Console.ReadLine();
                        }

                        if (input == "y")
                        {
                            ability = Pickers.GetChosenChiAbility(plyr);
                            int chiCost = ability.CurrentSpellCost;

                            if (plyr.energy >= chiCost)
                            {
                                plyr.energy -= chiCost;
                                Console.WriteLine($"You attack a {mstr.name} using {ability.Name}. ");
                                for (int i = 1; i <= ability.abilityNumberAttacks; i++)
                                {
                                    MartialCombat.DoExtraAttack(plyr, mstr, ability.abilityChiType, action, ability.ShortName);
                                }
                            }
                            else
                                Console.WriteLine("You do not have enough Chi to do that!");
                        }
                        

                    }

                    if (plyr.currentClass == Player.PlayerClass.Fighter || plyr.currentClass == Player.PlayerClass.Ranger || plyr.currentClass == Player.PlayerClass.Rogue)
                    {
                        CombatAbilities ability = new CombatAbilities();



                        if(plyr.level >= 5 && plyr.level < 11)
                        {
                            ability = Pickers.GetSelectedCombatAbilityToUse(plyr, "e1");
                            
                            MartialCombat.DoExtraAttack(plyr, mstr, ability.abilityCombatType, action, ability.ShortName);
                        }
                        else if (plyr.level >= 11 && plyr.level < 20)
                        {
                            ability = Pickers.GetSelectedCombatAbilityToUse(plyr, "e2");

                            for (int i = 1; i <= ability.abilityNumberAttacks; i++)
                            {
                                MartialCombat.DoExtraAttack(plyr, mstr, ability.abilityCombatType, action, ability.ShortName);
                            }
                        }
                        else if (plyr.level == 20)
                        {
                            ability = Pickers.GetSelectedCombatAbilityToUse(plyr, "e3");

                            for (int i = 1; i <= ability.abilityNumberAttacks; i++)
                            {
                                MartialCombat.DoExtraAttack(plyr, mstr, ability.abilityCombatType, action, ability.ShortName);
                            }
                        }
                    }

                    //special attacks


                    // Rogue backstab
                    mstr = Skills.BackStab(mstr, plyr, ref leader, ref style);

                    // Ranger Animal Call 25% chance
                    mstr = Skills.AnimalCall(mstr, plyr, ref leader, ref style, ref companion);

                    // Wizard SpellBlast 
                    mstr = Skills.SpellBlast(mstr, plyr, ref leader, ref style, ref spellType);

                    // Combat Stealing
                    Skills.CombatStealing(mstr.name);

                    // Monk Chi Strike
                    mstr = Skills.ChiStrike(mstr, plyr, ref leader, ref style);

                    // Cleric Holy Strike
                    mstr = Skills.HolyStrike(mstr, plyr, ref leader, ref style);
                }
                else if (action.ToLower() == "v")
                {
                    Program.Save();
                    Console.WriteLine("You have saved successfully!");
                    Console.ReadKey();
                }
                else if (action.ToLower() == "d")
                {
                    bool playerHitTry = false;
                    HitChecks hitChecks = new HitChecks();
                    int attack = 0;

                    //attackhits bool and roll
                    hitChecks = MartialCombat.PlyrHitTry(plyr, mstr, hitChecks, "Melee");
                    
                    if(hitChecks.AttackHits == false)
                    {
                        Console.WriteLine($"You swing barely missing the {mstr.name}.");
                    }
                    else if (hitChecks.AttackHits == true)
                    {
                        attack = Randomizer.GetRandomDieRoll(plyr.attackDie, (plyr.numberAttackDie / 2)); // half damage dealt defending
                    }

                    if (hitChecks.AttackHits == true && hitChecks.ToHitRoll == 20)
                    {
                        attack *= 2;
                    }
                        


                    // defend

                    var weapon = TextHelpers.GetWeapon();
                    mstr.currentCombatOrder = 1;
                    plyr = MartialCombat.DoMonsterSoloCombat(mstr, plyr, action.ToLower());

                    UIHelpers.Print($"As {mstr.name} moves forward to attack, you ready your {weapon} to defend.");
                    if (hitChecks.AttackHits == true && hitChecks.ToHitRoll == 20)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You deal a critical hit!");
                        Console.ResetColor();
                    }
                    Console.WriteLine($"You deal {attack} damage to {mstr.name}.");
                    //                TextHelpers.GetMonsterHitLine(mstr.name, damage, monsterCrit, action.ToLower());

                    mstr.health -= attack;
                    if (mstr.health <= 0)
                    {

                        mstr.IsAlive = false;
                    }
                }
                else if (action == "c")
                {
                    SpellScroll chosenSpell = new SpellScroll();
                    bool spellCrit = false;
                    bool spellFail = false;
                    int upper = Checkers.GetSpellFailUpper();


                    chosenSpell = Pickers.GetChosenSpell();
                    int spellCost = Checkers.GetSpellCost(chosenSpell);

                    int spellAttack = (int)Math.Ceiling(Randomizer.GetRandomDieRoll(chosenSpell.spellDamageDice, chosenSpell.spellNumberDamageDice, (chosenSpell.spellDamageModifier * chosenSpell.spellNumberDamageDice)) + Program.currentPlayer.magicMastery) * (plyr.level / 2 + 1);

                    if (rand.Next(0, upper) == 0)
                        spellFail = true;

                    if (rand.Next(1, 11) == 3)
                        spellCrit = true;
                    // check for spell failure

                    if (spellFail)
                    {
                        Console.WriteLine($"You begin casting {chosenSpell.Name} but it backfires.");
                        spellAttack = 0;
                    }
                    else
                    {
                        if (spellCrit)
                        {
                            spellAttack = spellAttack * chosenSpell.critMultiplier;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{chosenSpell.Name} Critical!");
                            Console.ResetColor();
                        }

                        Console.WriteLine($"You begin casting {chosenSpell.Name}, you thrust your hand forward and");
                        Console.WriteLine(chosenSpell.FlavorText);
                        Console.WriteLine($"A {mstr.name} attacks in return.");
                        Console.WriteLine($"You deal {spellAttack} {chosenSpell.type} damage to the {mstr.name}.");

                        plyr = MartialCombat.DoMonsterSoloCombat(mstr, plyr, MartialCombat.GetMonsterAttackType(mstr));
                    }


                    //                   TextHelpers.GetMonsterHitLine(mstr.name, damage, monsterCrit, action.ToLower());
                    Console.ReadKey();

                    Program.currentPlayer.energy -= chosenSpell.SpellCost;
                    Program.currentPlayer.spellCasting += Checkers.GetSkillXPGain(mstr.level, chosenSpell.SpellCost, chosenSpell.RequiredLevel, "cast");
                    Program.currentPlayer.spellChanneling += Checkers.GetSkillXPGain(mstr.level, chosenSpell.SpellCost, chosenSpell.RequiredLevel, "channel");
                    Program.currentPlayer.magicMastery += Checkers.GetSkillXPGain(mstr.level, chosenSpell.SpellCost, chosenSpell.RequiredLevel, "magic");

                    mstr.health -= spellAttack;
                    if (mstr.health <= 0)
                    {
                        //Console.WriteLine($"{nm} was Slain!!");
                        mstr.IsAlive = false;
                    }

                }
                else if (action.ToLower() == "r" && Program.currentPlayer.race != Player.Race.Dwarf)
                {
                    // run
                    if (Program.currentPlayer.currentClass != Player.PlayerClass.Ranger && rand.Next(0, 2) == 0 &&
                        Program.currentPlayer.race != Player.Race.Halfling && Program.currentPlayer.race != Player.Race.Elf)
                    {
                        UIHelpers.Print($"As you sprint away from {mstr.name}, its strike catched you in the back, knocking you down.");
                        MartialCombat.DoMonsterSoloCombat(mstr, plyr, action.ToLower());
                        Console.ReadKey();
                        
                    }
                    else
                    {
                        UIHelpers.Print($"You use your crazy moves to evade the {mstr.name} and you successfully escape!");
                        Console.ReadKey();
                        Shop.loadShop(Program.currentPlayer);
                    };
                }
                else if (action.ToLower() == "r" && Program.currentPlayer.race == Player.Race.Dwarf)
                {
                    if (Program.currentPlayer.currentClass != Player.PlayerClass.Ranger && rand.Next(0, 3) == 0)
                    {
                        UIHelpers.Print($"As you sprint away from {mstr.name}, its strike catched you in the back, knocking you down.");
                        MartialCombat.DoMonsterSoloCombat(mstr, plyr, action.ToLower());
                        Console.ReadKey();
                        //                      Program.currentPlayer.health -= damage;
                    }
                    else
                    {
                        UIHelpers.Print($"You use your crazy moves to evade the {mstr.name} and you successfully escape!");
                        Console.ReadKey();
                        Shop.loadShop(Program.currentPlayer);
                    };
                }
                else if (action.ToLower() == "h")
                {
                    bool monsterCrit = false;
                    //Fix Monstercrit  // heal
                    PotionHealing(mstr, plyr, monsterCrit, action.ToLower(), "health");

                    if (mstr.IsAlive)
                    {
                        Console.ReadKey();
                    }

                }
                else if (action.ToLower() == "m")
                {
                    bool monsterCrit = false;
                    //Fix monstercrit
                    PotionHealing(mstr, plyr, monsterCrit, action.ToLower(), "mana");

                    if (mstr.IsAlive)
                    {
                        Console.ReadKey();
                    }
                }
                else if (action.ToLower() == "s")
                {
                    UIHelpers.DisplayStats(plyr);
                }

                CheckForDeath(mstr.name);

                if (mstr.IsAlive)
                    Console.ReadKey();
            }

            Console.WriteLine($"{mstr.name} was Slain!!");

            int xp = plyr.GetXP(mstr);
            int cn = plyr.GetCoins(mstr);
            UIHelpers.Print($"As you stand victorious over the {mstr.name}, its body dissolves into {cn} gold coins!");
            Console.ForegroundColor = ConsoleColor.Yellow;
            UIHelpers.Print($"You gain {xp} XP!");
            Console.WriteLine($"You quickly pocket {cn} coins and go down a long hall");
            Console.ResetColor();
            plyr.coins += cn;
            plyr.xp += xp;

            if (plyr.currentClass == Player.PlayerClass.Monk)
                plyr.energy = plyr.baseEnergy;

            if (plyr.CanLevelUp(plyr.level))
                plyr = plyr.LevelUp(plyr);

            Console.ReadKey();
        }

        public static int GetCurrentEncounterXPLevel(EncounterLevels encounterLvls)
        {
            int xpLevelFight = 0;
            var levelChosen = Randomizer.GetRandomNumber(21);

            switch (levelChosen)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    xpLevelFight = encounterLvls.Easy;
                    break;
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                    xpLevelFight = encounterLvls.Medium;
                    break;
                case 19:
                case 20:
                    xpLevelFight = encounterLvls.Hard;
                    break;
                default:
                    xpLevelFight = encounterLvls.Easy;
                    break;                    
            }            
            
            return xpLevelFight;

        }

        public static EncounterLevels GetXPThresholdsByLevel(int level)
        {
            EncounterLevels ecl = new EncounterLevels();

            var encounterLevls = new Dictionary<int, EncounterLevels>()
            {
                {1, new EncounterLevels { Easy = 25, Medium = 50, Hard = 75 } },
                {2, new EncounterLevels { Easy = 50, Medium = 75, Hard = 100} },
                {3, new EncounterLevels { Easy = 75, Medium = 100, Hard = 200} },
                {4, new EncounterLevels { Easy = 100, Medium = 200, Hard = 450} },
                {5, new EncounterLevels { Easy = 200, Medium = 450, Hard = 700} },
                {6, new EncounterLevels { Easy = 450, Medium = 700, Hard = 700} },
                {7, new EncounterLevels { Easy = 450, Medium = 700, Hard = 1100} },
                {8, new EncounterLevels { Easy = 450, Medium = 700, Hard = 1100} },
                {9, new EncounterLevels { Easy = 450, Medium = 1100, Hard = 1800} },
                {10, new EncounterLevels { Easy = 700, Medium = 1100, Hard = 1800} },
                {11, new EncounterLevels { Easy = 700, Medium = 1800, Hard = 2300} },
                {12, new EncounterLevels { Easy = 1100, Medium = 1800, Hard = 2900} },
                {13, new EncounterLevels { Easy = 1100, Medium = 2300, Hard = 2900} },
                {14, new EncounterLevels { Easy = 1100, Medium = 2300, Hard = 3900} },
                {15, new EncounterLevels { Easy = 1100, Medium = 2300, Hard = 3900} },
                {16, new EncounterLevels { Easy = 1800, Medium = 2900, Hard = 5000} },
                {17, new EncounterLevels { Easy = 1800, Medium = 3900, Hard = 5900} },
                {18, new EncounterLevels { Easy = 1800, Medium = 3900, Hard = 5900} },
                {19, new EncounterLevels { Easy = 2300, Medium = 5000, Hard = 7200} },
                {20, new EncounterLevels { Easy = 2900, Medium = 5900, Hard = 8400} }

            };

            foreach(var index in encounterLevls)
            {
                if (index.Key == level)
                {
                    ecl.Easy = index.Value.Easy;
                    ecl.Medium = index.Value.Medium;
                    ecl.Hard = index.Value.Hard;
                }
            }
            return ecl;
        }

        private static void PotionHealing(Monster m, Player p, bool monsterCrit, string action, string type)
        {
            if (Program.currentPlayer.potion == 0)
            {
                UIHelpers.Print("You feel around but find no potions!");
                MartialCombat.DoMonsterSoloCombat(m, p, action);
                p.armorMastery += Checkers.GetCombatSkillGain(m, p, Player.skillCombatType.armor.ToString());
            }
            else
            {
                // have potions
                int damage = m.level / 2 - (Program.currentPlayer.armorValue + Program.currentPlayer.damageResit);
                if (damage < 0)
                    damage = 0;

                if (type == "health")
                {
                    int heal = Program.currentPlayer.GetPotionHealValue("health");

                    if (heal > Program.currentPlayer.baseHealth - Program.currentPlayer.health)
                        heal = Program.currentPlayer.baseHealth - Program.currentPlayer.health;

                    UIHelpers.Print("You reach into your bag and pull out a pulsing red flask.  You take a drink from it");
                    if (heal != 0)
                        Console.WriteLine($"You feel better and gain {heal} health!");
                    else
                        Console.WriteLine($"You feel better but gain {heal} health.");

                    Program.currentPlayer.health += heal;

                    if (Program.currentPlayer.health > Program.currentPlayer.baseHealth)
                        Program.currentPlayer.health = Program.currentPlayer.baseHealth;

                    Program.currentPlayer.potion -= 1;

                    
                }
                else if (type == "mana")
                {
                    int mana = Program.currentPlayer.GetPotionHealValue("mana");

                    if (mana > Program.currentPlayer.baseEnergy - Program.currentPlayer.baseEnergy)
                        mana = Program.currentPlayer.baseEnergy - Program.currentPlayer.energy;

                    UIHelpers.Print("You reach into your bag and pull out a pulsing blue flask.  You take a drink from it.");
                    if (mana != 0)
                        Console.WriteLine($"You feel your power rise and gain {mana} {Program.currentPlayer.manaType}");
                    else
                        Console.WriteLine($"You feel your power attempt to rise but gain {mana} {Program.currentPlayer.manaType}");

                    Program.currentPlayer.energy += mana;

                    if (Program.currentPlayer.energy > Program.currentPlayer.baseEnergy)
                        Program.currentPlayer.energy = Program.currentPlayer.baseEnergy;

                    Program.currentPlayer.manaPotion -= 1;
                }


                if (rand.Next(0, 4) == 0)
                {
                    UIHelpers.Print($"As you were occupied {m.name} advanced and attacked you.");
                    Console.WriteLine($"You quickly step out of the way.");
                    p.evasion += Checkers.GetCombatSkillGain(m, p, Player.skillCombatType.evasion.ToString());
                }
                else
                {
                    UIHelpers.Print($"As you were occupied {m.name} advanced and attacked you.");
                    MartialCombat.DoMonsterSoloCombat(m, p, action);
                }

                
            }
        }

        public static void CheckForDeath(string nm)
        {
            if (Program.currentPlayer.health <= 0)
            {
                // Death code
                UIHelpers.Print($"As the {nm} delivers a massive blow you collapse to the floor.  Your conciousness fades, you have been slain!");
                Console.ReadKey();

                if (Program.currentPlayer.favors > 0)
                {
                    UIHelpers.Print("You are suddenly surrounded by a blinding light. You are lifted into the air and given");
                    UIHelpers.Print("a new lease on life!");
                    Program.currentPlayer.favors--;
                    Program.currentPlayer.health = Program.currentPlayer.baseHealth;
                    Program.currentPlayer.deaths += 1;
                }
                else
                    CheckPlayAgain();

            }
        }
        #endregion

        public static void CheckPlayAgain()
        {
            Console.Clear();
            UIHelpers.Print("Would you like to start a new game? Yes or No");
            string answer = Console.ReadLine().ToLower();

            bool tester = false;

            while (tester == false)
                if (answer == "yes" || answer == "y")
                {
                    System.Diagnostics.Process.Start(AppDomain.CurrentDomain.FriendlyName);
                    Environment.Exit(0);
                }
                else if (answer == "no" || answer == "n")
                {
                    UIHelpers.Print("Thank you for playing. Goodbye!");
                    Environment.Exit(0);
                }
                else
                    Environment.Exit(0);
        }
    }
}
