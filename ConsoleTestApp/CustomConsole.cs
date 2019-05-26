using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestApp
{
    public static class CC
    {
        public static class Info
        {
            public static void WriteLine(string value)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(value);
                Console.ResetColor();
            }

            public static void Write(string value)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(value);
                Console.ResetColor();
            }
        }

        public static class Cyan
        {
            public static void WriteLine(string value)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(value);
                Console.ResetColor();
            }

            public static void Write(string value)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(value);
                Console.ResetColor();
            }
        }

        public static void Title(string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("///////////"); //10
            foreach(char character in value)
            {
                Console.Write("/");
            }
            Console.WriteLine("///////////"); //10

            Console.Write("////////// "); //10
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write(value);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(" //////////"); //10
            Console.Write("///////////"); //10
            foreach (char character in value)
            {
                Console.Write("/");
            }
            Console.WriteLine("///////////"); //10
            Console.ResetColor();
        }
        
    }
}
