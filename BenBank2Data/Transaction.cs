using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenBank2Data
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public FinancialEntity Sender { get; set; }
        public FinancialEntity Recipient { get; set; }
        public double Ammount { get; set; }

        public void DoTransaction()
        {
            try
            {
                Sender.SendFunds(Ammount);

            }
            catch
            {

            }

            try
            {
                Recipient.RecieveFunds(Ammount);
            }
            catch
            {
                try
                {
                    Sender.RecieveFunds(Ammount);
                }
                catch
                {

                }
            }

        }
    }
}
