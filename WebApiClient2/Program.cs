using Microsoft.AspNetCore.SignalR.Client;

namespace WebApiClient
{
    class Program
    {
        private static HubConnection? connection;

        const string urlHub = "http://localhost:5175/messageHub";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Client 2");

            connection = new HubConnectionBuilder().WithUrl(urlHub).Build();

            connection.On<string, DateTime, int>("ReceiveMessage", (message, timestamp, number) => 
            {
                Console.WriteLine($"Message {number}: {message} | {timestamp}");
            });

            try
            {
                await connection.StartAsync();
                Console.WriteLine("Connected to SignalR");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connetion error: {ex}");
            }

            Console.WriteLine("'Enter' to exit");
            Console.ReadKey();
        }
    }
}