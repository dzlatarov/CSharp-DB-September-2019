namespace AddMinion
{
    using System;
    using System.Data.SqlClient;
    using InitialSetup;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var minionInput = Console.ReadLine().Split();
            var minionName = minionInput[1];
            var minionAge = int.Parse(minionInput[2]);
            var townName = minionInput[3];

            var villainInput = Console.ReadLine().Split();
            var villainName = villainInput[1];

            using (var connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();

                int? townID = GetTownId(connection, townName);

                if (townID == null)
                {
                    AddTownId(connection, townName);
                    townID = GetTownId(connection, townName);
                }

                int? villainId = GetVillainId(connection, villainName);

                if (villainId == null)
                {
                    AddVillainId(connection, villainName);
                    villainId = GetVillainId(connection, villainName);
                }

                AddMinion(connection, minionName, minionAge, (int)townID);
                int minionId = GetMinionId(connection, minionName);
                MakeMinionServentOfVillain(connection, minionId, minionName, villainId, villainName);
            }
        }

        private static void MakeMinionServentOfVillain(SqlConnection connection, int minionId, string minionName, int? villainId, string villainName)
        {
            var makeMinionServentQuery = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";

            using (var command = new SqlCommand(makeMinionServentQuery, connection))
            {
                command.Parameters.AddWithValue("@minionId", minionId);
                command.Parameters.AddWithValue("@villainId", villainId);

                var affectedRows = 0;

                try
                {
                    affectedRows = (int)command.ExecuteNonQuery();
                }
                
                catch(Exception)
                {
                    Console.WriteLine($"Minion {minionName} is already servent of villain {villainName}");
                }

                if(affectedRows > 0)
                {
                    Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                }
            }
        }

        private static int GetMinionId(SqlConnection connection, string minionName)
        {
            var getMinionIdQuery = @"SELECT Id FROM Minions WHERE Name = @Name";

            using (var command = new SqlCommand(getMinionIdQuery, connection))
            {
                command.Parameters.AddWithValue("Name", minionName);
                int minionId = (int)command.ExecuteScalar();

                return minionId;
            }
        }

        private static void AddMinion(SqlConnection connection, string minionName, int minionAge, int townID)
        {
            var addMinionQuery = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

            using (var command = new SqlCommand(addMinionQuery, connection))
            {
                command.Parameters.AddWithValue("@name", minionName);
                command.Parameters.AddWithValue("@age", minionAge);
                command.Parameters.AddWithValue("@townId", townID);

                command.ExecuteNonQuery();
            }
        }

        private static void AddVillainId(SqlConnection connection, string villainName)
        {
            var addVillainIdQuery = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

            using (var command = new SqlCommand(addVillainIdQuery, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);
                int? affectedRows = (int?)command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    Console.WriteLine($"Villain {villainName} was added to the database.");
                }
            }
        }

        private static int? GetVillainId(SqlConnection connection, string villainName)
        {
            var getVillaninId = @"SELECT Id FROM Villains WHERE Name = @Name";

            using (var command = new SqlCommand(getVillaninId, connection))
            {
                command.Parameters.AddWithValue("@Name", villainName);
                int? villainId = (int?)command.ExecuteScalar();

                return villainId;
            }
        }

        private static void AddTownId(SqlConnection connection, string townName)
        {
            string addTownQuery = @"INSERT INTO Towns (Name) VALUES (@townName)";

            using (var command = new SqlCommand(addTownQuery, connection))
            {
                command.Parameters.AddWithValue("@townName", townName);
                int affectedRows = (int)command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    Console.WriteLine($"Town {townName} was added to the database.");
                }
            }
        }

        private static int? GetTownId(SqlConnection connection, string townName)
        {
            var getTownIdQuery = @"SELECT Id FROM Towns WHERE Name = @townName";

            using (var command = new SqlCommand(getTownIdQuery, connection))
            {
                command.Parameters.AddWithValue("@townName", townName);
                int? townId = (int?)command.ExecuteScalar();

                return townId;
            }
        }
    }
}
