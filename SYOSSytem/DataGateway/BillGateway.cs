using Microsoft.Data.SqlClient;
using SYOSSytem.DTO;
using SYOSSytem.Singleton;

namespace SYOSSytem.DataGateway;

public class BillGateway
{
    public void AddBill(BillDTO bill)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            bill.BillID = GenerateUniqueBillID(connection);

            var query =
                "INSERT INTO Bill (BillID, Date, TotalPrice, Discount, CashTendered, ChangeAmount) VALUES (@BillID, @Date, @TotalPrice, @Discount, @CashTendered, @ChangeAmount)";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@BillID", bill.BillID);
            command.Parameters.AddWithValue("@Date", bill.Date == default ? DateTime.Now : bill.Date);
            command.Parameters.AddWithValue("@TotalPrice", bill.TotalPrice);
            command.Parameters.AddWithValue("@Discount", bill.Discount);
            command.Parameters.AddWithValue("@CashTendered", bill.CashTendered);
            command.Parameters.AddWithValue("@ChangeAmount", bill.ChangeAmount);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    private string GenerateUniqueBillID(SqlConnection connection)
    {
        var datePart = DateTime.Now.ToString("yyyyMMdd");
        var query = "SELECT COUNT(*) FROM Bill WHERE BillID LIKE @BillIDPattern";
        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@BillIDPattern", datePart + "-%");

        connection.Open();
        var count = (int)command.ExecuteScalar();
        connection.Close();

        var serialNumber = (count + 1).ToString("D4");
        return $"{datePart}-{serialNumber}";
    }

    public virtual List<BillDTO> GetAllBills()
    {
        var bills = new List<BillDTO>();

        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "SELECT * FROM Bill";
            var command = new SqlCommand(query, connection);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var bill = new BillDTO
                {
                    BillID = reader["BillID"].ToString(),
                    Date = Convert.ToDateTime(reader["Date"]),
                    TotalPrice = Convert.ToDecimal(reader["TotalPrice"]),
                    Discount = Convert.ToDecimal(reader["Discount"]),
                    CashTendered = Convert.ToDecimal(reader["CashTendered"]),
                    ChangeAmount = Convert.ToDecimal(reader["ChangeAmount"])
                };
                bills.Add(bill);
            }
        }

        return bills;
    }
}