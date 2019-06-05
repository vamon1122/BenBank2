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
        internal double _balance;
        public double Balance { get { return _balance; } }
        public Government MyGovernment { get; set; }

        public void Pay(FinancialEntity recipient, double ammount)
        {
            Transaction myTransaction = new Transaction()
            {
                Sender = this,
                Recipient = recipient,
                Ammount = ammount
            };

            try
            {
                myTransaction.Execute();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        internal abstract void TakeFunds(double ammount);
        internal abstract void RecieveFunds(double ammount);
    }

    public class Government : FinancialEntity
    {
        public Government() { }

        public Government(SqlDataReader reader)
        {
            Id = (Guid)reader[0];
            Name = reader[1].ToString().Trim();
            _balance = Convert.ToDouble(reader[2]);
            VAT = Convert.ToDouble(reader[3]);
            IncomeTax = Convert.ToDouble(reader[4]);
            MyGovernment = this;
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

                    var takeFunds = new SqlCommand("UPDATE tb_government SET balance = @balance WHERE government_id = @government_id", conn);
                    takeFunds.Parameters.Add(new SqlParameter("government_id", Id));
                    takeFunds.Parameters.Add(new SqlParameter("balance", Balance - ammount));

                    takeFunds.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            _balance -= ammount;
        }
        internal override void RecieveFunds(double ammount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var recieveFunds = new SqlCommand("UPDATE tb_government SET balance = @balance WHERE government_id = @government_id", conn);
                    recieveFunds.Parameters.Add(new SqlParameter("government_id", Id));
                    recieveFunds.Parameters.Add(new SqlParameter("balance", Balance + ammount));

                    recieveFunds.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            _balance += ammount;
        }
    }

    public class Person : FinancialEntity
    {
        public Person(SqlDataReader reader)
        {
            Id = (Guid)reader[0];
            foreach(Government g in DataStore.Governments)
            {
                Debug.WriteLine(string.Format("Id = {0} name = {1}", g.Id, g.Name));
            }
            MyGovernment = DataStore.Governments.First(x => x.Id == (Guid)reader[1]);
            Forename = reader[2].ToString().Trim();
            Surname = reader[3].ToString().Trim();
            _balance = Convert.ToDouble(reader[4]);
            Debug.WriteLine(string.Format("{0} was loaded. Their Id = {1}. They live under {2}. Their balance is {3}", Name, Id, MyGovernment.Name, Balance.ToString("£0.00")));
        }

        public override string Name {
            get
            {
                if (string.IsNullOrWhiteSpace(Surname))
                {
                    return Forename;
                }
                else
                {
                    return string.Format("{0} {1}", Forename, Surname);
                }
            }
            set { }
        }
        public string Forename { get; set; }
        public string Surname { get; set; }

        internal override void TakeFunds(double ammount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var takeFunds = new SqlCommand("UPDATE tb_person SET balance = @balance WHERE person_id = @person_id", conn);
                    takeFunds.Parameters.Add(new SqlParameter("person_id", Id));
                    takeFunds.Parameters.Add(new SqlParameter("balance", Balance - ammount));

                    takeFunds.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            _balance -= ammount;
        }
        internal override void RecieveFunds(double ammount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var recieveFunds = new SqlCommand("UPDATE tb_person SET balance = @balance WHERE person_id = @person_id", conn);
                    recieveFunds.Parameters.Add(new SqlParameter("person_id", Id));
                    recieveFunds.Parameters.Add(new SqlParameter("balance", Balance + ammount));

                    recieveFunds.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            _balance += ammount;
        }
    }

    public class Business : FinancialEntity
    {
        //This is for derived classes like Bank
        internal Business()
        {

        }
        public Business(SqlDataReader reader)
        {
            Id = (Guid)reader[0];
            BusinessOwner = DataStore.FinancialEntities.Find(x => x.Id.ToString() == reader[1].ToString());
            MyGovernment = BusinessOwner.MyGovernment;
            Name = reader[2].ToString().Trim();
            _balance = Convert.ToDouble(reader[3]);
            Debug.WriteLine(string.Format("{0} business was loaded. It's Id = {1}. It operates from {2} It's balance is {3}", Name, Id, MyGovernment.Name, Balance.ToString("£0.00")));
        }

        public override string Name { get; set; }
        public FinancialEntity BusinessOwner { get; set; }

        internal override void TakeFunds(double ammount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var takeFunds = new SqlCommand("UPDATE tb_business SET balance = @balance WHERE business_id = @business_id", conn);
                    takeFunds.Parameters.Add(new SqlParameter("business_id", Id));
                    takeFunds.Parameters.Add(new SqlParameter("balance", Balance - ammount));

                    takeFunds.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            _balance -= ammount;
        }
        internal override void RecieveFunds(double ammount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var recieveFunds = new SqlCommand("UPDATE tb_business SET balance = @balance WHERE business_id = @business_id", conn);
                    recieveFunds.Parameters.Add(new SqlParameter("business_id", Id));
                    recieveFunds.Parameters.Add(new SqlParameter("balance", Balance + ammount));

                    recieveFunds.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            _balance += ammount;
        }
    }

    public class Bank : Business
    {
        public Bank(SqlDataReader reader)
        {
            Id = (Guid)reader[0];
            BankOwner = DataStore.People.Find(x => x.Id.ToString() == reader[1].ToString());
            MyGovernment = BankOwner.MyGovernment;
            Name = reader[2].ToString().Trim();
            _balance = Convert.ToDouble(reader[3]);
            PositiveInterest = Convert.ToDouble(reader[4]);
            NegativeInterest = Convert.ToDouble(reader[5]);
            Debug.WriteLine(string.Format("{0} bank business was loaded. It's Id = {1}. It's government's Id = {2}. It's balance is {3}. It's positive interest rate is = {4}%. It's negative interest rate = {5}%", Name, Id, MyGovernment.Id, Balance.ToString("£0.00"), PositiveInterest, NegativeInterest));
        }

        public Person BankOwner { get; set; }
        public double PositiveInterest { get; set; }
        public double NegativeInterest { get; set; }
    }

    public class BankAccount : FinancialEntity
    {
        public BankAccount(SqlDataReader reader)
        {
            Id = (Guid)reader[0];
            AccountBank = DataStore.Banks.First(x => x.Id == (Guid)reader[1]);
            AccountHolder = DataStore.FinancialEntities.First(x => x.Id == (Guid)reader[2]);
            MyGovernment = AccountHolder.MyGovernment;
            _balance = Convert.ToDouble(reader[3]);
            Debug.WriteLine(string.Format("bank account was loaded. It's Id = {0}. It's bank is = {1}. It's account holder = {2}. It's balance is {3}.",  Id, AccountBank.Name, AccountHolder.Name, Balance.ToString("£0.00")));
        }

        public override string Name { get { return string.Format("{0}'s bank account", AccountHolder.Name); } set { } }
        public Bank AccountBank { get; set; }
        public FinancialEntity AccountHolder { get; }

        internal override void TakeFunds(double ammount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var takeFunds = new SqlCommand("UPDATE tb_bank_account SET balance = @balance WHERE bank_account_id = @bank_account_id", conn);
                    takeFunds.Parameters.Add(new SqlParameter("bank_account_id", Id));
                    takeFunds.Parameters.Add(new SqlParameter("balance", Balance - ammount));

                    takeFunds.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            _balance -= ammount;
            AccountBank.TakeFunds(ammount);
        }
        internal override void RecieveFunds(double ammount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataStore.ConnectionString))
                {
                    conn.Open();

                    var recieveFunds = new SqlCommand("UPDATE tb_bank_account SET balance = @balance WHERE bank_account_id = @bank_account_id", conn);
                    recieveFunds.Parameters.Add(new SqlParameter("bank_account_id", Id));
                    recieveFunds.Parameters.Add(new SqlParameter("balance", Balance + ammount));

                    recieveFunds.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            _balance += ammount;
            AccountBank.RecieveFunds(ammount);
        }
    }
}
