   M S B A M L   `   `   `  ����  SYOS.WPF(    "SYOS.WPF.Views.StockManagementView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      .��5      � $��10q�6     � RowDefinitions  5	   
   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   .+�5      �� $��Stock Management��6   $��20(�60   $
��Bold"�6>   $��0,0,0,10q�6P   � 5       � ColumnDefinitions 5      �� 5      $
��Auto �6'   �� 5      $��* �6'     5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   .+�5      �� $��
Item Code:��6   $��0��6.   $��0��6;   $��0,0,10,5q�6K   �� 5      $��0��64   $��1��6A   $��0,0,0,5q�6Q   ��6   �� *
ItemCode+6Q   �� 5!      $��	Quantity:��6   $��1��6-   $��0��6:   $��0,0,10,5q�6J   �� 5"      $��1��64   $��1��6A   $��0,0,0,5q�6Q   ��6   �� *
Quantity+6Q   �� 5$      $��Date of Purchase:��6   $��2��65   $��0��6B   $��0,0,10,5q�6R   (  "System.Windows.Controls.DatePicker  5%      $��2��6E   $��1��6R   $��0,0,0,5q�6b      SelectedDate 6   �� *DateOfPurchase+6b   �� 5'      $��Expiry Date:��6   $��3��60   $��0��6=   $��0,0,10,5q�6M     5(      $��3��6A   $��1��6N   $��0,0,0,5q�6^    6   �� *
ExpiryDate+6^   �� 5*       �� Orientation$ 
Horizontal=�6   $��4��66   $��1��6C   $��0,10,0,0q�6S   .+�5+      �� $��	Add Stock��6   $	��100��6U   $��0,0,10,0q�6a   ��61   �� *AddStockCommand+6a   �� 5,      $��Update Stock��6   $	��100��6[   $��0,0,10,0q�6g   ��64   �� *UpdateStockCommand+6g   �� 5-      $��Delete Stock��6   $	��100��6[   ��64   �� *DeleteStockCommand+6[   5.      5/      50      �� 52   
   $��1��6   $ 
Horizontal=�6"   $��	0,10,0,10q�6;   .+�53      �� $��Search by Item Code:��6   $��Center=�64   $��0,0,10,0q�6O   �� 54      $	��150��66   $��0,0,10,0q�6B   ��6   �� *SearchItemCode+6B   �� 55      $��Search��6   $��80��6M   $��0,0,10,0q�6X   ��6&   �� *SearchStockCommand+6X   �� 56      $��Clear Search��6   $	��100��6S   ��6,   �� *ClearSearchCommand+6S   57      &   System.Windows.Controls.DataGrid  59   
   $��2��6      AutoGenerateColumns$ False��5:      $��0,20,0,0q�6/   ��59       �� *Stocks+ �� SelectedItem 6?   �� *SelectedStock+   Columns 5;      0  *System.Windows.Controls.DataGridTextColumn  5<      ,  &System.Windows.Controls.DataGridColumn   Header$ Stock ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6S   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 67   �� *	StockID+6S     5=      $ 	Item Code��6%   $ * 6U   	 68   �� *
ItemCode+6U     5>      $ Quantity��6%   $ * 6T   	 67   �� *
Quantity+6T     5?      $ Date of Purchase��6%   $ * 6r   	 6?   �� *DateOfPurchase+
 �� StringFormat$
 d��6r     5@      $ Expiry Date��6%   $ * 6i   	 6:   �� *
ExpiryDate+$
 d��6i   5B      5C      5D      