using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using ЭМК.Model;

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
		}

		// Обработчик кнопки "Список пациентов"
		private void PatientsList_Click(object sender, RoutedEventArgs e)
		{
			// Создаем и показываем окно со списком пациентов
			PatientsListWindow patientsListWindow = new PatientsListWindow();
			patientsListWindow.Show();
			this.Close();
		}

		// Обработчик кнопки "Прием пациентов"
		private void PatientAppointment_Click(object sender, RoutedEventArgs e)
		{
			// Создаем и показываем окно приема пациентов
			RecordedPatientsWindow patientsWindow = new RecordedPatientsWindow();
			patientsWindow.Show();
			this.Close();
		}

		// Обработчик кнопки "Выход"
		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			// Запрашиваем подтверждение перед выходом
			MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти из системы?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				// Закрываем приложение
				Application.Current.Shutdown();
			}
		}
	}
}