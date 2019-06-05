using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BenBank2Data
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public FinancialEntity Sender { get; set; }
        public FinancialEntity Recipient { get; set; }
        public double Ammount { get; set; }

        public Transaction()
        {
            Id = Guid.NewGuid();
        }

        public void Execute()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var insertTransaction = new SqlCommand("INSERT INTO tb_transaction VALUES (@transaction_id, @sender_id, @recipient_id, @ammount)", conn);
                    insertTransaction.Parameters.Add(new SqlParameter("transaction_id", Id));
                    insertTransaction.Parameters.Add(new SqlParameter("sender_id", Sender.Id));
                    insertTransaction.Parameters.Add(new SqlParameter("recipient_id", Recipient.Id));
                    insertTransaction.Parameters.Add(new SqlParameter("ammount", Ammount));

                    insertTransaction.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            try
            {
                Sender.TakeFunds(Ammount);
            }
            catch(Exception ex)
            {
                throw ex;
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
                catch(Exception ex)
                {
                    throw ex;
                }
            }

        }

        public void ExecuteWithVAT()
        {
            double ammountAfterVAT = (double)decimal.Round((decimal)(Ammount / (1 + (Sender.MyGovernment.VAT / 100))), 2, MidpointRounding.AwayFromZero);
            double taxDue = Ammount - ammountAfterVAT;

            Transaction PayPayee = new Transaction();
            PayPayee.Sender = Sender;
            PayPayee.Recipient = Recipient;
            PayPayee.Ammount = ammountAfterVAT;
            PayPayee.Execute();

            Transaction PayVAT = new Transaction();
            PayVAT.Sender = Sender;
            PayVAT.Recipient = Sender.MyGovernment;
            PayVAT.Ammount = taxDue;
            PayVAT.Execute();
        }

        public void ExecuteWithIncomeTax()
        {
            double ammountAfterTax = (double)decimal.Round((decimal)(Ammount / (1 + (Recipient.MyGovernment.IncomeTax / 100))), 2, MidpointRounding.AwayFromZero);
            double taxDue = Ammount - ammountAfterTax;

            Transaction PayPayee = new Transaction();
            PayPayee.Sender = Sender;
            PayPayee.Recipient = Recipient;
            PayPayee.Ammount = ammountAfterTax;
            PayPayee.Execute();

            Transaction PayIncomeTax = new Transaction();
            PayIncomeTax.Sender = Sender;
            PayIncomeTax.Recipient = Recipient.MyGovernment;
            PayIncomeTax.Ammount = taxDue;
            PayIncomeTax.Execute();
        }
    }
}
