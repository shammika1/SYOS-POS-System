using Microsoft.Data.SqlClient;
using SYOSSytem.DTO;
using SYOSSytem.Singleton;

namespace SYOSSytem.DataGateway;

public class ItemGateway
{
    public virtual void AddItem(ItemDTO item)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "INSERT INTO Item (ItemCode, Name, Price) VALUES (@ItemCode, @Name, @Price)";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ItemCode", item.ItemCode);
            command.Parameters.AddWithValue("@Name", item.Name);
            command.Parameters.AddWithValue("@Price", item.Price);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public virtual void EditItem(ItemDTO item)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "UPDATE Item SET Name = @Name, Price = @Price WHERE ItemCode = @ItemCode";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ItemCode", item.ItemCode);
            command.Parameters.AddWithValue("@Name", item.Name);
            command.Parameters.AddWithValue("@Price", item.Price);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public virtual void DeleteItem(string itemCode)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "DELETE FROM Item WHERE ItemCode = @ItemCode";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ItemCode", itemCode);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public virtual List<ItemDTO> GetAllItems()
    {
        var items = new List<ItemDTO>();
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "SELECT * FROM Item";
            var command = new SqlCommand(query, connection);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
                items.Add(new ItemDTO
                {
                    ItemCode = reader["ItemCode"].ToString(),
                    Name = reader["Name"].ToString(),
                    Price = Convert.ToDecimal(reader["Price"])
                });
        }

        return items;
    }

    public virtual ItemDTO GetItem(string itemCode)
    {
        ItemDTO item = null;
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "SELECT * FROM Item WHERE ItemCode = @ItemCode";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ItemCode", itemCode);
            connection.Open();
            var reader = command.ExecuteReader();
            if (reader.Read())
                item = new ItemDTO
                {
                    ItemCode = reader["ItemCode"].ToString(),
                    Name = reader["Name"].ToString(),
                    Price = Convert.ToDecimal(reader["Price"])
                };
        }

        return item;
    }
}