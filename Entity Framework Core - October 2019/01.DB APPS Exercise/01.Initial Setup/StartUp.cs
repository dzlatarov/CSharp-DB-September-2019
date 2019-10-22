namespace InitialSetup
{
    using System.Data.SqlClient;
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();


                //string[] createTables =
                //{
                //    "CREATE TABLE Countries(Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(50))",

                //    "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries (Id))",

                //    "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(50), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))",

                //    "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(50))",

                //    "CREATE TABLE Villains(Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))",

                //    "CREATE TABLE MinionsVillains(MinionId INT FOREIGN KEY REFERENCES Minions(Id), VillainId INT FOREIGN KEY REFERENCES Villains(Id), CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))"
                //};

                //foreach (var table in createTables)
                //{
                //    Executer(table, connection);
                //}

                string[] insertData =
                {
                    "INSERT INTO Countries (Name) VALUES ('Bulgaria'), ('Germany'), ('Greece'), ('Poland'), ('Croatia')",

                    "INSERT INTO Towns (Name, CountryCode) VALUES ('Sofia', 1), ('Berlin', 2), ('Lefkada', 3), ('Krakow', 4), ('Zagreb', 5)",

                    "INSERT INTO Minions (Name, Age, TownId) VALUES ('Ivan', 34, 1), ('Frank', 25, 2), ('Niko', 40, 3), ('Irina', 18, 4), ('Antonio', 30, 5)",

                    "INSERT INTO EvilnessFactors (Name) VALUES ('super good'), ('good'), ('bad'), ('evil'), ('super evil')",

                    "INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('George', 1), ('Petar', 2), ('Grisho', 3), ('Lubo', 4), ('Genadi', 5)",

                    "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (1, 5), (2, 5), (3, 1), (3,2), (4,3), (4,2), (5,2) , (5,4)"
                };

                foreach (var data in insertData)
                {
                    Executer(data, connection);
                }
            }
        }
        public static void Executer(string text, SqlConnection connection)
        {
            var command = new SqlCommand(text, connection);
            command.ExecuteNonQuery();
        }
    }
}