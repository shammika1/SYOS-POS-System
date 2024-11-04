using Microsoft.Data.SqlClient;
using SYOS.Server.Singleton;
using SYOS.Shared.DTO;
using SYOS.Server.Exceptions;

namespace SYOS.Server.DataGateway
{
    public class ShelfGateway
    {
        public async Task AddShelfAsync(ShelfDTO shelf)
        {
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = @"INSERT INTO Shelf (ShelfLocation, ShelfQuantity, ItemID, Version) 
                              VALUES (@ShelfLocation, @ShelfQuantity, @ItemID, @Version)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ShelfLocation", shelf.ShelfLocation);
                command.Parameters.AddWithValue("@ShelfQuantity", shelf.ShelfQuantity);
                command.Parameters.AddWithValue("@ItemID", shelf.ItemCode);
                command.Parameters.AddWithValue("@Version", 1); // Initial version
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                shelf.Version = 1;
            }
        }

        public async Task DeleteShelfAsync(int shelfID)
        {
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = "DELETE FROM Shelf WHERE ShelfID = @ShelfID";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ShelfID", shelfID);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<ShelfDTO>> GetAllShelvesAsync()
        {
            var shelves = new List<ShelfDTO>();
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = "SELECT * FROM Shelf";
                var command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        shelves.Add(new ShelfDTO
                        {
                            ShelfID = Convert.ToInt32(reader["ShelfID"]),
                            ShelfLocation = reader["ShelfLocation"].ToString(),
                            ShelfQuantity = Convert.ToInt32(reader["ShelfQuantity"]),
                            ItemCode = reader["ItemID"].ToString(),
                            Version = Convert.ToInt32(reader["Version"])
                        });
                    }
                }
            }
            return shelves;
        }

        public async Task<ShelfDTO> GetShelfByIdAsync(int shelfID)
        {
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = "SELECT * FROM Shelf WHERE ShelfID = @ShelfID";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ShelfID", shelfID);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new ShelfDTO
                        {
                            ShelfID = Convert.ToInt32(reader["ShelfID"]),
                            ShelfLocation = reader["ShelfLocation"].ToString(),
                            ShelfQuantity = Convert.ToInt32(reader["ShelfQuantity"]),
                            ItemCode = reader["ItemID"].ToString(),
                            Version = Convert.ToInt32(reader["Version"])
                        };
                    }
                }
            }
            return null;
        }

        public async Task UpdateShelfAsync(ShelfDTO shelf)
        {
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = @"UPDATE Shelf 
                              SET ShelfQuantity = @ShelfQuantity, Version = @NewVersion 
                              WHERE ShelfID = @ShelfID AND Version = @OldVersion";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ShelfQuantity", shelf.ShelfQuantity);
                command.Parameters.AddWithValue("@ShelfID", shelf.ShelfID);
                command.Parameters.AddWithValue("@OldVersion", shelf.Version);
                command.Parameters.AddWithValue("@NewVersion", shelf.Version + 1);

                await connection.OpenAsync();
                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    throw new ConcurrencyException("The shelf has been modified by another user.");
                }

                shelf.Version++; // Update the version number
            }
        }

        public async Task<List<ShelfDTO>> GetShelvesByItemCodeAsync(string itemCode)
        {
            var shelves = new List<ShelfDTO>();
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = "SELECT * FROM Shelf WHERE ItemID = @ItemID ORDER BY ShelfLocation";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ItemID", itemCode);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        shelves.Add(new ShelfDTO
                        {
                            ShelfID = Convert.ToInt32(reader["ShelfID"]),
                            ShelfLocation = reader["ShelfLocation"].ToString(),
                            ShelfQuantity = Convert.ToInt32(reader["ShelfQuantity"]),
                            ItemCode = reader["ItemID"].ToString(),
                            Version = Convert.ToInt32(reader["Version"])
                        });
                    }
                }
            }
            return shelves;
        }
    }
}