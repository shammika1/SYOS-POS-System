using Microsoft.AspNetCore.SignalR;
using SYOS.Server.DataGateway;
using SYOS.Server.Hubs;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;
using System.ComponentModel;

namespace SYOS.Server.Services
{
    public class BillService : IBillService
    {
        private readonly BillGateway _billGateway;
        private readonly BillItemGateway _billItemGateway;
        private readonly ItemGateway _itemGateway;
        private readonly ShelfGateway _shelfGateway;
        private readonly IHubContext<SYOSHub> _hubContext;
        private readonly BackgroundWorker _backgroundWorker;


        public BillService(BillGateway billGateway, BillItemGateway billItemGateway,
                           ItemGateway itemGateway, ShelfGateway shelfGateway,
                           IHubContext<SYOSHub> hubContext)
        {
            _billGateway = billGateway;
            _billItemGateway = billItemGateway;
            _itemGateway = itemGateway;
            _shelfGateway = shelfGateway;
            _hubContext = hubContext;
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += ProcessBillsBackgroundTask;
            _backgroundWorker.RunWorkerAsync();
        }

        private void ProcessBillsBackgroundTask(object sender, DoWorkEventArgs e)
        {
            while (!_backgroundWorker.CancellationPending)
            {
                // Process bills, update statistics.
                Thread.Sleep(TimeSpan.FromMinutes(5)); // Run every 5 minutes
            }
        }

        public async Task<BillDTO> CreateBillAsync(BillDTO bill)
        {
            var billId = await _billGateway.AddBillAsync(bill);
            foreach (var billItem in bill.BillItems)
            {
                billItem.BillID = billId;
                await _billItemGateway.AddBillItemAsync(billItem);
            }
            bill.BillID = billId;
            await _hubContext.Clients.All.SendAsync("ReceiveBillUpdate", bill);
            return bill;
        }

        public async Task<BillDTO> GetBillAsync(string billId)
        {
            var bill = await _billGateway.GetBillAsync(billId);
            if (bill != null)
            {
                bill.BillItems = await _billItemGateway.GetBillItemsAsync(billId);
            }
            return bill;
        }

        public async Task<List<BillDTO>> GetAllBillsAsync()
        {
            return await _billGateway.GetAllBillsAsync();
        }

        public async Task<BillDTO> ProcessSaleAsync(List<BillItemDTO> billItems, decimal discount, decimal cashTendered)
        {
            var totalPrice = billItems.Sum(item => item.TotalPrice);
            var discountedPrice = totalPrice - discount;
            var changeAmount = cashTendered - discountedPrice;

            var bill = new BillDTO
            {
                Date = DateTime.Now,
                TotalPrice = discountedPrice,
                Discount = discount,
                CashTendered = cashTendered,
                ChangeAmount = changeAmount,
                BillItems = billItems
            };

            var createdBill = await CreateBillAsync(bill);

            // Update shelf quantities
            foreach (var billItem in billItems)
            {
                var shelves = await _shelfGateway.GetShelvesByItemCodeAsync(billItem.ItemCode);
                var remainingQuantity = billItem.Quantity;

                foreach (var shelf in shelves)
                {
                    if (remainingQuantity == 0) break;

                    var quantityToReduce = Math.Min(shelf.ShelfQuantity, remainingQuantity);
                    shelf.ShelfQuantity -= quantityToReduce;
                    remainingQuantity -= quantityToReduce;

                    await _shelfGateway.UpdateShelfAsync(shelf);
                    await _hubContext.Clients.All.SendAsync("ReceiveShelfUpdate", shelf);
                }

                if (remainingQuantity > 0)
                {
                    // Handle the case where there's not enough stock
                    // This could involve throwing an exception, logging, or some other business logic
                }
            }

            return createdBill;
        }
    }
}