using System.Net.Http;
using System.Net.Http.Json;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;

namespace SYOS.WPF.Services
{
    public class StockService : IStockService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:5000/api/stock"; // Adjust port as needed

        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<StockDTO>> GetAllStocksAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<StockDTO>>(BaseUrl);
        }

        public async Task<List<StockDTO>> GetStocksByItemCodeAsync(string itemCode)
        {
            return await _httpClient.GetFromJsonAsync<List<StockDTO>>($"{BaseUrl}/byitem/{itemCode}");
        }

        public async Task<StockDTO> GetStockByIdAsync(int stockId)
        {
            return await _httpClient.GetFromJsonAsync<StockDTO>($"{BaseUrl}/{stockId}");
        }

        public async Task AddStockAsync(StockDTO stock)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, stock);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateStockAsync(StockDTO stock)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{stock.StockID}", stock);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteStockAsync(int stockId)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{stockId}");
            response.EnsureSuccessStatusCode();
        }
    }
}