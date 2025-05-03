using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			LoadDoctorInfo();
		}

		private void LoadDoctorInfo()
		{
			if (App.CurrentDoctor != null)
			{
				txtDoctorInfo.Text = $"Добро пожаловать, {App.CurrentDoctor.lastname} {App.CurrentDoctor.name}";

				// Если есть отчество, можно добавить и его
				if (!string.IsNullOrEmpty(App.CurrentDoctor.surname))
				{
					txtDoctorInfo.Text += $" {App.CurrentDoctor.surname}";
				}
			}
		}

		private void Next_Click(object sender, RoutedEventArgs e)
		{
			Window1 window1 = new Window1();
			window1.Show();
			this.Close();
		}
    }
}