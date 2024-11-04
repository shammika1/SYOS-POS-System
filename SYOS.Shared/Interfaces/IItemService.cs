using SYOS.Shared.DTO;

namespace SYOS.Shared.Interfaces;

public interface IItemService
{
    Task<ItemDTO> GetItemAsync(string itemCode);
    Task<List<ItemDTO>> GetAllItemsAsync();
    Task AddItemAsync(ItemDTO item);
    Task EditItemAsync(ItemDTO item);
    Task DeleteItemAsync(string itemCode);
}