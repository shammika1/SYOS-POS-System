using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;
using SYOS.WPF.Command;
using SYOS.WPF.Services;

namespace SYOS.WPF.ViewModels
{
    public class StockViewModel : INotifyPropertyChanged
    {
        private readonly IStockService _stockService;
        private readonly SignalRClient _signalRClient;
        private ObservableCollection<StockDTO> _stocks;
        private StockDTO _selectedStock;
        private string _itemCode;
        private int _quantity;
        private DateTime _dateOfPurchase;
        private DateTime? _expiryDate;
        private string _searchItemCode;

        public ObservableCollection<StockDTO> Stocks
        {
            get => _stocks;
            set { _stocks = value; OnPropertyChanged(); }
        }

        public StockDTO SelectedStock
        {
            get => _selectedStock;
            set
            {
                _selectedStock = value;
                if (_selectedStock != null)
                {
                    ItemCode = _selectedStock.ItemCode;
                    Quantity = _selectedStock.Quantity;
                    DateOfPurchase = _selectedStock.DateOfPurchase;
                    ExpiryDate = _selectedStock.ExpiryDate;
                }
                OnPropertyChanged();
            }
        }

        public string ItemCode
        {
            get => _itemCode;
            set { _itemCode = value; OnPropertyChanged(); }
        }

        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); }
        }

        public DateTime DateOfPurchase
        {
            get => _dateOfPurchase;
            set { _dateOfPurchase = value; OnPropertyChanged(); }
        }

        public DateTime? ExpiryDate
        {
            get => _expiryDate;
            set { _expiryDate = value; OnPropertyChanged(); }
        }

        public string SearchItemCode
        {
            get => _searchItemCode;
            set { _searchItemCode = value; OnPropertyChanged(); }
        }

        public ICommand AddStockCommand { get; }
        public ICommand UpdateStockCommand { get; }
        public ICommand DeleteStockCommand { get; }
        public ICommand SearchStockCommand { get; }
        public ICommand ClearSearchCommand { get; }

        public StockViewModel(IStockService stockService, SignalRClient signalRClient)
        {
            _stockService = stockService;
            _signalRClient = signalRClient;
            LoadStocksAsync();

            AddStockCommand = new RelayCommand(AddStockAsync);
            UpdateStockCommand = new RelayCommand(UpdateStockAsync, () => SelectedStock != null);
            DeleteStockCommand = new RelayCommand(DeleteStockAsync, () => SelectedStock != null);
            SearchStockCommand = new RelayCommand(SearchStockAsync);
            ClearSearchCommand = new RelayCommand(ClearSearchAsync);

            SetupSignalRHandlers();
        }

        private async Task LoadStocksAsync()
        {
            var stocks = await _stockService.GetAllStocksAsync();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Stocks = new ObservableCollection<StockDTO>(stocks);
            });
        }

        private async Task AddStockAsync()
        {
            var newStock = new StockDTO
            {
                ItemCode = ItemCode,
                Quantity = Quantity,
                DateOfPurchase = DateOfPurchase,
                ExpiryDate = ExpiryDate
            };
            await _stockService.AddStockAsync(newStock);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Stocks.Add(newStock);
            });
            ClearFields();
        }

        private async Task UpdateStockAsync()
        {
            if (SelectedStock != null)
            {
                SelectedStock.ItemCode = ItemCode;
                SelectedStock.Quantity = Quantity;
                SelectedStock.DateOfPurchase = DateOfPurchase;
                SelectedStock.ExpiryDate = ExpiryDate;
                await _stockService.UpdateStockAsync(SelectedStock);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    int index = Stocks.IndexOf(SelectedStock);
                    Stocks[index] = SelectedStock;
                });
                ClearFields();
            }
        }

        private async Task DeleteStockAsync()
        {
            if (SelectedStock != null)
            {
                await _stockService.DeleteStockAsync(SelectedStock.StockID);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Stocks.Remove(SelectedStock);
                });
                ClearFields();
            }
        }

        private async Task SearchStockAsync()
        {
            if (!string.IsNullOrWhiteSpace(SearchItemCode))
            {
                var stocks = await _stockService.GetStocksByItemCodeAsync(SearchItemCode);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Stocks = new ObservableCollection<StockDTO>(stocks);
                });
            }
        }

        private async Task ClearSearchAsync()
        {
            SearchItemCode = string.Empty;
            await LoadStocksAsync();
        }

        private void ClearFields()
        {
            ItemCode = string.Empty;
            Quantity = 0;
            DateOfPurchase = DateTime.Now;
            ExpiryDate = null;
            SelectedStock = null;
        }

        private void SetupSignalRHandlers()
        {
            _signalRClient.OnStockUpdated(HandleStockUpdate);
            _signalRClient.OnStockDeleted(HandleStockDelete);
        }

        private void HandleStockUpdate(StockDTO updatedStock)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var existingStock = Stocks.FirstOrDefault(s => s.StockID == updatedStock.StockID);
                if (existingStock != null)
                {
                    var index = Stocks.IndexOf(existingStock);
                    Stocks[index] = updatedStock;
                }
                else
                {
                    Stocks.Add(updatedStock);
                }
            });
        }

        private void HandleStockDelete(int stockId)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var stockToRemove = Stocks.FirstOrDefault(s => s.StockID == stockId);
                if (stockToRemove != null)
                {
                    Stocks.Remove(stockToRemove);
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}