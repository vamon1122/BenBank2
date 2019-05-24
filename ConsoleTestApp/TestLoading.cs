using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenBank2Data;


namespace ConsoleTestApp
{
    public static class TestLoading
    {
        public static void Load()
        {
            Console.WriteLine("Testing loading...");
            DateTime start = DateTime.Now;
            DataStore.LoadGovernments();
            DataStore.LoadPeople();
            DataStore.LoadBusinesses();
            DataStore.LoadBanks();
            DataStore.LoadBankAccounts();
            Console.WriteLine(string.Format("Loaded in {0} second(s)", ((double)(DateTime.Now - start).Milliseconds / 1000).ToString("0.000")));
        }
    }
}
