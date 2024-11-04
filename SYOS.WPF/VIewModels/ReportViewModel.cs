using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;
using SYOS.WPF.Command;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SYOS.WPF.ViewModels
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        private readonly IReportService _reportService;
        private string _reportContent;
        private DateTime _selectedDate = DateTime.Today;
        private DateTime? _startDate;
        private DateTime? _endDate;

        private readonly object _reportLock = new object();
        private int _completedReports = 0;

        private void IncrementCompletedReports()
        {
            lock (_reportLock)
            {
                _completedReports++;
                if (_completedReports == 5) // Assuming 5 total reports
                {
                    ReportContent = "All reports completed.";
                }
            }
        }

        public string ReportContent
        {
            get => _reportContent;
            set { SetProperty(ref _reportContent, value); }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set { SetProperty(ref _selectedDate, value); }
        }

        public DateTime? StartDate
        {
            get => _startDate;
            set { SetProperty(ref _startDate, value); }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set { SetProperty(ref _endDate, value); }
        }

        public ICommand GenerateDailySaleReportCommand { get; }
        public ICommand GenerateReshelveReportCommand { get; }
        public ICommand GenerateReorderReportCommand { get; }
        public ICommand GenerateStockReportCommand { get; }
        public ICommand GenerateBillReportCommand { get; }

        public ReportViewModel(IReportService reportService)
        {
            _reportService = reportService;

            GenerateDailySaleReportCommand = new RelayCommand(GenerateDailySaleReportAsync);
            GenerateReshelveReportCommand = new RelayCommand(GenerateReshelveReportAsync);
            GenerateReorderReportCommand = new RelayCommand(GenerateReorderReportAsync);
            GenerateStockReportCommand = new RelayCommand(GenerateStockReportAsync);
            GenerateBillReportCommand = new RelayCommand(GenerateBillReportAsync);
        }

        private async Task GenerateDailySaleReportAsync()
        {
            try
            {
                ReportContent = "Generating Daily Sale Report...";

                var report = await _reportService.GetDailySaleReportAsync(SelectedDate);

                var sb = new StringBuilder();
                sb.AppendLine($"Daily Sale Report for {report.Date:d}");
                sb.AppendLine("-----------------------------------");
                foreach (var item in report.Items)
                {
                    sb.AppendLine($"{item.ItemName}: {item.TotalQuantity} sold, Revenue: {item.TotalRevenue:C}");
                }
                sb.AppendLine("-----------------------------------");
                sb.AppendLine($"Total Revenue: {report.TotalRevenue:C}");

                ReportContent = sb.ToString();
            }
            catch (Exception ex)
            {
                ReportContent = $"Error generating report: {ex.Message}";
            }
        }


        //private async Task GenerateDailySaleReportAsync()
        //{
        //    var report = await _reportService.GetDailySaleReportAsync(SelectedDate);
        //    var sb = new StringBuilder();
        //    sb.AppendLine($"Daily Sale Report for {report.Date:d}");
        //    sb.AppendLine("-----------------------------------");
        //    foreach (var item in report.Items)
        //    {
        //        sb.AppendLine($"{item.ItemName}: {item.TotalQuantity} sold, Revenue: {item.TotalRevenue:C}");
        //    }
        //    sb.AppendLine("-----------------------------------");
        //    sb.AppendLine($"Total Revenue: {report.TotalRevenue:C}");
        //    ReportContent = sb.ToString();
        //    IncrementCompletedReports();

        //}

        private async Task GenerateReshelveReportAsync()
        {
            var report = await _reportService.GetReshelveReportAsync();
            var sb = new StringBuilder();
            sb.AppendLine("Reshelve Report");
            sb.AppendLine("-----------------------------------");
            foreach (var item in report.Items)
            {
                sb.AppendLine($"{item.ItemName}: {item.QuantityToReshelve} to reshelve");
            }
            ReportContent = sb.ToString();
        }

        private async Task GenerateReorderReportAsync()
        {
            var report = await _reportService.GetReorderReportAsync();
            var sb = new StringBuilder();
            sb.AppendLine("Reorder Report");
            sb.AppendLine("-----------------------------------");
            foreach (var item in report.Items)
            {
                sb.AppendLine($"{item.ItemName}: Current Stock - {item.CurrentStock}");
            }
            ReportContent = sb.ToString();
        }

        private async Task GenerateStockReportAsync()
        {
            var report = await _reportService.GetStockReportAsync();
            var sb = new StringBuilder();
            sb.AppendLine("Stock Report");
            sb.AppendLine("-----------------------------------");
            foreach (var batch in report.Batches)
            {
                sb.AppendLine($"Batch {batch.BatchId}: {batch.ItemName}, Quantity: {batch.Quantity}, Expiry: {batch.ExpiryDate:d}");
            }
            ReportContent = sb.ToString();
        }

        private async Task GenerateBillReportAsync()
        {
            var report = await _reportService.GetBillReportAsync(StartDate, EndDate);
            var sb = new StringBuilder();
            sb.AppendLine("Bill Report");
            sb.AppendLine("-----------------------------------");
            foreach (var bill in report.Bills)
            {
                sb.AppendLine($"Bill {bill.BillID}: Date - {bill.Date:d}, Total - {bill.TotalPrice:C}");
            }
            ReportContent = sb.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public async Task GenerateAllReportsAsync()
        {
            var dailySaleTask = Task.Run(() => GenerateDailySaleReportAsync());
            var reshelveTask = Task.Run(() => GenerateReshelveReportAsync());
            var reorderTask = Task.Run(() => GenerateReorderReportAsync());
            var stockTask = Task.Run(() => GenerateStockReportAsync());
            var billTask = Task.Run(() => GenerateBillReportAsync());

            await Task.WhenAll(dailySaleTask, reshelveTask, reorderTask, stockTask, billTask);

            ReportContent = "All reports generated successfully.";
        }
    }
}