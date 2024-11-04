using Microsoft.AspNetCore.Mvc;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;

namespace SYOS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<ActionResult<List<StockDTO>>> GetAllStocks()
        {
            return await _stockService.GetAllStocksAsync();
        }

        [HttpGet("{stockId}")]
        public async Task<ActionResult<StockDTO>> GetStock(int stockId)
        {
            var stock = await _stockService.GetStockByIdAsync(stockId);
            if (stock == null)
            {
                return NotFound();
            }
            return stock;
        }

        [HttpPost]
        public async Task<ActionResult<StockDTO>> AddStock(StockDTO stock)
        {
            await _stockService.AddStockAsync(stock);
            return CreatedAtAction(nameof(GetStock), new { stockId = stock.StockID }, stock);
        }

        [HttpPut("{stockId}")]
        public async Task<IActionResult> UpdateStock(int stockId, StockDTO stock)
        {
            if (stockId != stock.StockID)
            {
                return BadRequest();
            }
            await _stockService.UpdateStockAsync(stock);
            return NoContent();
        }

        [HttpDelete("{stockId}")]
        public async Task<IActionResult> DeleteStock(int stockId)
        {
            await _stockService.DeleteStockAsync(stockId);
            return NoContent();
        }
    }
}