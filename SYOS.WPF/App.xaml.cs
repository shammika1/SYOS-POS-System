using Microsoft.Extensions.DependencyInjection;
using SYOS.WPF.Services;
using SYOS.WPF.ViewModels;
using SYOS.Shared.Interfaces;
using System.Windows;

namespace SYOS.WPF
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            // Initialize SignalR connection
            var signalRClient = serviceProvider.GetRequiredService<SignalRClient>();
            await signalRClient.InitializeConnection("https://localhost:5000/syoshub");

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register HttpClient with base address
            services.AddHttpClient<IBillService, BillService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5000/"); // Adjust this URL to match your server
            });


            // Register SignalRClient
            services.AddSingleton<SignalRClient>();
            // Register Services
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IStockService, StockService>();
            services.AddTransient<IShelfService, ShelfService>();
            services.AddTransient<IBillService, BillService>();
            services.AddTransient<IReportService, ReportService>();

            // Register ViewModels
            services.AddTransient<ItemViewModel>();
            services.AddTransient<StockViewModel>();
            services.AddTransient<ShelfViewModel>();
            services.AddTransient<BillViewModel>();
            services.AddTransient<ReportViewModel>();

            // Register Views
            services.AddTransient<MainWindow>();
        }
    }
}