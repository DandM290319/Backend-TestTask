using Npgsql;
using WebApiServer.Models;

namespace WebApiServer.Data
{
    public class MessageRepository
    {
        private readonly string connectionString;
        private readonly ILogger<MessageRepository> logger;

        public MessageRepository(string _connectionString, ILogger<MessageRepository> _logger)
        {
            connectionString = _connectionString;
            logger = _logger;
        }

        public async Task AddMessageAsync(Message message)
        {
            try
            {
                var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                logger.LogInformation($"Repository successfully connected with DataBase");

                var command = new NpgsqlCommand("INSERT INTO Messages (Content, Timestamp, Number) VALUES (@Content, @Timestamp, @Number)", connection);
                command.Parameters.AddWithValue("Content", message.Content);
                command.Parameters.AddWithValue("Timestamp", message.Timestamp);
                command.Parameters.AddWithValue("Number", message.Number);

                await command.ExecuteNonQueryAsync();
                logger.LogInformation($"Message {message.Number} | {message.Content} | {message.Timestamp} successfully added to DataBase");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in Repository AddMessage: {ex.Message}");
            }
        }

        public async Task<List<Message>> GetMessagesMinuteAgoAsync(DateTime since)
        {
            var messages = new List<Message>();

            var connection = new NpgsqlConnection(connectionString);

            try
            {
                await connection.OpenAsync();
                logger.LogInformation($"GetMessage Successfully connected with DataBase");

                var command = new NpgsqlCommand("SELECT Content, Timestamp, Number FROM Messages WHERE Timestamp >= @since", connection);
                command.Parameters.AddWithValue("Since", since);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        messages.Add(new Message
                        {
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            Timestamp = reader.GetDateTime(reader.GetOrdinal("Timestamp")),
                            Number = reader.GetInt32(reader.GetOrdinal("Number"))
                        });
                    }
                }


                return messages;
            }
            catch (Npgsql.NpgsqlException ex)
            {
                logger.LogError($"Error in Repository GetMessage method: {ex.Message}");
                return new List<Message>();
            }
            catch (Exception ex)
            {
                logger.LogError($"Unexpected Error: {ex.Message}");
                return new List<Message>();
            }
        }
    }
}
