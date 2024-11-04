using Microsoft.Data.SqlClient;
using SYOS.Server.Singleton;
using SYOS.Shared.DTO;
using SYOS.Server.Exceptions;

namespace SYOS.Server.DataGateway
{
    public class BillGateway
    {
        public async Task<string> AddBillAsync(BillDTO bill)
        {
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                bill.BillID = await GenerateUniqueBillIDAsync(connection);
                var query = @"INSERT INTO Bill (BillID, Date, TotalPrice, Discount, CashTendered, ChangeAmount, Version) 
                              VALUES (@BillID, @Date, @TotalPrice, @Discount, @CashTendered, @ChangeAmount, @Version)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BillID", bill.BillID);
                command.Parameters.AddWithValue("@Date", bill.Date);
                command.Parameters.AddWithValue("@TotalPrice", bill.TotalPrice);
                command.Parameters.AddWithValue("@Discount", bill.Discount);
                command.Parameters.AddWithValue("@CashTendered", bill.CashTendered);
                command.Parameters.AddWithValue("@ChangeAmount", bill.ChangeAmount);
                command.Parameters.AddWithValue("@Version", 1); // Initial version
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                return bill.BillID;
            }
        }

        private async Task<string> GenerateUniqueBillIDAsync(SqlConnection connection)
        {
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var query = "SELECT COUNT(*) FROM Bill WHERE BillID LIKE @BillIDPattern";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@BillIDPattern", datePart + "-%");
            await connection.OpenAsync();
            var count = (int)await command.ExecuteScalarAsync();
            await connection.CloseAsync();
            var serialNumber = (count + 1).ToString("D4");
            return $"{datePart}-{serialNumber}";
        }

        public async Task<List<BillDTO>> GetAllBillsAsync()
        {
            var bills = new List<BillDTO>();
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = "SELECT * FROM Bill";
                var command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        bills.Add(new BillDTO
                        {
                            BillID = reader["BillID"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]),
                            TotalPrice = Convert.ToDecimal(reader["TotalPrice"]),
                            Discount = Convert.ToDecimal(reader["Discount"]),
                            CashTendered = Convert.ToDecimal(reader["CashTendered"]),
                            ChangeAmount = Convert.ToDecimal(reader["ChangeAmount"]),
                            Version = Convert.ToInt32(reader["Version"])
                        });
                    }
                }
            }
            return bills;
        }

        public async Task<BillDTO> GetBillAsync(string billId)
        {
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = "SELECT * FROM Bill WHERE BillID = @BillID";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BillID", billId);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new BillDTO
                        {
                            BillID = reader["BillID"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]),
                            TotalPrice = Convert.ToDecimal(reader["TotalPrice"]),
                            Discount = Convert.ToDecimal(reader["Discount"]),
                            CashTendered = Convert.ToDecimal(reader["CashTendered"]),
                            ChangeAmount = Convert.ToDecimal(reader["ChangeAmount"]),
                            Version = Convert.ToInt32(reader["Version"])
                        };
                    }
                }
            }
            return null;
        }

        public async Task<List<BillDTO>> GetBillsByDateAsync(DateTime date)
        {
            var bills = new List<BillDTO>();
            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                var query = "SELECT * FROM Bill WHERE CAST(Date AS DATE) = @Date";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Date", date.Date);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        bills.Add(new BillDTO
                        {
                            BillID = reader["BillID"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]),
                            TotalPrice = Convert.ToDecimal(reader["TotalPrice"]),
                            Discount = Convert.ToDecimal(reader["Discount"]),
                            CashTendered = Convert.ToDecimal(reader["CashTendered"]),
                            ChangeAmount = Convert.ToDecimal(reader["ChangeAmount"])
                        });
                    }
                }
            }
            return bills;
        }
    }
}