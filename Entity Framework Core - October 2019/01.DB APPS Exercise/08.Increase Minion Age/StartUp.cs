
namespace IncreaseMinionAge
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
            int[] inputIds = Console.ReadLine().Split().Select(int.Parse).ToArray();

            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();

                foreach (var id in inputIds)
                {
                    UpdateNamesAndAges(connection, id);
                }
                PrintMinionsAndTheirAge(connection);
            }
        }

        private static void PrintMinionsAndTheirAge(SqlConnection connection)
        {
            string getAllMinionsNameAndAgeQuery = @"SELECT Name, Age FROM Minions";

            using (SqlCommand command = new SqlCommand(getAllMinionsNameAndAgeQuery, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string minionName = (string)reader["Name"];
                        int minionAge = (int)reader["Age"];

                        Console.WriteLine($"{minionName} {minionAge}");
                    }
                }
            }
        }

        private static void UpdateNamesAndAges(SqlConnection connection, int id)
        {
            string updateNamesAndAges = @"UPDATE Minions
                                            SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                            WHERE Id = @Id";
            using (SqlCommand command = new SqlCommand(updateNamesAndAges, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
