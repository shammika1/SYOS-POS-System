using System.Net.Http;
using System.Net.Http.Json;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;

namespace SYOS.WPF.Services
{
    public class ShelfService : IShelfService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:5000/api/shelf"; // Updated to use HTTPS port

        public ShelfService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:5000/"); // Updated base address

        }

        public async Task<List<ShelfDTO>> GetAllShelvesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ShelfDTO>>(BaseUrl);
        }

        public async Task<ShelfDTO> GetShelfByIdAsync(int shelfId)
        {
            return await _httpClient.GetFromJsonAsync<ShelfDTO>($"{BaseUrl}/{shelfId}");
        }

        public async Task<ShelfDTO> AddShelfAsync(ShelfDTO shelf)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, shelf);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ShelfDTO>();
        }

        public async Task<ShelfDTO> UpdateShelfAsync(ShelfDTO shelf)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{shelf.ShelfID}", shelf);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ShelfDTO>();
        }

        public async Task DeleteShelfAsync(int shelfId)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{shelfId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task AssignItemsToShelfAsync(int shelfId, string itemCode, int quantity)
        {
            var request = new { ItemCode = itemCode, Quantity = quantity };
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/{shelfId}/assign", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<ShelfDTO>> GetShelvesByItemCodeAsync(string itemCode)
        {
            return await _httpClient.GetFromJsonAsync<List<ShelfDTO>>($"{BaseUrl}/byItem/{itemCode}");
        }
    }
}