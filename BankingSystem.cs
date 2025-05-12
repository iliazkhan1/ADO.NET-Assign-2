using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BankTransactionManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Initiating fund transfer...");

            TransactionHandler handler = new TransactionHandler();
            bool result = await handler.TransferFundsAsync("Iliaz", 200.00M);

            if (result)
                Console.WriteLine("Transfer successful.");
            else
                Console.WriteLine("Transfer failed. Transaction rolled back.");

            Console.ReadLine();
        }
    }

    public class TransactionHandler
    {
        // 🔁 Replace with your server name
        private readonly string connectionString = "Server=ILIAZKHAN\\SQLEXPRESS;Database=BankingSystem;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public async Task<bool> TransferFundsAsync(string customerName, decimal amount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Deduct from Savings
                    string deductQuery = @"
                        UPDATE Accounts
                        SET Balance = Balance - @Amount
                        WHERE CustomerName = @CustomerName AND AccountType = 'Savings' AND Balance >= @Amount";

                    using (SqlCommand cmd1 = new SqlCommand(deductQuery, conn, transaction))
                    {
                        cmd1.Parameters.AddWithValue("@Amount", amount);
                        cmd1.Parameters.AddWithValue("@CustomerName", customerName);

                        int rowsAffected1 = await cmd1.ExecuteNonQueryAsync();
                        if (rowsAffected1 == 0)
                            throw new Exception("Insufficient funds or account not found.");
                    }

                    // 2. Add to Checking
                    string addQuery = @"
                        UPDATE Accounts
                        SET Balance = Balance + @Amount
                        WHERE CustomerName = @CustomerName AND AccountType = 'Checking'";

                    using (SqlCommand cmd2 = new SqlCommand(addQuery, conn, transaction))
                    {
                        cmd2.Parameters.AddWithValue("@Amount", amount);
                        cmd2.Parameters.AddWithValue("@CustomerName", customerName);

                        int rowsAffected2 = await cmd2.ExecuteNonQueryAsync();
                        if (rowsAffected2 == 0)
                            throw new Exception("Checking account not found.");
                    }

                    // 3. Commit transaction
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}