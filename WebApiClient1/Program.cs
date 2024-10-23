using System.Text;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        const string url = "http://localhost:5175/api/message";
            
        Console.WriteLine("Client 1");
        Console.WriteLine("Write message ('exit' to quit): ");

        int number = 0;

        while (true)
        {
            string? content = Console.ReadLine();
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                if (content == null)
                {
                    Console.WriteLine("Message can not be null");
                    continue;
                }
                else if (content.ToLower() == "exit")
                {
                    break;
                }

                var message = new
                {
                    Content = content,
                    Timestamp = DateTime.UtcNow,
                    Number = ++number
                };

                await SendMessage(url, message);
            }
        }
    }

    static async Task SendMessage(string url, object message)
    {
        using (HttpClient client = new HttpClient())
        {
            string json = JsonConvert.SerializeObject(message);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Message is sent");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error in sending {ex.Message}");
            }
        }
    }
}