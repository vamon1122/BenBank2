using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Data.SqlTypes;

namespace BenBank2Data
{
    abstract public class FinancialEntity
    {
        public Guid Id { get; set; }
        public abstract string Name { get; set; }
        public double Balance { get; set; }

        public void Pay(FinancialEntity pRecipient, double pAmmount)
        {
            Transaction MyTransaction = new Transaction()
            {
                Sender = this,
                Recipient = pRecipient,
                Ammount = pAmmount
            };

            try
            {
                MyTransaction.DoTransaction();
            }
            catch
            {

            }
           
        }

        internal abstract void SendFunds(double ammount);
        internal abstract void RecieveFunds(double ammount);
    }

    public class Person : FinancialEntity
    {
        public Person(SqlDataReader pReader)
        {
            Id = (Guid)pReader[0];
            GovernmentId = (Guid)pReader[1];
            Forename = pReader[2].ToString().Trim();
            Surname = pReader[3].ToString().Trim();
            Balance = Convert.ToDouble(pReader[4]);
            Debug.WriteLine(string.Format("{0} was loaded. Their Id = {1}. Their balance is {2}", Name, Id, Balance));
        }

        public Guid GovernmentId { get; set; }
        public override string Name { get { return string.Format("{0} {1}", Forename, Surname); } set { } }
        public string Forename { get; set; }
        public string Surname { get; set; }

        internal override void SendFunds(double ammount)
        {
            throw new NotImplementedException();
        }

        internal override void RecieveFunds(double ammount)
        {
            throw new NotImplementedException();
        }
    }

    public class Business : FinancialEntity
    {
        public Business(SqlDataReader pReader)
        {
            Id = (Guid)pReader[0];
            GovernmentId = (Guid)pReader[1];
            Name = pReader[2].ToString().Trim();
            Balance = Convert.ToDouble(pReader[3]);
            Debug.WriteLine(string.Format("{0} business was loaded. It's Id = {1}. It's balance is {2}", Name, Id, Balance));
        }

        public Guid GovernmentId { get; set; }
        public override string Name { get; set; }

        internal override void SendFunds(double ammount)
        {
            throw new NotImplementedException();
        }

        internal override void RecieveFunds(double ammount)
        {
            throw new NotImplementedException();
        }
    }

    public class Government : FinancialEntity
    {
        public Government(SqlDataReader pReader)
        {
            Id = (Guid)pReader[0];
            Name = pReader[1].ToString().Trim();
            Balance = Convert.ToDouble(pReader[2]);
            VAT = Convert.ToDouble(pReader[3]);
            IncomeTax = Convert.ToDouble(pReader[4]);
            Debug.WriteLine(string.Format("{0} government was loaded. It's Id = {1}. It's balance is {2}", Name, Id, Balance));
        }

        public override string Name { get; set; }
        public double VAT { get; set; }
        public double IncomeTax { get; set; }

        internal override void SendFunds(double ammount)
        {
            throw new NotImplementedException();
        }

        internal override void RecieveFunds(double ammount)
        {
            throw new NotImplementedException();
        }
    }

    public class BankAccount : FinancialEntity
    {
        public override string Name { get; set; }

        internal override void SendFunds(double ammount)
        {
            throw new NotImplementedException();
        }

        internal override void RecieveFunds(double ammount)
        {
            throw new NotImplementedException();
        }

    }

    
}
