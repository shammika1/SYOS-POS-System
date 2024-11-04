using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;

namespace SYOS.WPF.Services
{
    public class BillService : IBillService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/bill"; // This should be a relative URL

        public BillService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Ensure the base address is set
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri("https://localhost:5000/"); // Adjust this URL to match your server
            }
        }

        public async Task<BillDTO> CreateBillAsync(BillDTO bill)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, bill);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BillDTO>();
        }

        public async Task<BillDTO> GetBillAsync(string billId)
        {
            return await _httpClient.GetFromJsonAsync<BillDTO>($"{BaseUrl}/{billId}");
        }

        public async Task<List<BillDTO>> GetAllBillsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<BillDTO>>(BaseUrl);
        }

        public async Task<BillDTO> ProcessSaleAsync(List<BillItemDTO> billItems, decimal discount, decimal cashTendered)
        {
            var request = new
            {
                BillItems = billItems,
                Discount = discount,
                CashTendered = cashTendered
            };

            Debug.WriteLine($"Sending request to: {_httpClient.BaseAddress}{BaseUrl}/process-sale");
            Debug.WriteLine($"Request content: {JsonSerializer.Serialize(request)}");

            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/process-sale", request);

            Debug.WriteLine($"Response status: {response.StatusCode}");
            var content = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response content: {content}");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BillDTO>();
        }
    }
}