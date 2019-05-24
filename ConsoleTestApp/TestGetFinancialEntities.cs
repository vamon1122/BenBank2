using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenBank2Data;

namespace ConsoleTestApp
{
    public static class TestGetFinancialEntities
    {
        public static void GetFinancialEntities()
        {
            foreach (FinancialEntity financialEntity in DataStore.FinancialEntities)
            {
                Console.WriteLine(string.Format("FinancialEntity \"{0}\" was found. It's Id = {1}. It's balance = {2}", financialEntity.Name, financialEntity.Id, financialEntity.Balance.ToString("£0.00")));
            }
            Console.WriteLine("Test complete!");
        }
    }
}
