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
    public class ShelfViewModel : INotifyPropertyChanged
    {
        private readonly IShelfService _shelfService;
        private readonly SignalRClient _signalRClient;
        private ObservableCollection<ShelfDTO> _shelves;
        private ShelfDTO _selectedShelf;
        private string _shelfLocation;
        private int _shelfQuantity;
        private string _itemCode;

        public ObservableCollection<ShelfDTO> Shelves
        {
            get => _shelves;
            set { _shelves = value; OnPropertyChanged(); }
        }

        public ShelfDTO SelectedShelf
        {
            get => _selectedShelf;
            set
            {
                _selectedShelf = value;
                if (_selectedShelf != null)
                {
                    ShelfLocation = _selectedShelf.ShelfLocation;
                    ShelfQuantity = _selectedShelf.ShelfQuantity;
                    ItemCode = _selectedShelf.ItemCode;
                }
                OnPropertyChanged();
            }
        }

        public string ShelfLocation
        {
            get => _shelfLocation;
            set { _shelfLocation = value; OnPropertyChanged(); }
        }

        public int ShelfQuantity
        {
            get => _shelfQuantity;
            set { _shelfQuantity = value; OnPropertyChanged(); }
        }

        public string ItemCode
        {
            get => _itemCode;
            set { _itemCode = value; OnPropertyChanged(); }
        }

        public ICommand AddShelfCommand { get; }
        public ICommand UpdateShelfCommand { get; }
        public ICommand DeleteShelfCommand { get; }
        public ICommand AssignItemsToShelfCommand { get; }

        public ShelfViewModel(IShelfService shelfService, SignalRClient signalRClient)
        {
            _shelfService = shelfService;
            _signalRClient = signalRClient;
            LoadShelvesAsync();

            AddShelfCommand = new RelayCommand(AddShelfAsync);
            UpdateShelfCommand = new RelayCommand(UpdateShelfAsync, () => SelectedShelf != null);
            DeleteShelfCommand = new RelayCommand(DeleteShelfAsync, () => SelectedShelf != null);
            AssignItemsToShelfCommand = new RelayCommand(AssignItemsToShelfAsync, () => SelectedShelf != null);

            SetupSignalRHandlers();
        }

        private async Task LoadShelvesAsync()
        {
            var shelves = await _shelfService.GetAllShelvesAsync();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Shelves = new ObservableCollection<ShelfDTO>(shelves);
            });
        }

        private async Task AddShelfAsync()
        {
            var newShelf = new ShelfDTO
            {
                ShelfLocation = ShelfLocation,
                ShelfQuantity = ShelfQuantity,
                ItemCode = ItemCode
            };
            await _shelfService.AddShelfAsync(newShelf);
            ClearFields();
        }

        private async Task UpdateShelfAsync()
        {
            if (SelectedShelf != null)
            {
                SelectedShelf.ShelfLocation = ShelfLocation;
                SelectedShelf.ShelfQuantity = ShelfQuantity;
                SelectedShelf.ItemCode = ItemCode;
                try
                {
                    await _shelfService.UpdateShelfAsync(SelectedShelf);
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to update shelf: {ex.Message}");
                    await LoadShelvesAsync(); // Reload shelves to get the latest data
                }
            }
        }

        private async Task DeleteShelfAsync()
        {
            if (SelectedShelf != null)
            {
                await _shelfService.DeleteShelfAsync(SelectedShelf.ShelfID);
                ClearFields();
            }
        }

        private async Task AssignItemsToShelfAsync()
        {
            if (SelectedShelf != null)
            {
                try
                {
                    await _shelfService.AssignItemsToShelfAsync(SelectedShelf.ShelfID, ItemCode, ShelfQuantity);
                    MessageBox.Show("Items assigned to shelf successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to assign items to shelf: {ex.Message}");
                }
            }
        }

        private void ClearFields()
        {
            ShelfLocation = string.Empty;
            ShelfQuantity = 0;
            ItemCode = string.Empty;
            SelectedShelf = null;
        }

        private void SetupSignalRHandlers()
        {
            _signalRClient.OnShelfUpdated(HandleShelfUpdate);
            _signalRClient.OnShelfDeleted(HandleShelfDelete);
        }

        private void HandleShelfUpdate(ShelfDTO updatedShelf)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var existingShelf = Shelves.FirstOrDefault(s => s.ShelfID == updatedShelf.ShelfID);
                if (existingShelf != null)
                {
                    var index = Shelves.IndexOf(existingShelf);
                    Shelves[index] = updatedShelf;
                }
                else
                {
                    Shelves.Add(updatedShelf);
                }
            });
        }

        private void HandleShelfDelete(int shelfId)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var shelfToRemove = Shelves.FirstOrDefault(s => s.ShelfID == shelfId);
                if (shelfToRemove != null)
                {
                    Shelves.Remove(shelfToRemove);
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