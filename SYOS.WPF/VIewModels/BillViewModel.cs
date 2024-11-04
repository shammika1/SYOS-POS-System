using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;
using SYOS.WPF.Command;
using SYOS.WPF.Services;

namespace SYOS.WPF.ViewModels
{
    public class BillViewModel : INotifyPropertyChanged
    {
        private readonly IBillService _billService;
        private readonly IItemService _itemService;
        private readonly SignalRClient _signalRClient;
        private ObservableCollection<BillItemDTO> _currentBillItems;
        private ObservableCollection<BillDTO> _bills;
        private string _selectedItemCode;
        private int _quantity;
        private decimal _discount;
        private decimal _cashTendered;

        public ObservableCollection<BillItemDTO> CurrentBillItems
        {
            get => _currentBillItems;
            set { _currentBillItems = value; OnPropertyChanged(); }
        }

        public ObservableCollection<BillDTO> Bills
        {
            get => _bills;
            set { _bills = value; OnPropertyChanged(); }
        }

        public string SelectedItemCode
        {
            get => _selectedItemCode;
            set { _selectedItemCode = value; OnPropertyChanged(); }
        }

        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); }
        }

        public decimal Discount
        {
            get => _discount;
            set { _discount = value; OnPropertyChanged(); }
        }

        private string _cashTenderedString;
        public string CashTenderedString
        {
            get => _cashTenderedString;
            set
            {
                if (_cashTenderedString != value)
                {
                    _cashTenderedString = value;
                    OnPropertyChanged();
                    if (decimal.TryParse(value, out decimal result))
                    {
                        CashTendered = result;
                    }
                    else
                    {
                        CashTendered = 0;
                    }
                }
            }
        }

        public decimal CashTendered { get; private set; }



        public ICommand AddItemCommand { get; }
        public ICommand ProcessSaleCommand { get; }

        public BillViewModel(IBillService billService, IItemService itemService, SignalRClient signalRClient)
        {
            _billService = billService;
            _itemService = itemService;
            _signalRClient = signalRClient;
            CurrentBillItems = new ObservableCollection<BillItemDTO>();
            Bills = new ObservableCollection<BillDTO>();

            AddItemCommand = new RelayCommand(AddItemAsync, CanAddItem);
            ProcessSaleCommand = new RelayCommand(ProcessSaleAsync, CanProcessSale);

            LoadBillsAsync();
            SetupSignalRHandlers();

            Debug.WriteLine("BillViewModel initialized");
        }

        private async Task LoadBillsAsync()
        {
            var bills = await _billService.GetAllBillsAsync();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Bills = new ObservableCollection<BillDTO>(bills);
            });
        }

        private async Task AddItemAsync()
        {
            Debug.WriteLine($"AddItemAsync called. SelectedItemCode: {SelectedItemCode}, Quantity: {Quantity}");
            try
            {
                var item = await _itemService.GetItemAsync(SelectedItemCode);
                if (item != null)
                {
                    var billItem = new BillItemDTO
                    {
                        ItemCode = item.ItemCode,
                        ItemName = item.Name,
                        Quantity = Quantity,
                        TotalPrice = item.Price * Quantity
                    };
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        CurrentBillItems.Add(billItem);
                        Debug.WriteLine($"Item added to bill: {billItem.ItemCode}, {billItem.Quantity}");
                    });
                    ClearItemFields();
                }
                else
                {
                    Debug.WriteLine($"Item not found: {SelectedItemCode}");
                    MessageBox.Show("Item not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddItemAsync: {ex}");
                MessageBox.Show($"Error adding item: {ex.Message}");
            }
        }

        private bool CanAddItem()
        {
            var result = !string.IsNullOrWhiteSpace(SelectedItemCode) && Quantity > 0;
            Debug.WriteLine($"CanAddItem called. Result: {result}");
            return result;
        }

        private async Task ProcessSaleAsync()
        {
            try
            {
                var bill = await _billService.ProcessSaleAsync(CurrentBillItems.ToList(), Discount, CashTendered);
                string billSummary = CreateBillSummary(bill);
                MessageBox.Show(billSummary, "Sale Processed Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearAllFields();
                // Optionally, you can update the Bills collection here if you're displaying a list of bills
                Bills.Add(bill);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing sale: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string CreateBillSummary(BillDTO bill)
        {
            StringBuilder summary = new StringBuilder();
            summary.AppendLine($"Bill ID: {bill.BillID}");
            summary.AppendLine($"Date: {bill.Date}");
            summary.AppendLine("\nItem Details:");
            summary.AppendLine("-----------------------------------");
            summary.AppendLine(string.Format("{0,-20} {1,-10} {2,-10}", "Item", "Quantity", "Price"));
            summary.AppendLine("-----------------------------------");

            foreach (var item in bill.BillItems)
            {
                summary.AppendLine(string.Format("{0,-20} {1,-10} {2,-10:C}", item.ItemName, item.Quantity, item.TotalPrice));
            }

            summary.AppendLine("-----------------------------------");
            summary.AppendLine($"Subtotal: {bill.TotalPrice + bill.Discount:C}");
            summary.AppendLine($"Discount: {bill.Discount:C}");
            summary.AppendLine($"Total Price: {bill.TotalPrice:C}");
            summary.AppendLine($"Cash Tendered: {bill.CashTendered:C}");
            summary.AppendLine($"Change: {bill.ChangeAmount:C}");

            return summary.ToString();
        }

        private void ClearAllFields()
        {
            CurrentBillItems.Clear();
            Discount = 0;
            CashTendered = 0;
            OnPropertyChanged(nameof(CurrentBillItems));
            OnPropertyChanged(nameof(Discount));
            OnPropertyChanged(nameof(CashTendered));
        }

        private bool CanProcessSale()
        {
            var result = CurrentBillItems.Any() && CashTendered > 0;
            Debug.WriteLine($"CanProcessSale called. Result: {result}");
            return result;
        }

        private void ClearItemFields()
        {
            SelectedItemCode = string.Empty;
            Quantity = 0;
        }



        private void SetupSignalRHandlers()
        {
            _signalRClient.OnBillUpdated(HandleBillUpdate);
        }

        private void HandleBillUpdate(BillDTO updatedBill)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var existingBill = Bills.FirstOrDefault(b => b.BillID == updatedBill.BillID);
                if (existingBill != null)
                {
                    var index = Bills.IndexOf(existingBill);
                    Bills[index] = updatedBill;
                }
                else
                {
                    Bills.Add(updatedBill);
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