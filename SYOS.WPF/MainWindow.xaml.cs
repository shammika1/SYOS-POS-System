using System.Windows;
using SYOS.WPF.ViewModels;

namespace SYOS.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow(ItemViewModel itemViewModel, StockViewModel stockViewModel, 
                          ShelfViewModel shelfViewModel, BillViewModel billViewModel,
                          ReportViewModel reportViewModel)
        {
            InitializeComponent();

            ItemManagementView.DataContext = itemViewModel;
            StockManagementView.DataContext = stockViewModel;
            ShelfManagementView.DataContext = shelfViewModel;
            BillManagementView.DataContext = billViewModel;
            ReportView.DataContext = reportViewModel;
        }
    }
}