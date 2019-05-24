using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BenBank2Data
{
    public static class DataStore
    {
        public static readonly string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\benba\Documents\GitHub\BenBank2\BenBank2Data\BenBank2Db.mdf;Integrated Security=True";
        public static void Everything()
        {

        }

        static DataStore()
        {
            Banks = new List<Bank>();
            Businesses = new List<Business>();
            Governments = new List<Government>();
            People = new List<Person>();
            Transactions = new List<Transaction>();
            BankAccounts = new List<BankAccount>();
        }

        public static List<Bank> Banks;
        public static List<Business> Businesses;
        public static List<Government> Governments;
        public static List<Person> People;
        public static List<Transaction> Transactions;
        public static List<BankAccount> BankAccounts;
        public static List<FinancialEntity> FinancialEntities { get{ 
            var financialEntities = new List<FinancialEntity>();
                financialEntities.AddRange(Banks);
                financialEntities.AddRange(Businesses);
                financialEntities.AddRange(Governments);
                financialEntities.AddRange(People);
                financialEntities.AddRange(BankAccounts);

                return financialEntities;
            } }


        public static void LoadGovernments()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                Debug.WriteLine("Connection opened");

                SqlCommand SelectGovernments = new SqlCommand("SELECT * FROM tb_government", conn);

                using (SqlDataReader reader = SelectGovernments.ExecuteReader())
                {
                    Debug.WriteLine("Reader executed");
                    while (reader.Read())
                    {
                        Debug.WriteLine((Guid)reader[0]);
                        Debug.WriteLine(reader[1].ToString().Trim());
                        Governments.Add(new Government(reader));
                    }
                }
            }
        }

        public static void LoadPeople()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                Debug.WriteLine("Connection opened");

                SqlCommand SelectPeople = new SqlCommand("SELECT * FROM tb_person", conn);

                using (SqlDataReader reader = SelectPeople.ExecuteReader())
                {
                    Debug.WriteLine("Reader executed");
                    while (reader.Read())
                    {
                        People.Add(new Person(reader));
                    }
                }
            }
        }

        public static void LoadBusinesses()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                Debug.WriteLine("Connection opened");

                SqlCommand SelectBusinesses = new SqlCommand("SELECT tb_business.* FROM tb_business INNER JOIN tb_bank ON tb_business.business_id <> tb_bank.business_id", conn);

                using (SqlDataReader reader = SelectBusinesses.ExecuteReader())
                {
                    Debug.WriteLine("Reader executed");
                    while (reader.Read())
                    {
                        Businesses.Add(new Business(reader));
                    }
                }
            }
        }

        public static void LoadBanks()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                Debug.WriteLine("Connection opened");

                SqlCommand SelectBusinesses = new SqlCommand("SELECT tb_business.*, tb_bank.positive_interest, tb_bank.negative_interest FROM tb_business INNER JOIN tb_bank ON tb_business.business_id = tb_bank.business_id", conn);

                using (SqlDataReader reader = SelectBusinesses.ExecuteReader())
                {
                    Debug.WriteLine("Reader executed");
                    while (reader.Read())
                    {
                        Banks.Add(new Bank(reader));
                    }
                }
            }
        }

        public static void LoadBankAccounts()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                Debug.WriteLine("Connection opened");

                SqlCommand SelectBankAccounts = new SqlCommand("SELECT * FROM tb_bank_account", conn);

                using (SqlDataReader reader = SelectBankAccounts.ExecuteReader())
                {
                    Debug.WriteLine("Reader executed");
                    while (reader.Read())
                    {
                        BankAccounts.Add(new BankAccount(reader));
                    }
                }
            }
        }
    }
}
