namespace ChangeTownNamesCasing
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using InitialSetup;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var countryName = Console.ReadLine();

            using (var connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();

                string changeAllTownNamesQuery =
                                @"UPDATE Towns
                                  SET Name = UPPER(Name)
                                  WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

                using (var command = new SqlCommand(changeAllTownNamesQuery, connection))
                {
                    command.Parameters.AddWithValue("@countryName", countryName);

                    int? affectedRows = (int?)command.ExecuteNonQuery();

                    if (affectedRows > 0)
                    {
                        Console.WriteLine($"{affectedRows} town names were affected");
                        PrintChangedTowns(connection, countryName);
                    }
                    else if(affectedRows == 0)
                    {
                        Console.WriteLine("No town names were affected.");
                    }
                }
            }
        }

        private static void PrintChangedTowns(SqlConnection connection, string countryName)
        {
            var getAllChangeTownNamesQuery = @"SELECT t.Name 
                                             FROM Towns as t
                                             JOIN Countries AS c ON c.Id = t.CountryCode
                                             WHERE c.Name = @countryName";

            using (var command = new SqlCommand(getAllChangeTownNamesQuery, connection))
            {
                command.Parameters.AddWithValue("@countryName", countryName);


                using (var reader = command.ExecuteReader())
                {
                    var towns = new List<string>();

                    while(reader.Read())
                    {
                        towns.Add((string)reader[0]);
                    }

                    Console.WriteLine($"[{string.Join(", ",towns)}]");
                }
            }
        }
    }
}
