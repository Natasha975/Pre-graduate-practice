   M S B A M L   `   `   `  ����A  =ЭМК, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ЭМКЭМК  5   	       ЭМК.LoginWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:ЭМК     5   	     � Title$  ЭМК��5   	   $	��300��6   $	��400��6"    � Icon$$ Image/medical_monitor_icon.ico��6.    � WindowStartupLocation$ CenterScreen=�5	   	    � 
ResizeMode$ NoResize=�6.   .��5      � ���	#FFF0F0F0 � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   ���	#FF3F51B5 �� Padding$ 10q�65   ._�5      �� $ ��Вход в систему��6   
�������$��18(�6A   $��Center=�6O   5      �� 5   
   $��1��6   $��Center=�6"   $��Center=�6=   $	��300��6Z   .+�5      �� 
������� �� CornerRadius$ 5��6(   $ 20q�69   ���	#FFDDDDDD$��1q�6^   ._�5      �� .+�5      �� $��
Логин��6    $��0,0,0,5q�6-   �� 5      -   5         ��Name$ txtUsername��5      $��ivanov��63   $��30��6A   $��5q�6M   �� 5      $��Пароль��6    $��0,10,0,5q�6.   G� 5      -   5         G�Name$ txtPassword��5   "   	 G� Password$	 
Ivanov2024��67   $��30��6M   $��5q�6Y   �� 5!      -   5        
 ��Name$
 chkRemember��5!      $!��Запомнить меня��64   $��0,10,0,0q�6M   5"      5#      5$      �� 5'   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��10q�6W   .+�5(      �� -   5        $��Вход��5(      $	��100��6$   $��30��60   $��0,0,10,0q�6<   ���	#FF3F51B5
�������6x   �� 5)      -   5        $��Отмена��5)      $	��100��6&   $��30��62   
���   �
�������6v   5*      5+      5,      