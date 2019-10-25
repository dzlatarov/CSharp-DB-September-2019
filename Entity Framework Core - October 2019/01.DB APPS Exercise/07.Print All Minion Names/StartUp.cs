namespace PrintAllMinionNames
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using InitialSetup;
    public class StartUp
    {
        public static void Main(string[] args)
        {
            List<string> minions = GetMinionNames();

            for (int i = 0; i < minions.Count / 2; i++)
            {
                Console.WriteLine(minions[i]);
                Console.WriteLine(minions[minions.Count - 1 - i]);

                if(minions.Count % 2 != 0)
                {
                    Console.WriteLine(minions[minions.Count / 2]);
                }
            }
        }

        private static List<string> GetMinionNames()
        {
            List<string> names = new List<string>();

            using(var connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();

                string getMinionNamesQuery = @"SELECT Name FROM Minions";

                using(var command = new SqlCommand(getMinionNamesQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            names.Add((string)reader[0]);
                        }
                    }
                }
            }

            return names;
        }
    }
}
