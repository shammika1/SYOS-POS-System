using Microsoft.AspNetCore.Mvc;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;

namespace SYOS.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ItemDTO>>> GetAllItems()
    {
        return await _itemService.GetAllItemsAsync();
    }

    [HttpGet("{itemCode}")]
    public async Task<ActionResult<ItemDTO>> GetItem(string itemCode)
    {
        var item = await _itemService.GetItemAsync(itemCode);
        if (item == null) return NotFound();
        return item;
    }

    [HttpPost]
    public async Task<ActionResult<ItemDTO>> AddItem(ItemDTO item)
    {
        await _itemService.AddItemAsync(item);
        return CreatedAtAction(nameof(GetItem), new { itemCode = item.ItemCode }, item);
    }

    [HttpPut("{itemCode}")]
    public async Task<IActionResult> EditItem(string itemCode, ItemDTO item)
    {
        if (itemCode != item.ItemCode) return BadRequest();
        await _itemService.EditItemAsync(item);
        return NoContent();
    }

    [HttpDelete("{itemCode}")]
    public async Task<IActionResult> DeleteItem(string itemCode)
    {
        await _itemService.DeleteItemAsync(itemCode);
        return NoContent();
    }
}