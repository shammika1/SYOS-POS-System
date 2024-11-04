using SYOS.Shared.DTO;
using System;
using System.Threading.Tasks;

namespace SYOS.Shared.Interfaces
{
    public interface IReportService
    {
        Task<DailySaleReportDTO> GetDailySaleReportAsync(DateTime date);
        Task<ReshelveReportDTO> GetReshelveReportAsync();
        Task<ReorderReportDTO> GetReorderReportAsync();
        Task<StockReportDTO> GetStockReportAsync();
        Task<BillReportDTO> GetBillReportAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}