using Microsoft.Data.SqlClient;
using SYOSSytem.DTO;
using SYOSSytem.Singleton;

namespace SYOSSytem.DataGateway;

public class BillItemGateway
{
    public virtual void AddBillItem(BillItemDTO billItem)
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query =
                "INSERT INTO BillItem (BillID, ItemCode, ItemName, Quantity, TotalPrice) VALUES (@BillID, @ItemCode, @ItemName, @Quantity, @TotalPrice)";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@BillID", billItem.BillID);
            command.Parameters.AddWithValue("@ItemCode", billItem.ItemCode);
            command.Parameters.AddWithValue("@ItemName", billItem.ItemName);
            command.Parameters.AddWithValue("@Quantity", billItem.Quantity);
            command.Parameters.AddWithValue("@TotalPrice", billItem.TotalPrice);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public List<BillItemDTO> GetAllBillItems()
    {
        var billItems = new List<BillItemDTO>();

        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "SELECT BillID, ItemCode, Quantity, TotalPrice FROM BillItem";
            var command = new SqlCommand(query, connection);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var billItem = new BillItemDTO
                {
                    BillID = reader["BillID"].ToString(),
                    ItemCode = reader["ItemCode"].ToString(),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    TotalPrice = Convert.ToDecimal(reader["TotalPrice"])
                };
                billItems.Add(billItem);
            }
        }

        return billItems;
    }
}