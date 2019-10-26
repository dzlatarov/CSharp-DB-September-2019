namespace IncreaseAgeStoredProcedure
{
    using System;
    using InitialSetup;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Collections.Generic;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            int inputId = int.Parse(Console.ReadLine());

            using(SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();

                UpgradeAgeOfMinion(connection, inputId);
                PrintNameAndAgeOfMinion(connection, inputId);
            }
        }

        private static void PrintNameAndAgeOfMinion(SqlConnection connection, int inputId)
        {
            string printNameAndAgeQuery = @"SELECT Name, Age FROM Minions WHERE Id = @Id";

            using(SqlCommand command = new SqlCommand(printNameAndAgeQuery, connection))
            {
                command.Parameters.AddWithValue("@Id", inputId);

                using(SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        string minionName = (string)reader["Name"];
                        int age = (int)reader["Age"];

                        Console.WriteLine($"{minionName} - {age} years old");
                    }
                }
            }
        }

        private static void UpgradeAgeOfMinion(SqlConnection connection, int inputId)
        {
            string updateAgeQuery = @"EXEC usp_GetOlder @id";

            using(SqlCommand command = new SqlCommand(updateAgeQuery, connection))
            {
                command.Parameters.AddWithValue("@id", inputId);
                command.ExecuteNonQuery();
            }
        }
    }
}
