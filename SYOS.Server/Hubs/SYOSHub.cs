using Microsoft.AspNetCore.SignalR;
using SYOS.Shared.DTO;

namespace SYOS.Server.Hubs
{
    public class SYOSHub : Hub
    {
        public async Task BroadcastItemUpdate(ItemDTO item)
        {
            await Clients.All.SendAsync("ReceiveItemUpdate", item);
        }

        public async Task BroadcastStockUpdate(StockDTO stock)
        {
            await Clients.All.SendAsync("ReceiveStockUpdate", stock);
        }

        public async Task BroadcastItemDelete(string itemCode)
        {
            await Clients.All.SendAsync("ReceiveItemDelete", itemCode);
        }

        public async Task BroadcastStockDelete(int stockId)
        {
            await Clients.All.SendAsync("ReceiveStockDelete", stockId);
        }

        public async Task BroadcastShelfUpdate(ShelfDTO shelf)
        {
            await Clients.All.SendAsync("ReceiveShelfUpdate", shelf);
        }

        public async Task BroadcastShelfDelete(int shelfId)
        {
            await Clients.All.SendAsync("ReceiveShelfDelete", shelfId);
        }

        public async Task BroadcastBillUpdate(BillDTO bill)
        {
            await Clients.All.SendAsync("ReceiveBillUpdate", bill);
        }

        public async Task BroadcastReportUpdate(string reportType)
        {
            await Clients.All.SendAsync("ReceiveReportUpdate", reportType);
        }
    }
}