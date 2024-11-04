using Microsoft.AspNetCore.Mvc;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace SYOS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("daily-sale/{date}")]
        public async Task<ActionResult<DailySaleReportDTO>> GetDailySaleReport(DateTime date)
        {
            return await _reportService.GetDailySaleReportAsync(date);
        }

        [HttpGet("reshelve")]
        public async Task<ActionResult<ReshelveReportDTO>> GetReshelveReport()
        {
            return await _reportService.GetReshelveReportAsync();
        }

        [HttpGet("reorder")]
        public async Task<ActionResult<ReorderReportDTO>> GetReorderReport()
        {
            return await _reportService.GetReorderReportAsync();
        }

        [HttpGet("stock")]
        public async Task<ActionResult<StockReportDTO>> GetStockReport()
        {
            return await _reportService.GetStockReportAsync();
        }

        [HttpGet("bill")]
        public async Task<ActionResult<BillReportDTO>> GetBillReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            return await _reportService.GetBillReportAsync(startDate, endDate);
        }
    }
}