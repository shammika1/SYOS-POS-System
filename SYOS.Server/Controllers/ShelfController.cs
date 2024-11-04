using Microsoft.AspNetCore.Mvc;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;

namespace SYOS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShelfController : ControllerBase
    {
        private readonly IShelfService _shelfService;

        public ShelfController(IShelfService shelfService)
        {
            _shelfService = shelfService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ShelfDTO>>> GetAllShelves()
        {
            return await _shelfService.GetAllShelvesAsync();
        }

        [HttpGet("{shelfId}")]
        public async Task<ActionResult<ShelfDTO>> GetShelf(int shelfId)
        {
            var shelf = await _shelfService.GetShelfByIdAsync(shelfId);
            if (shelf == null)
            {
                return NotFound();
            }
            return shelf;
        }

        [HttpPost]
        public async Task<ActionResult<ShelfDTO>> AddShelf(ShelfDTO shelf)
        {
            var addedShelf = await _shelfService.AddShelfAsync(shelf);
            return CreatedAtAction(nameof(GetShelf), new { shelfId = addedShelf.ShelfID }, addedShelf);
        }

        [HttpPut("{shelfId}")]
        public async Task<ActionResult<ShelfDTO>> UpdateShelf(int shelfId, ShelfDTO shelf)
        {
            if (shelfId != shelf.ShelfID)
            {
                return BadRequest();
            }
            try
            {
                var updatedShelf = await _shelfService.UpdateShelfAsync(shelf);
                return Ok(updatedShelf);
            }
            catch (Exception)
            {
                return StatusCode(409, "The shelf has been modified by another user. Please refresh and try again.");
            }
        }

        [HttpDelete("{shelfId}")]
        public async Task<IActionResult> DeleteShelf(int shelfId)
        {
            await _shelfService.DeleteShelfAsync(shelfId);
            return NoContent();
        }

        [HttpPost("{shelfId}/assign")]
        public async Task<IActionResult> AssignItemsToShelf(int shelfId, [FromBody] AssignItemsRequest request)
        {
            await _shelfService.AssignItemsToShelfAsync(shelfId, request.ItemCode, request.Quantity);
            return Ok();
        }

        [HttpGet("byItem/{itemCode}")]
        public async Task<ActionResult<List<ShelfDTO>>> GetShelvesByItemCode(string itemCode)
        {
            return await _shelfService.GetShelvesByItemCodeAsync(itemCode);
        }
    }

    public class AssignItemsRequest
    {
        public string ItemCode { get; set; }
        public int Quantity { get; set; }
    }
}