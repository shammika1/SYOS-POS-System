using System.Net.Http;
using System.Net.Http.Json;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;

namespace SYOS.WPF.Services;

public class ItemService : IItemService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://localhost:5000/api/item"; // Updated to use HTTPS port

    public ItemService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:5000/"); // Updated base address
    }

    public async Task<ItemDTO> GetItemAsync(string itemCode)
    {
        return await _httpClient.GetFromJsonAsync<ItemDTO>($"{BaseUrl}/{itemCode}");
    }

    public async Task<List<ItemDTO>> GetAllItemsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ItemDTO>>(BaseUrl);
    }

    public async Task AddItemAsync(ItemDTO item)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseUrl, item);
        response.EnsureSuccessStatusCode();
    }

    public async Task EditItemAsync(ItemDTO item)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{item.ItemCode}", item);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteItemAsync(string itemCode)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{itemCode}");
        response.EnsureSuccessStatusCode();
    }
}