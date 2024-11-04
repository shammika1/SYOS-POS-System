using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;

namespace SYOS.WPF.Services
{
    public class ReportService : IReportService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/report";

        public ReportService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Ensure the base address is set
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri("https://localhost:5000/"); // Adjust this URL to match your server
            }
        }

        public async Task<DailySaleReportDTO> GetDailySaleReportAsync(DateTime date)
        {
            return await _httpClient.GetFromJsonAsync<DailySaleReportDTO>($"{BaseUrl}/daily-sale/{date:yyyy-MM-dd}");
        }

        public async Task<ReshelveReportDTO> GetReshelveReportAsync()
        {
            return await _httpClient.GetFromJsonAsync<ReshelveReportDTO>($"{BaseUrl}/reshelve");
        }

        public async Task<ReorderReportDTO> GetReorderReportAsync()
        {
            return await _httpClient.GetFromJsonAsync<ReorderReportDTO>($"{BaseUrl}/reorder");
        }

        public async Task<StockReportDTO> GetStockReportAsync()
        {
            return await _httpClient.GetFromJsonAsync<StockReportDTO>($"{BaseUrl}/stock");
        }

        public async Task<BillReportDTO> GetBillReportAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            string url = $"{BaseUrl}/bill";
            if (startDate.HasValue)
                url += $"?startDate={startDate.Value:yyyy-MM-dd}";
            if (endDate.HasValue)
                url += $"{(startDate.HasValue ? "&" : "?")}endDate={endDate.Value:yyyy-MM-dd}";

            return await _httpClient.GetFromJsonAsync<BillReportDTO>(url);
        }
    }
}