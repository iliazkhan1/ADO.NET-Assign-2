using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RealTimeChatSystem
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(" Real-Time Chat Sender Using ADO.NET\n");

            ChatService chat = new ChatService();

            // Send messages asynchronously
            await chat.SendMessageAsync("Alice", "Bob", "Are you coming to the meeting?");
            await chat.SendMessageAsync("Bob", "Alice", "Yes, I’m on my way.");
            await chat.SendMessageAsync("Eve", "Alice", "Lunch at 1 PM?");

            Console.WriteLine("\n All messages sent without blocking.");
        }
    }

    public class ChatService
    {
        // 🛠 Update YOUR_SERVER_NAME before running
        private readonly string connectionString = "Server=ILIAZKHAN\\SQLEXPRESS;Database=Chatsystem;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public async Task SendMessageAsync(string sender, string receiver, string message)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string query = @"
                        INSERT INTO ChatMessages (Sender, Receiver, MessageText)
                        VALUES (@Sender, @Receiver, @MessageText);";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Sender", sender);
                        cmd.Parameters.AddWithValue("@Receiver", receiver);
                        cmd.Parameters.AddWithValue("@MessageText", message);

                        await cmd.ExecuteNonQueryAsync();

                        Console.WriteLine($"Sent from {sender} to {receiver} at {DateTime.Now:T}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error: {ex.Message}");
            }
        }
    }
}
