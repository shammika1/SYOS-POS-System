   M S B A M L   `   `   `  ����  SYOS.WPF'    !SYOS.WPF.Views.ItemManagementView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      .��5      � $��10q�6     � RowDefinitions  5	   
   �� 5
      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   .+�5      �� $��Item Management��6   $��20(�6/   $
��Bold"�6=   $��0,0,0,10q�6O   � 5       � ColumnDefinitions 5      �� 5      $
��Auto �6'   �� 5      $��* �6'     5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   .+�5      �� $��
Item Code:��6   $��0��6.   $��0��6;   $��0,0,10,5q�6K   �� 5      $��0��64   $��1��6A   $��0,0,0,5q�6Q   ��6   �� *
ItemCode+6Q   �� 5      $��Name:��6   $��1��6)   $��0��66   $��0,0,10,5q�6F   �� 5       $��1��60   $��1��6=   $��0,0,0,5q�6M   ��6   �� *Name+6M   �� 5"      $��Price:��6   $��2��6*   $��0��67   $��0,0,10,5q�6G   �� 5#      $��2��61   $��1��6>   $��0,0,0,5q�6N   ��6   �� *Price+6N   �� 5%       �� Orientation$ 
Horizontal=�6   $��3��66   $��1��6C   $��0,10,0,0q�6S   .+�5&      �� $	��Add��6   $��80��6N   $��0,0,10,0q�6Y   ��6+   �� *AddItemCommand+6Y   �� 5'      $
��Edit��6   $��80��6P   $��0,0,10,0q�6[   ��6,   �� *EditItemCommand+6[   �� 5(      $��Delete��6   $��80��6T   ��6.   �� *DeleteItemCommand+6T   5)      5*      5+      &   System.Windows.Controls.DataGrid  5-   
   $��1��6      AutoGenerateColumns$ False��5.      $��0,20,0,0q�6/   ��5-       �� *Items+ �� SelectedItem 6>   �� *SelectedItem+   Columns 5/      0  *System.Windows.Controls.DataGridTextColumn  50      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Item Code��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6U   1  +System.Windows.Controls.DataGridBoundColumn   Binding 68   �� *
ItemCode+6U     51      $
 Name��6%   $ * 6L    63   �� *Name+6L     52      $ Price��6%   $ * 6^    64   �� *Price+	 �� StringFormat$	 C��6^   54      55      56      