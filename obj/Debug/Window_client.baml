   M S B A M L   `   `   `  ����M  IChat_bot_multicast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Chat_bot_multicastChat_bot_multicast  5   	   &     Chat_bot_multicast.Window_client   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:Chat_bot_multicast     5   	     � Title$  Window_client��5   	   $	��450��6   $	��800��6,    � WindowStartupLocation$ CenterOwner=�68   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��304* �6   6,   �� 5      $	��65* �6   6+   �� 5      $	��65* �6   6+   .+�5   
   �� -   5         ��Name$ tb_chat��5       �� 
IsReadOnly . $��0��6q   ��6#   �� *
MyString+6q   �� 5   
   -   5        $ tb_send��5      $��Enter message here...��6#   $��16(�6@   $��1��6�   �� 5   
   -   5        $
��Send��5      $��24(�6    $��2��6D   5      5      