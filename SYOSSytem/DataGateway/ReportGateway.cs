using Microsoft.Data.SqlClient;
using SYOSSytem.DTO;
using SYOSSytem.Singleton;

namespace SYOSSytem.DataGateway;

public class ReportGateway
{
    public virtual List<SalesReportDTO> GetDailySalesReport(DateTime date)
    {
        var salesReport = new List<SalesReportDTO>();
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = @"
                SELECT bi.ItemCode, i.Name AS ItemName, SUM(bi.Quantity) AS TotalQuantity, SUM(bi.TotalPrice) AS TotalRevenue
                FROM BillItem bi
                JOIN Bill b ON bi.BillID = b.BillID
                JOIN Item i ON bi.ItemCode = i.ItemCode
                WHERE CAST(b.Date AS DATE) = @Date
                GROUP BY bi.ItemCode, i.Name";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Date", date);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
                salesReport.Add(new SalesReportDTO
                {
                    ItemCode = reader["ItemCode"].ToString(),
                    ItemName = reader["ItemName"].ToString(),
                    TotalQuantity = Convert.ToInt32(reader["TotalQuantity"]),
                    TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"])
                });
        }

        return salesReport;
    }

    public virtual List<ReshelvingReportDTO> GetReshelvingReport()
    {
        var reshelvingReport = new List<ReshelvingReportDTO>();
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = @"
                SELECT s.ItemID AS ItemCode, i.Name AS ItemName, (100 - s.ShelfQuantity) AS Quantity
                FROM Shelf s
                JOIN Item i ON s.ItemID = i.ItemCode
                WHERE s.ShelfQuantity < 100";
            var command = new SqlCommand(query, connection);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
                reshelvingReport.Add(new ReshelvingReportDTO
                {
                    ItemCode = reader["ItemCode"].ToString(),
                    ItemName = reader["ItemName"].ToString(),
                    Quantity = Convert.ToInt32(reader["Quantity"])
                });
        }

        return reshelvingReport;
    }

    public virtual List<ReorderReportDTO> GetReorderReport()
    {
        var reorderReport = new List<ReorderReportDTO>();
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = @"
                SELECT i.ItemCode, i.Name AS ItemName, SUM(s.Quantity) AS RemainingQuantity
                FROM Stock s
                JOIN Item i ON s.ItemCode = i.ItemCode
                GROUP BY i.ItemCode, i.Name
                HAVING SUM(s.Quantity) < 50";
            var command = new SqlCommand(query, connection);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
                reorderReport.Add(new ReorderReportDTO
                {
                    ItemCode = reader["ItemCode"].ToString(),
                    ItemName = reader["ItemName"].ToString(),
                    RemainingQuantity = Convert.ToInt32(reader["RemainingQuantity"])
                });
        }

        return reorderReport;
    }

    public virtual List<StockReportDTO> GetStockReport()
    {
        var stockReport = new List<StockReportDTO>();
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = @"
                SELECT s.StockID, s.ItemCode, i.Name AS ItemName, s.Quantity, s.ExpiryDate
                FROM Stock s
                JOIN Item i ON s.ItemCode = i.ItemCode";
            var command = new SqlCommand(query, connection);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
                stockReport.Add(new StockReportDTO
                {
                    StockID = Convert.ToInt32(reader["StockID"]),
                    ItemCode = reader["ItemCode"].ToString(),
                    ItemName = reader["ItemName"].ToString(),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    ExpiryDate = reader["ExpiryDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ExpiryDate"])
                });
        }

        return stockReport;
    }

    public virtual List<BillReportDTO> GetBillReport()
    {
        var billReport = new List<BillReportDTO>();
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            var query = "SELECT * FROM Bill";
            var command = new SqlCommand(query, connection);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
                billReport.Add(new BillReportDTO
                {
                    BillID = reader["BillID"].ToString(),
                    Date = Convert.ToDateTime(reader["Date"]),
                    TotalPrice = Convert.ToDecimal(reader["TotalPrice"]),
                    Discount = Convert.ToDecimal(reader["Discount"]),
                    CashTendered = Convert.ToDecimal(reader["CashTendered"]),
                    ChangeAmount = Convert.ToDecimal(reader["ChangeAmount"])
                });
        }

        return billReport;
    }
}