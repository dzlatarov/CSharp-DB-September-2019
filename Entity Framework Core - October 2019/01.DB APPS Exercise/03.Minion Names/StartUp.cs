namespace MinionNames
{
    using System;
    using InitialSetup;
    using System.Data.SqlClient;
    public class StartUp
    {
        public static void Main(string[] args)
        {

            var villainId = int.Parse(Console.ReadLine());

            using (var connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();

                string villianNameQuery = @"SELECT Name FROM Villains WHERE Id = @id";

                using (var command = new SqlCommand(villianNameQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", villainId);
                    var villianName = (string)command.ExecuteScalar();

                    if(villianName == null)
                    {
                        Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                        return;
                    }

                    Console.WriteLine($"Villian: {villianName}");
                }

                string minionsQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

                using (var command = new SqlCommand(minionsQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", villainId);

                    using (var reader = command.ExecuteReader())
                    {
                        if(!reader.HasRows)
                        {
                            Console.WriteLine("(no minions)");
                            return;
                        }

                        while(reader.Read())
                        {
                            var rowNumber = (long)reader["RowNum"];
                            var name = (string)reader["Name"];
                            var age = (int)reader["Age"];

                            Console.WriteLine($"{rowNumber}. {name} {age}");
                        }
                    }
                }
            }
        }
    }
}
