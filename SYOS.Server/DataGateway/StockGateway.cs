using Microsoft.Data.SqlClient;
using SYOS.Server.Exceptions;
using SYOS.Server.Singleton;
using SYOS.Shared.DTO;

namespace SYOS.Server.DataGateway
{
    public class StockGateway
    {
        public async Task AddStockAsync(StockDTO stock)
        {
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query =
                    "INSERT INTO Stock (ItemCode, Quantity, ExpiryDate, DateOfPurchase) VALUES (@ItemCode, @Quantity, @ExpiryDate, @DateOfPurchase)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ItemCode", stock.ItemCode);
                command.Parameters.AddWithValue("@Quantity", stock.Quantity);
                command.Parameters.AddWithValue("@ExpiryDate", stock.ExpiryDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DateOfPurchase", stock.DateOfPurchase);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteStockAsync(int stockID)
        {
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = "DELETE FROM Stock WHERE StockID = @StockID";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StockID", stockID);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<StockDTO>> GetAllStocksAsync()
        {
            var stocks = new List<StockDTO>();
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = "SELECT StockID, ItemCode, Quantity, ExpiryDate, DateOfPurchase FROM Stock";
                var command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        stocks.Add(new StockDTO
                        {
                            StockID = Convert.ToInt32(reader["StockID"]),
                            ItemCode = reader["ItemCode"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            ExpiryDate = reader["ExpiryDate"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader["ExpiryDate"]),
                            DateOfPurchase = Convert.ToDateTime(reader["DateOfPurchase"])
                        });
                    }
                }
            }

            return stocks;
        }
        public async Task<List<StockDTO>> GetStocksByItemCodeAsync(string itemCode)
        {
            var stocks = new List<StockDTO>();
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = "SELECT * FROM Stock WHERE ItemCode = @ItemCode";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ItemCode", itemCode);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        stocks.Add(new StockDTO
                        {
                            StockID = Convert.ToInt32(reader["StockID"]),
                            ItemCode = reader["ItemCode"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            ExpiryDate = reader["ExpiryDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ExpiryDate"]),
                            DateOfPurchase = Convert.ToDateTime(reader["DateOfPurchase"])
                        });
                    }
                }
            }
            return stocks;
        }

        public async Task<StockDTO> GetStockByIdAsync(int stockId)
        {
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = "SELECT * FROM Stock WHERE StockID = @StockID";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StockID", stockId);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new StockDTO
                        {
                            StockID = Convert.ToInt32(reader["StockID"]),
                            ItemCode = reader["ItemCode"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            ExpiryDate = reader["ExpiryDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ExpiryDate"]),
                            DateOfPurchase = Convert.ToDateTime(reader["DateOfPurchase"])
                        };
                    }
                }
            }
            return null;
        }

        public async Task UpdateStockAsync(StockDTO stock)
        {
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = @"UPDATE Stock 
                              SET Quantity = @Quantity, 
                                  ExpiryDate = @ExpiryDate, 
                                  DateOfPurchase = @DateOfPurchase,
                                  Version = @NewVersion
                              WHERE StockID = @StockID AND Version = @OldVersion";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StockID", stock.StockID);
                command.Parameters.AddWithValue("@Quantity", stock.Quantity);
                command.Parameters.AddWithValue("@ExpiryDate", stock.ExpiryDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DateOfPurchase", stock.DateOfPurchase);
                command.Parameters.AddWithValue("@OldVersion", stock.Version);
                command.Parameters.AddWithValue("@NewVersion", stock.Version + 1);

                await connection.OpenAsync();
                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    throw new ConcurrencyException("The stock has been modified by another user.");
                }

                stock.Version++; // Update the version number on the DTO
            }
        }
    }
}


