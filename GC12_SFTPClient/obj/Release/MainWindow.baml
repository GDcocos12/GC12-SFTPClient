   M S B A M L   `   `   `  ���� J  FGC12_SFTPClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:GC12_SFTPClientGC12_SFTPClient       GC12_SFTPClient.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:GC12_SFTPClient       � Title$  GC12 SFTP Client��$	��750��$
��1000��.���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto ��� $
��Auto ��� $
��Auto � � ColumnDefinitions �� $��* ��� $
��Auto �.+��� $��0��$��0��$��2�� �� Orientation$ 
Horizontal=�$��5q�.+��� $��gcsftp_icon.png��$��48��$��48��$	��Top=�$��0,0,10,0q��  �� $
��Auto ��� $
��Auto �.+��� $��0��$
��GC12��$��Center=�$��Segoe UI Semibold)�$��18(��� $��1��$��SFTP Client��$��Center=�$��Segoe UI Semibold)�$��14(�$��0,2,0,0q��� $��1��$��0��$��2��$��5q�
�������$��1q� �� CornerRadius$ 5��._�� $��5q� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � �� $
��Auto ��� $��* �.+��� $��0��$��0��$��Host:���� -    ��Name$ txtHost��$��0��$��1��$��5q��� $��1��$��0��$��	Username:���� -   $ txtUsername��$��1��$��1��$��5q��� $��2��$��0��$��	Password:��G� -    G�Name$ txtPassword��$��2��$��1��$��5q��� $��3��$��0��$��Port:���� -   $ txtPort��$��3��$��1��$��5q�$��22���� $��2��$��1��$��5q�
�������$��1q�$ 5��._��� $ 
Horizontal=�.+��� -    ��Name$ btnLoad��$
��Load��$��5q�$
��10,3q��� -   $ btnSave��$
��Save��$��5q�$
��10,3q��� -   $ 
btnConnect��$��Connect��$��5q�$
��10,3q��� -   $ btnDisconnect��$��
Disconnect��$��5q�$
��10,3q�{�.  �� $��2��$��0��$��5q�
�������$��1q�$ 5��$
��Left=�._��� $ 
Horizontal=�.+��� -	   $ btnDownload��$��Download��$��5q�$
��10,3q�{�.  �� -
   $ 	btnUpload��$��Upload��$��5q�$
��10,3q�{�.  �� -   $ 	btnDelete��$��Delete��$��5q�$
��10,3q�{�.  �� -   $ btnEdit��$
��Edit��$��5q�$
��10,3q�{�.  �� -   $ 
btnRefresh��$��Refresh��$��5q�$
��10,3q�{�.  �� -   $ 	btnCreate��$��Create��$��5q�$
��10,3q�{�.  �� $��3��$��0��$��2��$��5q�
�������$��1q�$ 5��._��  �� $
��Auto ��� $��* �.+��� $��0��$��Files:��$
��Bold"�$��5q�R� -    R�Name$ tvFiles��$��1��$��5q�	 � 	Resources	 /   )GC12_SFTPClient.DirectoryToImageConverter   DirectoryToImageConverter*   $GC12_SFTPClient.FileToImageConverter  FileToImageConverter��%   &	        &	          ���� 
 �� ItemsSource
 �� *
Children+.R��� $ 
Horizontal=�.+���  ��Name$	 img��$��16��$��16��$��0,0,5,0q�����  �� 	Converter# [ �� ���� *DisplayName+ �� Triggers ��  �� Value$
 True�� �� Binding ��  �� RelativeSource ��  �� AncestorType$ TreeViewItemL� �� Path$ 
IsExpanded�.a���  �� 
TargetName$	 img�� �� Property � �� �� Value$ /Images/open_folder.png������  �� 
TargetType" Q�.�-   ��  �@Q�
IsExpanded �� *IsRoot+ �� Mode$ OneWay=��� -    ��Name$ 	txtStatus��$��4��$��0��$��2��$��5q�
�������� -    �Name$ progressBar��$��5��$��0��$��2��$��20��$��5q� � Minimum$ 0Q� � Maximum$	 100Q� � Value$ 0Q��� -    ��Name$ editArea��$��6��$��0��$��2��$��5q�
�������$��1q�$ 5��$y�	Collapsed=�$	��300��._��  �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��$��	Edit File��$
��Bold"�$��5q��� -   $ txtEdit��$��1��$��5q� �� AcceptsReturn .  �� TextWrapping$
 Wrap=�"  �� VerticalScrollBarVisibility$  Visible=�$! �� HorizontalScrollBarVisibility$
! Auto=��� $��2��$ 
Horizontal=�$��Right=�.+��� -   $ btnSaveEdit��$
��Save��$��5q�$
��10,3q��� -   $ btnCancelEdit��$��Cancel��$��5q�$
��10,3q�