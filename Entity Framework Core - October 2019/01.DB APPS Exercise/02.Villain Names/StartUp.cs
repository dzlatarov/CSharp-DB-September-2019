namespace VillainNames
{
    using InitialSetup;
    using System;
    using System.Data.SqlClient;
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();

                var query = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                FROM Villains AS v
                                JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                                GROUP BY v.Id, v.Name
                                HAVING COUNT(mv.VillainId) > 3
                                ORDER BY COUNT(mv.VillainId)";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = (string)reader["Name"];
                            int count = (int)reader["MinionsCount"];

                            Console.WriteLine($"{name} - {count}");
                        }
                    }
                }
            }
        }
    }
}
