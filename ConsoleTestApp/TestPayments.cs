using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenBank2Data;

namespace ConsoleTestApp
{
    public static class TestPayments
    {
        public static void DoTestPayments()
        {
            Person Person1 = DataStore.People.First();
            Business Business1 = DataStore.Businesses.First();
            Government Government1 = DataStore.Governments.First();
            Bank Bank1 = DataStore.Banks.First();
            BankAccount BankAccount1 = DataStore.BankAccounts.First();

            CC.Title("Make payments from each type of financial entity");
            DoTest(Person1, "Person1", Business1, "Business1");
            DoTest(Business1, "Business1", Government1, "Government1");
            DoTest(Government1, "Government1", Bank1, "Bank1");
            DoTest(Bank1, "Bank1", BankAccount1, "BankAccount1");
            DoTest(BankAccount1, "BankAccount1", Person1, "Person1");

            void DoTest(FinancialEntity financialEntity1, string financialEntity1Name, FinancialEntity financialEntity2, string financialEntity2Name)
            {
                Console.WriteLine(string.Format("{0} has {1}. {2} has {3}.", financialEntity1Name, financialEntity1.Balance.ToString("£0.00"), financialEntity2Name, financialEntity2.Balance.ToString("£0.00")));
                CC.Cyan.WriteLine(string.Format("How much will {0} pay {1}?", financialEntity1Name, financialEntity2Name));
                CC.Cyan.Write(string.Format("{0} will pay: ", financialEntity1Name));
                Console.Write("£");
                var input = Console.ReadLine();
                double ammount = 0;
                try
                {
                    ammount = Convert.ToDouble(input);
                }
                catch
                {
                    Console.WriteLine(string.Format("\"{0}\" is not a valid ammount. Please try again.", input));
                    DoTest(financialEntity1, financialEntity1Name, financialEntity2, financialEntity2Name);
                }

                financialEntity1.Pay(financialEntity2, ammount);

                Console.WriteLine(string.Format("{0} now has {1}. {2} now has {3}.", financialEntity1Name, financialEntity1.Balance.ToString("£0.00"), financialEntity2Name, financialEntity2.Balance.ToString("£0.00")));
            }
        }
    }
}
