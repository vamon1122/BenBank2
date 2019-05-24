using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenBank2Data;

namespace ConsoleTestApp
{
    public static class TestLoadPeople
    {
        public static void LoadPeople()
        {
            Console.WriteLine("Testing load people...");
            DataStore.LoadPeople();
            Console.WriteLine("Test complete!");
        }
    }
}
