   M S B A M L   `   `   `  ����C  ?QangaJeu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:LauncherQangaClientLauncherQangaClient  5   	   $    LauncherQangaClient.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:LauncherQangaClient     5   	   -   5          � Title$  Qanga��5   	   $��474.196��6   $��875.059��6(   ��5	      �� 5
   
    j� Stretch$ Uniform=�6    �� ImageSource$ image/Background.png��6'   .��5      � -   5        $��	0,0,-8,-7q�5       � ColumnDefinitions 5   
   �� 5      $	��98* �6   �� 5      $
��294* �6   �� 5      $
��125* �6   .+�5   
   �� -   5         ��Name$ BtnStart��5      $��Lancer le jeu��6#   $
��Left=�6;   $��101,397,0,0q�6V   $	��Top=�6k   $��74��6�   $��2��6�   {�.  6�   � 5   
   -   5         �Name$ progressabar1��5      $
��Left=�6-   $��18��6H   $��153,397,0,0q�6T   $	��Top=�6i   $	��408��6�   $��2��6�   $��1��6�   � 5   
   -   5        $ progressabar2��5      $
��Left=�6-   $��14��6H   $��
153,22,0,0q�6T   $	��Top=�6h   $	��408��6�   $��2��6�   $��1��6�   �� 5   
   -   5         ��Name$ LbTéléchargement��5      $�� ��6*   $��1��65   $
��Left=�6E   $��248,397,0,0q�6`   $	��Top=�6u   $��22��6�    J� RenderTransformOrigin$ 0.628,0.324&�6�   $��11(�6�   $	��218��6�   �� 5   
   -   5        $ 	operation��5      $�� ��6#   $��1��6.   $
��Left=�6>   $��
52,161,0,0q�6Y   $	��Top=�6m   $	��241��6�   (  "System.Windows.Controls.WebBrowser  5   
   $��1��6   $
��Left=�6%   $	��286��6@   $��	58,87,0,0q�6M   $	��Top=�6`   $	��615��6x   $��2��6�    J� OpacityMask �	#00000000	   Source	 6�   Z� 5      5      