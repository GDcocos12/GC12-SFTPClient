   M S B A M L   `   `   `  ���� J  FGC12_SFTPClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:GC12_SFTPClientGC12_SFTPClient  "    GC12_SFTPClient.CreateDialog   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:GC12_SFTPClient       � Title$  Create New...��$	��200��$	��300�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=�.��� $��10q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0�� �� Orientation$ 
Horizontal=�.+�	� -    	�Name$ rbFile��$
��File�� d� 	IsChecked$
 TrueY� 	� 	GroupName$ 
CreateType��$��0,0,10,0q�	� -   $ rbDirectory��$��	Directory��$ 
CreateType���� $��1��$��Name:���� -    ��Name$ txtName��$��2��$��0,0,0,10q��� $��3��$ 
Horizontal=�$��Right=�.+��� -   	 ��Name$	 btnOK��$��OK��$��75��$��0,0,10,0q�
 �� 	IsDefault
 . �� -   $	 	btnCancel��$��Cancel��$��75�� �� IsCancel . 