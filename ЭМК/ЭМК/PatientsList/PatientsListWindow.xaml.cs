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
using System.Windows.Shapes;

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для PatientsListWindow.xaml
	/// </summary>
	public partial class PatientsListWindow : Window
	{
		private readonly bool _isReception;
		public PatientsListWindow(bool isReception = false)
		{
			InitializeComponent();
			_isReception = isReception;
		}

		// Обработчик нажатия кнопки просмотра пациентов
		private void PatientView_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new PatientPage(_isReception));
		}

		// Обработчик нажатия кнопки "Назад"
		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			if (MainFrame.CanGoBack)
			{
				MainFrame.GoBack();
			}
		}

		// Обработчик нажатия кнопки выхода
		private void ExitBt_Click(object sender, RoutedEventArgs e)
		{
			if (_isReception)
			{
				// Для регистратуры - открываем окно авторизации
				var loginWindow = new LoginWindow();
				loginWindow.Show();
			}
			else
			{
				// Для врачей - открываем главное окно (как было)
				var mainWindow = new MainWindow();
				mainWindow.Show();
			}
			this.Close();
		}
	}
}
