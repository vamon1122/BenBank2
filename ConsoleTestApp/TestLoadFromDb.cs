using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenBank2Data;


namespace ConsoleTestApp
{
    public static class TestLoadFromDb
    {
        public static void Load()
        {
            CC.Title("Load all data from the database");
            CC.Info.WriteLine("Loading entities from the database...");
            DateTime start = DateTime.Now;
            DataStore.LoadGovernments();
            DataStore.LoadPeople();
            DataStore.LoadBusinesses();
            DataStore.LoadBanks();
            DataStore.LoadBankAccounts();
            CC.Info.WriteLine(string.Format("Loaded in {0} second(s)", ((double)(DateTime.Now - start).Milliseconds / 1000).ToString("0.000")));
        }
    }
}
