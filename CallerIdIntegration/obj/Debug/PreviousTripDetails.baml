   M S B A M L   `   `   `  ����  CallerIdIntegration-    'CallerIdIntegration.PreviousTripDetails   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Previous Trip Details��5   	   $	��500��6'   $	��600��67    � WindowStartupLocation$ CenterScreen=�6F   .��5      �  � RowDefinitions 5   
   �� 5      6   �� 5	      $
��.15* �6   6,   .+�5   
   �� $��0��6      BorderDataGrid#���   6   ._�5      �  5      �� 5      $
��.05* �6$   64   �� 5      $��* �6$   61   .+�5      �� $��0��6   $��Center=�6)   $
��Left=�6D   $��Previous Trip Details:��6_   $��15(�6}   $
��Bold"�6�   6�   &   System.Windows.Controls.DataGrid  5      -   5          Name$ DataGridPreviousDetails��5   r   $��1��6      
IsReadOnly .    CanUserResizeColumns .    AutoGenerateColumns .  $��0,20,0,0q�6�   ��6�   �� *ListPreviousDetails+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Telephone number��6-   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 * 6G   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6Q   �� *TelephoneNumber+ �� Mode$ TwoWay=�   SortMemberPath 6�   �� *TelephoneNumber+6�     5      $ Pick Up Address��6-   $	 * 6F   
 6P   �� *PickUpAddress+$ TwoWay=� 6   �� *PickUpAddress+6�     5      $ Drop Off Address��6-   $	 * 6G   
 6Q   �� *DropOffAddress+$ TwoWay=� 6�   �� *DropOffAddress+6�   5      5      5      �� 5   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�5      �� $��Stretch=�6   $	��120��63   $��40��6?   $��Enter��6K     DarkBlue#���  6[   $��0,0,10,0q�6~   6�   �� 5       $��Stretch=�6   $	��120��63   $��40��6?   $��	Auto-Send��6K   #���  6_   $��0,0,10,0q�6�   6�   5!      5#      5$      