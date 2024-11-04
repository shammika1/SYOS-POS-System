using Microsoft.Data.SqlClient;

namespace TestProject;

public static class DatabaseCleaner
{
    private static readonly string connectionString =
        "Data Source=SHAMMIKA-LAPTOP\\SQLEXPRESS;Initial Catalog=SYOS_DB;Integrated Security=True;TrustServerCertificate=True";

    public static void Clean()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var commands = new[]
            {
                "DELETE FROM BillItem",
                "DELETE FROM Bill",
                "DELETE FROM Shelf",
                "DELETE FROM Stock",
                "DELETE FROM Item"
            };

            foreach (var commandText in commands)
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.ExecuteNonQuery();
                }
        }
    }
}