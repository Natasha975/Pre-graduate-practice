using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using ЭМК.Model;

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MedCardDBEntities dbContext;
		public MainWindow()
		{
			InitializeComponent();

			dbContext = new MedCardDBEntities();

			LoadDoctorInfo();
		}

		private async void LoadDoctorInfo()
		{
			try
			{
				if (App.CurrentDoctor != null)
				{
					// Создаем базовую строку с ФИО
					string doctorInfo = $"{App.CurrentDoctor.lastname} {App.CurrentDoctor.name}";

					// Добавляем отчество, если оно есть
					if (!string.IsNullOrEmpty(App.CurrentDoctor.surname))
					{
						doctorInfo += $" {App.CurrentDoctor.surname}";
					}

					// Загружаем специальность
					if (App.CurrentDoctor.id_specialization > 0)
					{
						var specialization = await dbContext.specialization
							.Where(s => s.id_specialization == App.CurrentDoctor.id_specialization)
							.FirstOrDefaultAsync();

						if (specialization != null)
						{
							// Добавляем специальность в скобках
							doctorInfo += $" ({specialization.name})";
						}
					}

					// Устанавливаем текст в Label
					txtDoctorInfo.Text = doctorInfo;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке врачач: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
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