using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using SYOS.Server.DataGateway;
using SYOS.Server.Exceptions;
using SYOS.Server.Hubs;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;

namespace SYOS.Server.Services
{
    public class ShelfService : IShelfService
    {
        private readonly ShelfGateway _shelfGateway;
        private readonly StockGateway _stockGateway;
        private readonly IHubContext<SYOSHub> _hubContext;
        private readonly ConcurrentDictionary<int, SemaphoreSlim> _shelfLocks = new ConcurrentDictionary<int, SemaphoreSlim>();


        public ShelfService(ShelfGateway shelfGateway, StockGateway stockGateway, IHubContext<SYOSHub> hubContext)
        {
            _shelfGateway = shelfGateway;
            _stockGateway = stockGateway;
            _hubContext = hubContext;
        }

        public async Task<List<ShelfDTO>> GetAllShelvesAsync()
        {
            return await _shelfGateway.GetAllShelvesAsync();
        }

        public async Task<ShelfDTO> GetShelfByIdAsync(int shelfId)
        {
            return await _shelfGateway.GetShelfByIdAsync(shelfId);
        }

        public async Task<ShelfDTO> AddShelfAsync(ShelfDTO shelf)
        {
            await _shelfGateway.AddShelfAsync(shelf);
            await _hubContext.Clients.All.SendAsync("ReceiveShelfUpdate", shelf);
            return shelf;
        }

        public async Task<ShelfDTO> UpdateShelfAsync(ShelfDTO shelf)
        {
            try
            {
                await _shelfGateway.UpdateShelfAsync(shelf);
                await _hubContext.Clients.All.SendAsync("ReceiveShelfUpdate", shelf);
                return shelf;
            }
            catch (ConcurrencyException)
            {
                var latestShelf = await _shelfGateway.GetShelfByIdAsync(shelf.ShelfID);
                await _hubContext.Clients.All.SendAsync("ReceiveShelfUpdate", latestShelf);
                throw; // Rethrow the exception to be handled by the controller
            }
        }

        public async Task DeleteShelfAsync(int shelfId)
        {
            await _shelfGateway.DeleteShelfAsync(shelfId);
            await _hubContext.Clients.All.SendAsync("ReceiveShelfDelete", shelfId);
        }

        public async Task AssignItemsToShelfAsync(int shelfId, string itemCode, int quantity)
        {
            // Implementation as before...
        }

        public async Task<List<ShelfDTO>> GetShelvesByItemCodeAsync(string itemCode)
        {
            return await _shelfGateway.GetShelvesByItemCodeAsync(itemCode);
        }


        public class ShelfUpdateRequest
        {
            public int ShelfId { get; set; }
            public int QuantityChange { get; set; }
        }
    }
}