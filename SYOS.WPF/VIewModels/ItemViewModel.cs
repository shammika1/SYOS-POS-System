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
    public class ItemViewModel : INotifyPropertyChanged
    {
        private readonly IItemService _itemService;
        private readonly SignalRClient _signalRClient;
        private ObservableCollection<ItemDTO> _items;
        private ItemDTO _selectedItem;
        private string _itemCode;
        private string _name;
        private decimal _price;

        public ObservableCollection<ItemDTO> Items
        {
            get => _items;
            set { _items = value; OnPropertyChanged(); }
        }

        public ItemDTO SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        public string ItemCode
        {
            get => _itemCode;
            set { _itemCode = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public decimal Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        public ICommand AddItemCommand { get; }
        public ICommand EditItemCommand { get; }
        public ICommand DeleteItemCommand { get; }

        public ItemViewModel(IItemService itemService, SignalRClient signalRClient)
        {
            _itemService = itemService;
            _signalRClient = signalRClient;
            LoadItemsAsync();

            AddItemCommand = new RelayCommand(AddItemAsync);
            EditItemCommand = new RelayCommand(EditItemAsync, () => SelectedItem != null);
            DeleteItemCommand = new RelayCommand(DeleteItemAsync, () => SelectedItem != null);

            SetupSignalRHandlers();
        }

        private async Task LoadItemsAsync()
        {
            var items = await _itemService.GetAllItemsAsync();
            Items = new ObservableCollection<ItemDTO>(items);
        }

        private async Task AddItemAsync()
        {
            var newItem = new ItemDTO { ItemCode = ItemCode, Name = Name, Price = Price };
            await _itemService.AddItemAsync(newItem);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Items.Add(newItem);
            });
            ClearFields();
        }

        private async Task EditItemAsync()
        {
            if (SelectedItem != null)
            {
                SelectedItem.Name = Name;
                SelectedItem.Price = Price;
                await _itemService.EditItemAsync(SelectedItem);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    int index = Items.IndexOf(SelectedItem);
                    Items[index] = SelectedItem;
                });
                ClearFields();
            }
        }

        private async Task DeleteItemAsync()
        {
            if (SelectedItem != null)
            {
                await _itemService.DeleteItemAsync(SelectedItem.ItemCode);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Items.Remove(SelectedItem);
                });
                ClearFields();
            }
        }

        private void ClearFields()
        {
            ItemCode = string.Empty;
            Name = string.Empty;
            Price = 0;
            SelectedItem = null;
        }

        private void SetupSignalRHandlers()
        {
            _signalRClient.OnItemUpdated(HandleItemUpdate);
            _signalRClient.OnItemDeleted(HandleItemDelete);
        }

        private void HandleItemUpdate(ItemDTO updatedItem)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var existingItem = Items.FirstOrDefault(i => i.ItemCode == updatedItem.ItemCode);
                if (existingItem != null)
                {
                    var index = Items.IndexOf(existingItem);
                    Items[index] = updatedItem;
                }
                else
                {
                    Items.Add(updatedItem);
                }
            });
        }

        private void HandleItemDelete(string itemCode)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var itemToRemove = Items.FirstOrDefault(i => i.ItemCode == itemCode);
                if (itemToRemove != null)
                {
                    Items.Remove(itemToRemove);
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