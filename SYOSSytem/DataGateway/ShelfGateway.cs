using Microsoft.Data.SqlClient;
using SYOSSytem.DTO;
using SYOSSytem.Singleton;

namespace SYOSSytem.DataGateway;

public class ShelfGateway
{
    public virtual void AddShelf(ShelfDTO shelf)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query =
                "INSERT INTO Shelf (ShelfLocation, ShelfQuantity, ItemID) VALUES (@ShelfLocation, @ShelfQuantity, @ItemID)";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ShelfLocation", shelf.ShelfLocation);
            command.Parameters.AddWithValue("@ShelfQuantity", shelf.ShelfQuantity);
            command.Parameters.AddWithValue("@ItemID", shelf.ItemCode);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public virtual void DeleteShelf(int shelfID)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "DELETE FROM Shelf WHERE ShelfID = @ShelfID";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ShelfID", shelfID);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public virtual List<ShelfDTO> GetAllShelves()
    {
        var shelves = new List<ShelfDTO>();
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "SELECT * FROM Shelf";
            var command = new SqlCommand(query, connection);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
                shelves.Add(new ShelfDTO
                {
                    ShelfID = Convert.ToInt32(reader["ShelfID"]),
                    ShelfLocation = reader["ShelfLocation"].ToString(),
                    ShelfQuantity = Convert.ToInt32(reader["ShelfQuantity"]),
                    ItemCode = reader["ItemID"].ToString()
                });
        }

        return shelves;
    }

    public virtual ShelfDTO GetShelfById(int shelfID)
    {
        ShelfDTO shelf = null;
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "SELECT * FROM Shelf WHERE ShelfID = @ShelfID";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ShelfID", shelfID);
            connection.Open();
            var reader = command.ExecuteReader();
            if (reader.Read())
                shelf = new ShelfDTO
                {
                    ShelfID = Convert.ToInt32(reader["ShelfID"]),
                    ShelfLocation = reader["ShelfLocation"].ToString(),
                    ShelfQuantity = Convert.ToInt32(reader["ShelfQuantity"]),
                    ItemCode = reader["ItemID"].ToString()
                };
        }

        return shelf;
    }

    public virtual List<ShelfDTO> GetShelvesByItemCode(string itemCode)
    {
        var shelves = new List<ShelfDTO>();
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "SELECT * FROM Shelf WHERE ItemID = @ItemID ORDER BY ShelfLocation";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ItemID", itemCode);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
                shelves.Add(new ShelfDTO
                {
                    ShelfID = Convert.ToInt32(reader["ShelfID"]),
                    ShelfLocation = reader["ShelfLocation"].ToString(),
                    ShelfQuantity = Convert.ToInt32(reader["ShelfQuantity"]),
                    ItemCode = reader["ItemID"].ToString()
                });
        }

        return shelves;
    }

    public virtual void UpdateShelf(ShelfDTO shelf)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "UPDATE Shelf SET ShelfQuantity = @ShelfQuantity WHERE ShelfID = @ShelfID";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ShelfQuantity", shelf.ShelfQuantity);
            command.Parameters.AddWithValue("@ShelfID", shelf.ShelfID);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}