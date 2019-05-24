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
        public static readonly string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ben\source\repos\BenBank2\BenBank2Data\BenBank2Db.mdf;Integrated Security=True";
        public static void Everything()
        {

        }

        static DataStore()
        {
            People = new List<Person>();
        }

        public static List<Person> People;

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
    }
}
