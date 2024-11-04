using Microsoft.AspNetCore.SignalR;
using SYOS.Server.DataGateway;
using SYOS.Server.Hubs;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;
using System.Data;
using SYOS.Server.Exceptions;
using System.Collections.Concurrent;

namespace SYOS.Server.Services
{
    public class ItemService : IItemService
    {
        private readonly ItemGateway _itemGateway;
        private readonly IHubContext<SYOSHub> _hubContext;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _itemLocks;
        private readonly SemaphoreSlim _globalLock;

        public ItemService(ItemGateway itemGateway, IHubContext<SYOSHub> hubContext)
        {
            _itemGateway = itemGateway;
            _hubContext = hubContext;
            _itemLocks = new ConcurrentDictionary<string, SemaphoreSlim>();
            _globalLock = new SemaphoreSlim(1, 1);
        }

        public async Task<ItemDTO> GetItemAsync(string itemCode)
        {
            await _globalLock.WaitAsync();
            try
            {
                return await _itemGateway.GetItemAsync(itemCode);
            }
            finally
            {
                _globalLock.Release();
            }
        }

        public async Task<List<ItemDTO>> GetAllItemsAsync()
        {
            await _globalLock.WaitAsync();
            try
            {
                return await _itemGateway.GetAllItemsAsync();
            }
            finally
            {
                _globalLock.Release();
            }
        }

        public async Task AddItemAsync(ItemDTO item)
        {
            var itemLock = _itemLocks.GetOrAdd(item.ItemCode, _ => new SemaphoreSlim(1, 1));
            await itemLock.WaitAsync();
            try
            {
                await _itemGateway.AddItemAsync(item);
                await _hubContext.Clients.All.SendAsync("ReceiveItemUpdate", item);

            }
            finally
            {
                itemLock.Release();
            }
        }

        public async Task EditItemAsync(ItemDTO item)
        {
            var itemLock = _itemLocks.GetOrAdd(item.ItemCode, _ => new SemaphoreSlim(1, 1));
            await itemLock.WaitAsync();
            try
            {
                await _itemGateway.EditItemAsync(item);
                await _hubContext.Clients.All.SendAsync("ReceiveItemUpdate", item);
            }
            catch (ConcurrencyException)

            {
                // Fetch the latest version from the database
                var latestItem = await _itemGateway.GetItemAsync(item.ItemCode);
                await _hubContext.Clients.All.SendAsync("ReceiveItemUpdate", latestItem);
                throw; // Rethrow the exception to be handled by the controller
            }
            finally
            {
                itemLock.Release();
            }
        }

        //public async Task EditItemAsync(ItemDTO item)
        //{
        //    try
        //    {
        //        await _itemGateway.EditItemAsync(item);
        //        await _hubContext.Clients.All.SendAsync("ReceiveItemUpdate", item);
        //    }
        //    catch (ConcurrencyException)

        //    {
        //        // Fetch the latest version from the database
        //        var latestItem = await _itemGateway.GetItemAsync(item.ItemCode);
        //        await _hubContext.Clients.All.SendAsync("ReceiveItemUpdate", latestItem);
        //        throw; // Rethrow the exception to be handled by the controller
        //    }
        //}


        public async Task DeleteItemAsync(string itemCode)
        {
            var itemLock = _itemLocks.GetOrAdd(itemCode, _ => new SemaphoreSlim(1, 1));
            await itemLock.WaitAsync();
            try
            {
                await _itemGateway.DeleteItemAsync(itemCode);
                await _hubContext.Clients.All.SendAsync("ReceiveItemDelete", itemCode);
                _itemLocks.TryRemove(itemCode, out _);
            }
            finally
            {
                itemLock.Release();
            }
        }

    }
}