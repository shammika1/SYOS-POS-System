using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace SYOS.WPF.Services
{
    public class UpdatedEntityEventArgs : EventArgs
    {
        public string EntityType { get; }
        public string EntityId { get; }

        public UpdatedEntityEventArgs(string entityType, string entityId)
        {
            EntityType = entityType;
            EntityId = entityId;
        }
    }

    public class RealTimeUpdateService
    {
        private HubConnection _hubConnection;

        public event EventHandler<UpdatedEntityEventArgs> EntityUpdated;

        public async Task StartAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5000/updateHub")
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string, string>("ReceiveUpdate", (entityType, entityId) =>
            {
                EntityUpdated?.Invoke(this, new UpdatedEntityEventArgs(entityType, entityId));
            });

            await _hubConnection.StartAsync();
        }

        public async Task StopAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
            }
        }
    }
}