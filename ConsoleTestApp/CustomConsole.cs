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
        
    }
}
