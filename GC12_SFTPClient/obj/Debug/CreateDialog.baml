   M S B A M L   `   `   `  ����J  FGC12_SFTPClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:GC12_SFTPClientGC12_SFTPClient  5   	   "    GC12_SFTPClient.CreateDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:GC12_SFTPClient     5   	     � Title$  Create New...��5   	   $	��200��6   $	��300��6,    � WindowStartupLocation$ CenterOwner=�5	   	    � 
ResizeMode$ NoResize=�6-   .��5
      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   .+�5      	� -   5         	�Name$ rbFile��5      $
��File��6*    d� 	IsChecked$
 TrueY�69    	� 	GroupName$ 
CreateType��6J   $��0,0,10,0q�6a   	� 5      -   5        $ rbDirectory��5      $��	Directory��6/   $ 
CreateType��6C   5      �� 5   
   $��1��6   $��Name:��6   �� 5   
   -   5         ��Name$ txtName��5      $��2��6#   $��0,0,0,10q�60   �� 5   
   $��3��6   $ 
Horizontal=�6"   $��Right=�6;   .+�5      �� -   5        	 ��Name$	 btnOK��5      $��OK��6$   $��75��61   $��0,0,10,0q�6<   
 �� 	IsDefault
 . 6b   �� 5      -   5        $	 	btnCancel��5      $��Cancel��6(   $��75��69    �� IsCancel . 6\   5      5      5      