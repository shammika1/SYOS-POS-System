using SYOS.Server.DataGateway;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SYOS.Server.Services
{
    public class ReportService : IReportService
    {
        private readonly ItemGateway _itemGateway;
        private readonly StockGateway _stockGateway;
        private readonly ShelfGateway _shelfGateway;
        private readonly BillGateway _billGateway;
        private readonly BillItemGateway _billItemGateway;

        public ReportService(
            ItemGateway itemGateway,
            StockGateway stockGateway,
            ShelfGateway shelfGateway,
            BillGateway billGateway,
            BillItemGateway billItemGateway)
        {
            _itemGateway = itemGateway;
            _stockGateway = stockGateway;
            _shelfGateway = shelfGateway;
            _billGateway = billGateway;
            _billItemGateway = billItemGateway;
        }

        public async Task<DailySaleReportDTO> GetDailySaleReportAsync(DateTime date)
        {
            var bills = await _billGateway.GetBillsByDateAsync(date);
            var billItems = await Task.WhenAll(bills.Select(b => _billItemGateway.GetBillItemsByBillIdAsync(b.BillID)));
            var allBillItems = billItems.SelectMany(bi => bi).ToList();

            var report = new DailySaleReportDTO
            {
                Date = date,
                Items = allBillItems
                    .GroupBy(bi => bi.ItemCode)
                    .Select(g => new DailySaleItemDTO
                    {
                        ItemCode = g.Key,
                        ItemName = g.First().ItemName,
                        TotalQuantity = g.Sum(bi => bi.Quantity),
                        TotalRevenue = g.Sum(bi => bi.TotalPrice)
                    })
                    .ToList(),
                TotalRevenue = allBillItems.Sum(bi => bi.TotalPrice)
            };

            return report;
        }

        public async Task<ReshelveReportDTO> GetReshelveReportAsync()
        {
            var items = await _itemGateway.GetAllItemsAsync();
            var stocks = await _stockGateway.GetAllStocksAsync();
            var shelves = await _shelfGateway.GetAllShelvesAsync();

            var report = new ReshelveReportDTO
            {
                Items = items
                    .Where(i => stocks.Where(s => s.ItemCode == i.ItemCode).Sum(s => s.Quantity) >
                                shelves.Where(sh => sh.ItemCode == i.ItemCode).Sum(sh => sh.ShelfQuantity))
                    .Select(i => new ReshelveItemDTO
                    {
                        ItemCode = i.ItemCode,
                        ItemName = i.Name,
                        QuantityToReshelve = stocks.Where(s => s.ItemCode == i.ItemCode).Sum(s => s.Quantity) -
                                             shelves.Where(sh => sh.ItemCode == i.ItemCode).Sum(sh => sh.ShelfQuantity)
                    })
                    .ToList()
            };

            return report;
        }

        public async Task<ReorderReportDTO> GetReorderReportAsync()
        {
            var items = await _itemGateway.GetAllItemsAsync();
            var stocks = await _stockGateway.GetAllStocksAsync();

            var report = new ReorderReportDTO
            {
                Items = items
                    .Where(i => stocks.Where(s => s.ItemCode == i.ItemCode).Sum(s => s.Quantity) < 50) // Assuming reorder level is 50
                    .Select(i => new ReorderItemDTO
                    {
                        ItemCode = i.ItemCode,
                        ItemName = i.Name,
                        CurrentStock = stocks.Where(s => s.ItemCode == i.ItemCode).Sum(s => s.Quantity)
                    })
                    .ToList()
            };

            return report;
        }

        public async Task<StockReportDTO> GetStockReportAsync()
        {
            var stocks = await _stockGateway.GetAllStocksAsync();
            var items = await _itemGateway.GetAllItemsAsync();

            var report = new StockReportDTO
            {
                Batches = stocks
                    .Select(s => new StockBatchDTO
                    {
                        BatchId = s.StockID,
                        ItemCode = s.ItemCode,
                        ItemName = items.FirstOrDefault(i => i.ItemCode == s.ItemCode)?.Name,
                        Quantity = s.Quantity,
                        DateOfPurchase = s.DateOfPurchase,
                        ExpiryDate = s.ExpiryDate
                    })
                    .ToList()
            };

            return report;
        }

        public async Task<BillReportDTO> GetBillReportAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var bills = await _billGateway.GetAllBillsAsync();

            if (startDate.HasValue)
                bills = bills.Where(b => b.Date >= startDate.Value).ToList();

            if (endDate.HasValue)
                bills = bills.Where(b => b.Date <= endDate.Value).ToList();

            var billItems = await Task.WhenAll(bills.Select(b => _billItemGateway.GetBillItemsByBillIdAsync(b.BillID)));

            var report = new BillReportDTO
            {
                Bills = bills.Select(b => new BillDTO
                {
                    BillID = b.BillID,
                    Date = b.Date,
                    TotalPrice = b.TotalPrice,
                    Discount = b.Discount,
                    CashTendered = b.CashTendered,
                    ChangeAmount = b.ChangeAmount,
                    BillItems = billItems.FirstOrDefault(bi => bi.Any(item => item.BillID == b.BillID)) ?? new List<BillItemDTO>()
                }).ToList()
            };

            return report;
        }
    }
}