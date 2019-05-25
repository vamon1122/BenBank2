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

        public void DoTransaction()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var InsertTransaction = new SqlCommand("INSERT INTO tb_transaction VALUES (@transaction_id, @sender_id, @recipient_id, @ammount)", conn);
                    InsertTransaction.Parameters.Add(new SqlParameter("transaction_id", Id));
                    InsertTransaction.Parameters.Add(new SqlParameter("sender_id", Sender.Id));
                    InsertTransaction.Parameters.Add(new SqlParameter("recipient_id", Recipient.Id));
                    InsertTransaction.Parameters.Add(new SqlParameter("ammount", Ammount));

                    InsertTransaction.ExecuteNonQuery();
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
    }
}
