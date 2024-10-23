using System.Net.Http.Json;

namespace WebApiClient3
{
    class Program
    {
        const string url = "http://localhost:5175/api/message";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Client 3");
            while (true)
            {
                Console.WriteLine("Press any key to get messages less then 1 minute old");
                Console.ReadKey();

                await GetRecentMessages();
            }
        }

        static async Task GetRecentMessages()
        {
            using var client = new HttpClient();
            try
            {
                var response = await client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var messages = await response.Content.ReadFromJsonAsync<Message[]>();

                if (messages == null)
                {
                    Console.WriteLine("No messages left");
                }
                else
                {
                    Console.WriteLine("Last minute messages:");
                    foreach (var message in messages)
                    {
                        Console.WriteLine($"{message.Number} | {message.Content} | {message.Timestamp}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error in connetion {ex}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }

        public class Message
        {
            public string Content { get; set; } = string.Empty;
            public int Number { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}