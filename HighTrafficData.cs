using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EmployeeManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Fetching recent employees (joined in last 6 months)...\n");

            EmployeeRepository repo = new EmployeeRepository();
            List<Employee> employees = await repo.GetRecentEmployeesAsync();

            foreach (var emp in employees)
            {
                Console.WriteLine($"ID: {emp.Id}, Name: {emp.Name}, Joined: {emp.JoinDate.ToShortDateString()}");
            }

            Console.WriteLine("\nDone.");
            Console.ReadLine();
        }
    }

    public class EmployeeRepository
    {
        // 🔁 Replace with your actual SQL Server connection string
        private readonly string connectionString = "Server=ILIAZKHAN\\SQLEXPRESS;Database=HighTrafficData;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public async Task<List<Employee>> GetRecentEmployeesAsync()
        {
            List<Employee> employees = new List<Employee>();

            string query = @"
                SELECT Id, Name, JoinDate
                FROM Employees
                WHERE JoinDate >= DATEADD(MONTH, -6, GETDATE())
                ORDER BY JoinDate DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        employees.Add(new Employee
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            JoinDate = reader.GetDateTime(2)
                        });
                    }
                }
            }

            return employees;
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
