using Microsoft.Data.SqlClient;
using SYOS.Server.Singleton;
using SYOS.Shared.DTO;

namespace SYOS.Server.DataGateway
{
    public class BillItemGateway
    {
        public async Task AddBillItemAsync(BillItemDTO billItem)
        {
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = @"INSERT INTO BillItem (BillID, ItemCode, ItemName, Quantity, TotalPrice) 
                              VALUES (@BillID, @ItemCode, @ItemName, @Quantity, @TotalPrice)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BillID", billItem.BillID);
                command.Parameters.AddWithValue("@ItemCode", billItem.ItemCode);
                command.Parameters.AddWithValue("@ItemName", billItem.ItemName);
                command.Parameters.AddWithValue("@Quantity", billItem.Quantity);
                command.Parameters.AddWithValue("@TotalPrice", billItem.TotalPrice);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<BillItemDTO>> GetBillItemsAsync(string billId)
        {
            var billItems = new List<BillItemDTO>();
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = "SELECT * FROM BillItem WHERE BillID = @BillID";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BillID", billId);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        billItems.Add(new BillItemDTO
                        {
                            BillItemID = Convert.ToInt32(reader["BillItemID"]),
                            BillID = reader["BillID"].ToString(),
                            ItemCode = reader["ItemCode"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            TotalPrice = Convert.ToDecimal(reader["TotalPrice"])
                        });
                    }
                }
            }
            return billItems;
        }

        public async Task<List<BillItemDTO>> GetBillItemsByBillIdAsync(string billId)
        {
            var billItems = new List<BillItemDTO>();
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = "SELECT * FROM BillItem WHERE BillID = @BillID";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BillID", billId);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        billItems.Add(new BillItemDTO
                        {
                            BillItemID = Convert.ToInt32(reader["BillItemID"]),
                            BillID = reader["BillID"].ToString(),
                            ItemCode = reader["ItemCode"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            TotalPrice = Convert.ToDecimal(reader["TotalPrice"])
                        });
                    }
                }
            }
            return billItems;
        }
    }
}