using Npgsql;

namespace WebApiServer.Data
{
    public static class DataBaseInitializer
    {
        public static void Initialize(string connectionString)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS Messages " +
                        "(Id SERIAL PRIMARY KEY, " +
                        "Content CHARACTER VARYING(128) NOT NULL, " +
                        "Timestamp TIMESTAMP WITH TIME ZONE NOT NULL, " +
                        "Number INTEGER NOT NULL)", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                Console.WriteLine($"DataBaseInitializer Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }
    }   
}
