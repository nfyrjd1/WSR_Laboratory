   M S B A M L   `   `   `  ����I  EWSR_Laboratory, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:WSR_Laboratory.ViewWSR_Laboratory.View  5   	   $    WSR_Laboratory.View.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:WSR_Laboratory.View     5   	   -   5        $	��450��5   5   $	��800��6E     � Title$  Лаборатория��5   	   $	��450��6   $	��800��6*   .��5	      �  � RowDefinitions 5
   
   �� 5      $��80 �6   �� 5      $��1* �6    � ColumnDefinitions 5   
   �� 5      $	��150 �6   �� 5      �� 5      $
��Auto �6   .+�5   
   � ���#76E383$��3��6$   6:   �� 5   
   $��../Resources/logo.png��6   $��75��60   �� 5   
   $(��"Медлаборатория №20��6      Text#��[  6.   $��1��6L   $
��Auto��6\   $��Center=�6i   $��Center=�6�   $��30(�6�   �� 5   
   -   5         ��Name$
 Time��5   /   $��Center=�6   $
��Auto��6=   $��2��6J   #��[  6Z    � 5   
   -   5          �Name$ 	MainFrame��5   a     � NavigationUIVisibility$ Hidden=�6   $��1��60   $��1��6=   $��2��6M   6a   �� 5   
   $��1��6   .+�5      �� -   5         ��Name$ UserBtn��5   8   $��Пользователь��6   $	��135��6,   6I   �� 5      -   5        $ 
HistoryBtn��5   J   $��История��6   $y�	Collapsed=�6'   $	��135��6>   6^   �� 5      -   5        $ 	ReportBtn��5   I   $��Отчёты��6   $y�	Collapsed=�6&   $	��135��6=   6I   �� 5      -   5        $ ResourcesBtn��5   M   $��Расходники��6   $y�	Collapsed=�6*   $	��135��6A   6M   �� 5       -   5        $ BloodBtn��5    N   $��Биоматериал��6   $y�	Collapsed=�6+   $	��135��6B   6N   �� 5!      -	   5        $ AnalyzerBtn��5!   N   $��Анализаторы��6   $y�	Collapsed=�6+   $	��135��6B   6N   �� 5"      -
   5        $ InsuranceBtn��5"   H   $��
Счета��6   $y�	Collapsed=�6%   $	��135��6<   6H   �� 5#      -   5        $ ExitBtn��5#   1   $��
Выйти��6   $	��135��6%   6B   5$      5%      5&      