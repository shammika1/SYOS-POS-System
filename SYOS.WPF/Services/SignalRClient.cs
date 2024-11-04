using Microsoft.AspNetCore.SignalR.Client;
using SYOS.Shared.DTO;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace SYOS.WPF.Services
{
    public class SignalRClient
    {
        private HubConnection _connection;

        public async Task InitializeConnection(string url)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

            await _connection.StartAsync();
        }

        public void OnItemUpdated(Action<ItemDTO> handler)
        {
            _connection.On<ItemDTO>("ReceiveItemUpdate", item =>
            {
                Application.Current.Dispatcher.Invoke(() => handler(item));
            });
        }

        public void OnItemDeleted(Action<string> handler)
        {
            _connection.On<string>("ReceiveItemDelete", itemCode =>
            {
                Application.Current.Dispatcher.Invoke(() => handler(itemCode));
            });
        }

        public void OnStockUpdated(Action<StockDTO> handler)
        {
            _connection.On<StockDTO>("ReceiveStockUpdate", stock =>
            {
                Application.Current.Dispatcher.Invoke(() => handler(stock));
            });
        }

        public void OnStockDeleted(Action<int> handler)
        {
            _connection.On<int>("ReceiveStockDelete", stockId =>
            {
                Application.Current.Dispatcher.Invoke(() => handler(stockId));
            });
        }

        public void OnShelfUpdated(Action<ShelfDTO> handler)
        {
            _connection.On<ShelfDTO>("ReceiveShelfUpdate", shelf =>
            {
                Application.Current.Dispatcher.Invoke(() => handler(shelf));
            });
        }

        public void OnShelfDeleted(Action<int> handler)
        {
            _connection.On<int>("ReceiveShelfDelete", shelfId =>
            {
                Application.Current.Dispatcher.Invoke(() => handler(shelfId));
            });
        }

        public void OnBillUpdated(Action<BillDTO> handler)
        {
            _connection.On("ReceiveBillUpdate", (BillDTO bill) =>
            {
                Application.Current.Dispatcher.Invoke(() => handler(bill));
            });
        }

        public void OnBillDeleted(Action<string> handler)
        {
            _connection.On("ReceiveBillDelete", (string billId) =>
            {
                Application.Current.Dispatcher.Invoke(() => handler(billId));
            });
        }


        public async Task DisposeAsync()
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
            }
        }


    }
}