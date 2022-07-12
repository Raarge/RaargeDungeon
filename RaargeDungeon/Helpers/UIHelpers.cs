using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaargeDungeon.Helpers
{
    public static class UIHelpers
    {
        #region ExperianceBar
        public static void ProgressBar(string fillerChar, string backgroundChar, decimal value, int size)
        {
            int diff = (int)(value * size);

            for (int i = 0; i < size; i++)
            {
                if (i < diff)
                    Console.Write(fillerChar);
                else
                    Console.Write(backgroundChar);
            }
        }
        #endregion
        public static void GenerateStatusBar(string barLabel, string statusMarker, string emptyMarker, ConsoleColor consoleColor, int stat, int baseStat)
        {
            Console.WriteLine($" {barLabel} Bar: ");
            Console.Write(" [");
            Console.ForegroundColor = consoleColor;
            ProgressBar($"{statusMarker}", $"{emptyMarker}", stat / (decimal)baseStat, 25);
            Console.ResetColor();
            Console.WriteLine("]");
        }

        #region SpecialPrint
        public static void Print(string text, int speed = 10)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(speed);
            }
            Console.WriteLine("");
        }
        #endregion
    }
}
