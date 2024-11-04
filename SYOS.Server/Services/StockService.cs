using Microsoft.AspNetCore.SignalR;
using SYOS.Server.DataGateway;
using SYOS.Server.Hubs;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;
using System.Data;
using SYOS.Server.Exceptions;

namespace SYOS.Server.Services
{
    public class StockService : IStockService
    {
        private readonly StockGateway _stockGateway;
        private readonly IHubContext<SYOSHub> _hubContext;

        public StockService(StockGateway stockGateway, IHubContext<SYOSHub> hubContext)
        {
            _stockGateway = stockGateway;
            _hubContext = hubContext;
        }

        public async Task<List<StockDTO>> GetAllStocksAsync()
        {
            return await _stockGateway.GetAllStocksAsync();
        }

        public async Task<List<StockDTO>> GetStocksByItemCodeAsync(string itemCode)
        {
            return await _stockGateway.GetStocksByItemCodeAsync(itemCode);
        }

        public async Task<StockDTO> GetStockByIdAsync(int stockId)
        {
            return await _stockGateway.GetStockByIdAsync(stockId);
        }

        public async Task AddStockAsync(StockDTO stock)
        {
            await _stockGateway.AddStockAsync(stock);
            await _hubContext.Clients.All.SendAsync("ReceiveStockUpdate", stock);
        }

        public async Task UpdateStockAsync(StockDTO stock)
        {
            try
            {
                await _stockGateway.UpdateStockAsync(stock);
                await _hubContext.Clients.All.SendAsync("ReceiveStockUpdate", stock);
            }
            catch (ConcurrencyException)
            {
                // Fetch the latest version from the database
                var latestStock = await _stockGateway.GetStockByIdAsync(stock.StockID);
                await _hubContext.Clients.All.SendAsync("ReceiveStockUpdate", latestStock);
                throw; // Rethrow the exception to be handled by the controller
            }
        }

        public async Task DeleteStockAsync(int stockId)
        {
            await _stockGateway.DeleteStockAsync(stockId);
            await _hubContext.Clients.All.SendAsync("ReceiveStockDelete", stockId);
        }
    }
}