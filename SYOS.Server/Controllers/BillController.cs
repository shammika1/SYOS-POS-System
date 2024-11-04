using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;

namespace SYOS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;
        private readonly ILogger<BillController> _logger;

        public BillController(IBillService billService, ILogger<BillController> logger)
        {
            _billService = billService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<BillDTO>>> GetAllBills()
        {
            return await _billService.GetAllBillsAsync();
        }

        [HttpGet("{billId}")]
        public async Task<ActionResult<BillDTO>> GetBill(string billId)
        {
            var bill = await _billService.GetBillAsync(billId);
            if (bill == null)
            {
                return NotFound();
            }
            return bill;
        }

        [HttpPost]
        public async Task<ActionResult<BillDTO>> CreateBill(BillDTO bill)
        {
            var createdBill = await _billService.CreateBillAsync(bill);
            return CreatedAtAction(nameof(GetBill), new { billId = createdBill.BillID }, createdBill);
        }

        [HttpPost("process-sale")]
        public async Task<ActionResult<BillDTO>> ProcessSale([FromBody] SaleRequest request)
        {
            _logger.LogInformation($"Received ProcessSale request: {JsonSerializer.Serialize(request)}");

            if (request.BillItems == null || !request.BillItems.Any())
            {
                _logger.LogWarning("ProcessSale request has no bill items");
                return BadRequest("Bill items are required");
            }

            // Remove BillID validation, as it will be generated server-side
            foreach (var item in request.BillItems)
            {
                item.BillID = null;
            }

            try
            {
                var processedBill = await _billService.ProcessSaleAsync(request.BillItems, request.Discount, request.CashTendered);
                _logger.LogInformation($"ProcessSale successful. Bill ID: {processedBill.BillID}");
                return Ok(processedBill);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing sale");
                return StatusCode(500, "An error occurred while processing the sale");
            }
        }
    }

    public class SaleRequest
    {
        public List<BillItemDTO> BillItems { get; set; }
        public decimal Discount { get; set; }
        public decimal CashTendered { get; set; }
    }
}