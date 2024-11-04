using Microsoft.Data.SqlClient;
using SYOSSytem.DTO;
using SYOSSytem.Singleton;

namespace SYOSSytem.DataGateway;

public class StockGateway
{
    public virtual void AddStock(StockDTO stock)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query =
                "INSERT INTO Stock (ItemCode, Quantity, ExpiryDate, DateOfPurchase) VALUES (@ItemCode, @Quantity, @ExpiryDate, @DateOfPurchase)";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ItemCode", stock.ItemCode);
            command.Parameters.AddWithValue("@Quantity", stock.Quantity);
            command.Parameters.AddWithValue("@ExpiryDate", stock.ExpiryDate);
            command.Parameters.AddWithValue("@DateOfPurchase", stock.DateOfPurchase);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public virtual void DeleteStock(int stockID)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "DELETE FROM Stock WHERE StockID = @StockID";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@StockID", stockID);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public virtual List<StockDTO> GetAllStocks()
    {
        var stocks = new List<StockDTO>();
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "SELECT StockID, ItemCode, Quantity, ExpiryDate FROM Stock";
            var command = new SqlCommand(query, connection);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
                stocks.Add(new StockDTO
                {
                    StockID = Convert.ToInt32(reader["StockID"]),
                    ItemCode = reader["ItemCode"].ToString(),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    ExpiryDate = reader["ExpiryDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ExpiryDate"])
                });
        }

        return stocks;
    }

    public virtual List<StockDTO> GetStocksByItemCode(string itemCode)
    {
        var stocks = new List<StockDTO>();
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "SELECT * FROM Stock WHERE ItemCode = @ItemCode";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ItemCode", itemCode);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
                stocks.Add(new StockDTO
                {
                    StockID = Convert.ToInt32(reader["StockID"]),
                    ItemCode = reader["ItemCode"].ToString(),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    ExpiryDate = reader["ExpiryDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ExpiryDate"])
                });
        }

        return stocks;
    }

    public virtual List<StockDTO> GetStocksByStockID(int StockID)
    {
        var stocks = new List<StockDTO>();
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "SELECT * FROM Stock WHERE StockID = @StockID";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@StockID", StockID);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
                stocks.Add(new StockDTO
                {
                    StockID = Convert.ToInt32(reader["StockID"]),
                    ItemCode = reader["ItemCode"].ToString(),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    ExpiryDate = reader["ExpiryDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ExpiryDate"])
                });
        }

        return stocks;
    }

    public virtual void UpdateStock(StockDTO stock)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "UPDATE Stock SET Quantity = @Quantity WHERE StockID = @StockID";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Quantity", stock.Quantity);
            command.Parameters.AddWithValue("@StockID", stock.StockID);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}