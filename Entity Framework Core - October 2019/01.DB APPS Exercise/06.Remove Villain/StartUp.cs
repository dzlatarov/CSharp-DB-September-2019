namespace RemoveVillain
{
    using System;
    using System.Data.SqlClient;
    using InitialSetup;
    public class Program
    {
        public static void Main(string[] args)
        {
            int inputId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();

                string villainName = GetVillainName(connection, inputId);

                if(villainName == null)
                {
                    Console.WriteLine("No such villain was found.");
                    return;
                }

                int deletedMinionsCount = GetCountOfTheDeletedMinions(connection, inputId);
                DeleteVIllain(connection, villainName, inputId);
                Console.WriteLine($"{deletedMinionsCount} minions were released.");
            }
        }

        private static void DeleteVIllain(SqlConnection connection, string villainName, int inputId)
        {
            string deleteVillainQuery = @"DELETE FROM Villains
                                        WHERE Id = @villainId";

            using (SqlCommand command = new SqlCommand(deleteVillainQuery, connection))
            {
                command.Parameters.AddWithValue("@villainId", inputId);

                int affectedRows = command.ExecuteNonQuery();

                if(affectedRows == 0)
                {
                    throw new InvalidOperationException("Operation of deleting villain failed");
                }

                Console.WriteLine($"{villainName} was deleted.");
            }
        }

        private static int GetCountOfTheDeletedMinions(SqlConnection connection, int inputId)
        {
            string deleteFromMinionsVillainsQuery = @"DELETE FROM MinionsVillains 
                                                        WHERE VillainId = @villainId";

            using (SqlCommand command = new SqlCommand(deleteFromMinionsVillainsQuery, connection))
            {
                command.Parameters.AddWithValue("@villainId", inputId);

                int affectedRows = (int)command.ExecuteNonQuery();

                return affectedRows;
            }
        }

        private static string GetVillainName(SqlConnection connection, int inputId)
        {
            string villainIdQuery = @"SELECT Name FROM Villains WHERE Id = @villainId";

            using (SqlCommand command = new SqlCommand(villainIdQuery, connection))
            {
                command.Parameters.AddWithValue("@villainId", inputId);

                string villainName = (string)command.ExecuteScalar();               

                return villainName;
            }
        }
    }
}
