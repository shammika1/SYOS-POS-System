   M S B A M L   `   `   `  ����  SYOS.WPF'    !SYOS.WPF.Views.BillManagementView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      .��5      �   � RowDefinitions  5	   
   �� 5
      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��10q�6"   .+�5      �� $��Add Item to Bill��6   $
��Bold"�60   $��0,0,0,10q�6B   �� 5       �� Orientation$ 
Horizontal=�6   .+�5      �� $	��100��6a   $��0,0,10,0q�6m   $��Center=�6   ��6   �� *SelectedItemCode+ �� UpdateSourceTrigger$ PropertyChanged=�6   �� 5      $��50��6Y   $��0,0,10,0q�6d   $��Center=�6v   ��6   �� *
Quantity+$ PropertyChanged=�6v   �� 5      $��Add Item��6   $��80��6O   ��6,   �� *AddItemCommand+6O   5      5      &   System.Windows.Controls.DataGrid  5   
   $��1��6      AutoGenerateColumns$ False��6I   $��10q�6e   ��6    �� *CurrentBillItems+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Item Code��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 68   �� *
ItemCode+  5      $ 	Item Name��6%    68   �� *
ItemName+  5      $ Quantity��6%    67   �� *
Quantity+  5      $ Total Price��6%    6:   �� *
TotalPrice+ �� StringFormat$ C��5      �� 5!   
   $��2��6   $��10q�6"   .+�5"      �� $��Process Sale��6   $
��Bold"�6,   $��0,0,0,10q�6>   �� 5#      $ 
Horizontal=�6   .+�5$      �� $��	Discount:��6   $��Center=�6-   $��0,0,10,0q�6H   �� 5%      $��80��6Y   $��0,0,20,0q�6d   $��Center=�6v   ��6   �� *
Discount+$ PropertyChanged=�6v   �� 5&      $��Cash Tendered:��6   $��Center=�62   $��0,0,10,0q�6M   �� 5'      $��80��5(      $��0,0,20,0q�6%   $��Center=�67   ��5'      �� *CashTenderedString+$ PropertyChanged=�5(   7   �� 5)      $��Process Sale��6   $	��100��6W   ��60   �� *ProcessSaleCommand+6W   5*      5+      5,      5-      