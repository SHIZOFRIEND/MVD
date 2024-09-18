using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckConnection
{
    public class Connection
    {
        public static void Main(string[] args)
        {
            string connectionString = @"Data Source=DESKTOP-OIV0VNA\SQLEXPRESS;Initial Catalog=MVDBD;Integrated Security=True;";

            if (CheckConnection(connectionString))
            {
                Console.WriteLine("Соединение с базой данных успешно.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Ошибка при соединении с базой данных.");
            }
        }
        public static bool CheckConnection(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Ошибка при соединении с базой данных: {ex.Message}");
                    return false;
                }
            }
        }
    }
}