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
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        internal abstract void TakeFunds(double ammount);
        internal abstract void RecieveFunds(double ammount);
    }

    public class Person : FinancialEntity
    {
        public Person(SqlDataReader pReader)
        {
            Id = (Guid)pReader[0];
            Debug.WriteLine(pReader[0]);
            Debug.WriteLine(pReader[1]);
            foreach(Government g in DataStore.Governments)
            {
                Debug.WriteLine(string.Format("Id = {0} name = {1}", g.Id, g.Name));
            }
            PersonGovernment = DataStore.Governments.First(x => x.Id == (Guid)pReader[1]);
            Forename = pReader[2].ToString().Trim();
            Surname = pReader[3].ToString().Trim();
            Balance = Convert.ToDouble(pReader[4]);
            Debug.WriteLine(string.Format("{0} was loaded. Their Id = {1}. They live under {2}. Their balance is {3}", Name, Id, PersonGovernment.Name, Balance.ToString("£0.00")));
        }

        public Government PersonGovernment { get; set; }
        public override string Name { get { return string.Format("{0} {1}", Forename, Surname); } set { } }
        public string Forename { get; set; }
        public string Surname { get; set; }

        

        internal override void TakeFunds(double pAmmount)
        {
            
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var SendFunds = new SqlCommand("UPDATE tb_person SET balance = @balance WHERE person_id = @person_id", conn);
                    SendFunds.Parameters.Add(new SqlParameter("person_id", Id));
                    SendFunds.Parameters.Add(new SqlParameter("balance", Balance - pAmmount));

                    SendFunds.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            Balance -= pAmmount;
        }

        internal override void RecieveFunds(double ammount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var RecieveFunds = new SqlCommand("UPDATE tb_person SET balance = @balance WHERE person_id = @person_id", conn);
                    RecieveFunds.Parameters.Add(new SqlParameter("person_id", Id));
                    RecieveFunds.Parameters.Add(new SqlParameter("balance", Balance + ammount));

                    RecieveFunds.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Balance += ammount;
        }
    }

    public class Business : FinancialEntity
    {
        //This is for derived classes
        internal Business()
        {

        }

        public Business(SqlDataReader pReader)
        {
            Id = (Guid)pReader[0];
            BusinessGovernment = DataStore.Governments.First(x => x.Id == (Guid)pReader[1]);
            Name = pReader[2].ToString().Trim();
            Balance = Convert.ToDouble(pReader[3]);
            Debug.WriteLine(string.Format("{0} business was loaded. It's Id = {1}. It operates from {2} It's balance is {3}", Name, Id, BusinessGovernment.Name, Balance.ToString("£0.00")));
        }

        public Government BusinessGovernment { get; set; }

        public override string Name { get; set; }

        internal override void TakeFunds(double ammount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var SendFunds = new SqlCommand("UPDATE tb_business SET balance = @balance WHERE business_id = @business_id", conn);
                    SendFunds.Parameters.Add(new SqlParameter("business_id", Id));
                    SendFunds.Parameters.Add(new SqlParameter("balance", Balance - ammount));

                    SendFunds.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Balance -= ammount;
        }

        internal override void RecieveFunds(double ammount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var RecieveFunds = new SqlCommand("UPDATE tb_business SET balance = @balance WHERE business_id = @business_id", conn);
                    RecieveFunds.Parameters.Add(new SqlParameter("business_id", Id));
                    RecieveFunds.Parameters.Add(new SqlParameter("balance", Balance + ammount));

                    RecieveFunds.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Balance += ammount;
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
            Debug.WriteLine(string.Format("{0} government was loaded. It's Id = {1}. It's balance is {2}. It's VAT is = {3}%. It's Income Tax = {4}%", Name, Id, Balance.ToString("£0.00"), VAT, IncomeTax));
        }

        public override string Name { get; set; }
        public double VAT { get; set; }
        public double IncomeTax { get; set; }

        internal override void TakeFunds(double ammount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var SendFunds = new SqlCommand("UPDATE tb_government SET balance = @balance WHERE government_id = @government_id", conn);
                    SendFunds.Parameters.Add(new SqlParameter("government_id", Id));
                    SendFunds.Parameters.Add(new SqlParameter("balance", Balance - ammount));

                    SendFunds.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Balance -= ammount;
        }

        internal override void RecieveFunds(double ammount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var RecieveFunds = new SqlCommand("UPDATE tb_government SET balance = @balance WHERE government_id = @government_id", conn);
                    RecieveFunds.Parameters.Add(new SqlParameter("government_id", Id));
                    RecieveFunds.Parameters.Add(new SqlParameter("balance", Balance + ammount));

                    RecieveFunds.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Balance += ammount;
        }
    }

    public class BankAccount : FinancialEntity
    {
        public BankAccount(SqlDataReader pReader)
        {
            Id = (Guid)pReader[0];
            AccountBank = DataStore.Banks.First(x => x.Id == (Guid)pReader[1]);
            AccountHolder = DataStore.FinancialEntities.First(x => x.Id == (Guid)pReader[2]);
            Balance = Convert.ToDouble(pReader[3]);
            Debug.WriteLine(string.Format("bank account was loaded. It's Id = {0}. It's bank is = {1}. It's account holder = {2}. It's balance is {3}.",  Id, AccountBank.Name, AccountHolder.Name, Balance.ToString("£0.00")));
        }

        public Bank AccountBank { get; set; }

        public FinancialEntity AccountHolder { get; }

        public override string Name { get { return string.Format("{0}'s bank account", AccountHolder.Name); } set { } }

        internal override void TakeFunds(double ammount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var SendFunds = new SqlCommand("UPDATE tb_bank_account SET balance = @balance WHERE bank_account_id = @bank_account_id", conn);
                    SendFunds.Parameters.Add(new SqlParameter("bank_account_id", Id));
                    SendFunds.Parameters.Add(new SqlParameter("balance", Balance - ammount));

                    SendFunds.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Balance -= ammount;
        }

        internal override void RecieveFunds(double ammount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var RecieveFunds = new SqlCommand("UPDATE tb_bank_account SET balance = @balance WHERE bank_account_id = @bank_account_id", conn);
                    RecieveFunds.Parameters.Add(new SqlParameter("bank_account_id", Id));
                    RecieveFunds.Parameters.Add(new SqlParameter("balance", Balance + ammount));

                    RecieveFunds.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Balance += ammount;
        }

    }

    public class Bank : Business
    {

        public Bank(SqlDataReader pReader) 
        {
            Id = (Guid)pReader[0];
            BusinessGovernment = DataStore.Governments.First(x => x.Id == (Guid)pReader[1]);
            Name = pReader[2].ToString().Trim();
            Balance = Convert.ToDouble(pReader[3]);
            PositiveInterest = Convert.ToDouble(pReader[4]);
            NegativeInterest = Convert.ToDouble(pReader[5]);
            Debug.WriteLine(string.Format("{0} bank business was loaded. It's Id = {1}. It's government's Id = {2}. It's balance is {3}. It's positive interest rate is = {4}%. It's negative interest rate = {5}%", Name, Id, BusinessGovernment.Id, Balance.ToString("£0.00"), PositiveInterest, NegativeInterest));
        }

        public double PositiveInterest { get; set; }
        public double NegativeInterest { get; set; }
    }
}
