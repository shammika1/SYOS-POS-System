using Microsoft.Data.SqlClient;
using SYOS.Server.Exceptions;
using SYOS.Server.Singleton;
using SYOS.Shared.DTO;

namespace SYOS.Server.DataGateway;

public class ItemGateway
{
    public async Task AddItemAsync(ItemDTO item)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "INSERT INTO Item (ItemCode, Name, Price) VALUES (@ItemCode, @Name, @Price)";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ItemCode", item.ItemCode);
            command.Parameters.AddWithValue("@Name", item.Name);
            command.Parameters.AddWithValue("@Price", item.Price);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task EditItemAsync(ItemDTO item)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = @"UPDATE Item 
                              SET Name = @Name, Price = @Price, Version = @NewVersion 
                              WHERE ItemCode = @ItemCode AND Version = @OldVersion";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ItemCode", item.ItemCode);
            command.Parameters.AddWithValue("@Name", item.Name);
            command.Parameters.AddWithValue("@Price", item.Price);
            command.Parameters.AddWithValue("@OldVersion", item.Version);
            command.Parameters.AddWithValue("@NewVersion", item.Version + 1);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
            {
                throw new ConcurrencyException("The item has been modified by another user.");
            }

            item.Version++; // Update the version number on the DTO
        }
    }

    public async Task DeleteItemAsync(string itemCode)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "DELETE FROM Item WHERE ItemCode = @ItemCode";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ItemCode", itemCode);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task<List<ItemDTO>> GetAllItemsAsync()
    {
        var items = new List<ItemDTO>();
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "SELECT * FROM Item";
            var command = new SqlCommand(query, connection);
            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                    items.Add(new ItemDTO
                    {
                        ItemCode = reader["ItemCode"].ToString(),
                        Name = reader["Name"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"])
                    });
            }
        }

        return items;
    }

    public async Task<ItemDTO> GetItemAsync(string itemCode)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "SELECT * FROM Item WHERE ItemCode = @ItemCode";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ItemCode", itemCode);
            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                    return new ItemDTO
                    {
                        ItemCode = reader["ItemCode"].ToString(),
                        Name = reader["Name"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"])
                    };
            }
        }

        return null;
    }
}