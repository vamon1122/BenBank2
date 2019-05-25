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
        public static void PersonPayBusiness()
        {
            Person Person1 = DataStore.People.First();
            Business Business1 = DataStore.Businesses.First();
            Console.WriteLine(string.Format("Person1 is {0}. They have {1}", Person1.Name, Person1.Balance.ToString("£0.00")));
            Console.WriteLine(string.Format("Business1 is {0}. They have {1}", Business1.Name, Business1.Balance.ToString("£0.00")));

            GetInput();

            void GetInput()
            {
                CC.Cyan.WriteLine("How much will Person1 pay Business1?");
                CC.Cyan.Write("Person1 will pay: ");
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
                    GetInput();
                }

                Person1.Pay(Business1, ammount);
            }
            

        }
    }
}
