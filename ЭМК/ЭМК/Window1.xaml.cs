using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ЭМК.Model;

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		private diplomEntities dbContext;
		private List<Patient> allPatients;

		public Window1()
		{
			InitializeComponent();
			DataContext = this;
			dbContext = new diplomEntities();

			//LoadPatients();
			AddEnterKeyHandlerToTextBoxes();
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
					if (App.CurrentDoctor.specialization > 0)
					{
						var specialization = await dbContext.specialization
							.Where(s => s.id_specialization == App.CurrentDoctor.specialization)
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

			LoadPatients();
		}

		private async void LoadPatients()
		{
			try
			{
				// Загружаем всех пациентов из базы данных
				allPatients = await dbContext.patient
					.Select(p => new Patient
					{
						Id = p.id_patient,
						lastname = p.lastname,
						name = p.name,
						surname = p.surname,
						birthday = p.birthday,
						age = p.age,
						gender = p.gender,
						number_policy_OMS = p.number_policy_OMS,
						Snils = p.snils,
						Phone = p.phone,
						Address = p.address,
						HospitalName = dbContext.attachment
							.Where(a => a.id_patient == p.id_patient)
							.Join(dbContext.hospital,
								a => a.id_hospital,
								h => h.id_hospital,
								(a, h) => h.name)
							.FirstOrDefault()
					})
					.ToListAsync();

				// Применяем фильтр (изначально без фильтра)
				ApplyFilter();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке пациентов: {ex.Message}",
							  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ApplyFilter()
		{
			if (allPatients == null) return;

			var filteredPatients = allPatients.AsQueryable();

			// Фильтр по ФИО
			if (FioCheckBox.IsChecked == true && !string.IsNullOrWhiteSpace(FioTextBox.Text))
			{
				var searchText = FioTextBox.Text.ToLower();
				filteredPatients = filteredPatients.Where(p =>
					p.lastname.ToLower().Contains(searchText) ||
					p.name.ToLower().Contains(searchText) ||
					(p.surname != null && p.surname.ToLower().Contains(searchText)));
			}

			// Фильтр по СНИЛС
			if (SnilsCheckBox.IsChecked == true && !string.IsNullOrWhiteSpace(SnilsTextBox.Text))
			{
				var searchText = SnilsTextBox.Text;
				filteredPatients = filteredPatients.Where(p =>
					p.Snils.Contains(searchText));
			}

			if (PolCheckBox.IsChecked == true && !string.IsNullOrWhiteSpace(PolTextBox.Text))
			{
				var searchText = PolTextBox.Text;
				filteredPatients = filteredPatients.Where(p =>
					p.Age.Contains(searchText));
			}

			// Фильтр по полису
			if (EnpCheckBox.IsChecked == true && !string.IsNullOrWhiteSpace(EnpTextBox.Text))
			{
				var searchText = EnpTextBox.Text;
				filteredPatients = filteredPatients.Where(p =>
					p.Policy.Contains(searchText));
			}

			// Фильтр по телефону
			if (PhoneCheckBox.IsChecked == true && !string.IsNullOrWhiteSpace(PhoneTextBox.Text))
			{
				var searchText = PhoneTextBox.Text;
				filteredPatients = filteredPatients.Where(p =>
					p.Phone != null && p.Phone.Contains(searchText));
			}

			// Фильтр по дате рождения
			if (BirthDateCheckBox.IsChecked == true && BirthDatePicker.SelectedDate != null)
			{
				var selectedDate = BirthDatePicker.SelectedDate.Value;
				filteredPatients = filteredPatients.Where(p =>
					p.birthday.Year == selectedDate.Year &&
					p.birthday.Month == selectedDate.Month &&
					p.birthday.Day == selectedDate.Day);
			}

			// Применяем фильтр к DataGrid
			DGPatients.ItemsSource = filteredPatients.ToList();
		}

		private void AddEnterKeyHandlerToTextBoxes()
		{
			// Добавляем обработчик нажатия Enter для всех TextBox
			foreach (var textBox in FindVisualChildren<TextBox>(this))
			{
				textBox.KeyDown += (sender, e) =>
				{
					if (e.Key == Key.Enter)
					{
						ApplyFilter();
					}
				};
			}
		}

		// Вспомогательный метод для поиска всех TextBox в окне
		private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
		{
			if (depObj != null)
			{
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
					if (child != null && child is T)
					{
						yield return (T)child;
					}

					foreach (T childOfChild in FindVisualChildren<T>(child))
					{
						yield return childOfChild;
					}
				}
			}
		}

		// Обработчик кнопки "Найти"
		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			ApplyFilter();
		}

		// Обработчик кнопки "Сбросить"
		private void ResetButton_Click(object sender, RoutedEventArgs e)
		{
			// Сбрасываем все чекбоксы и текстовые поля
			FioCheckBox.IsChecked = false;
			SnilsCheckBox.IsChecked = false;
			EnpCheckBox.IsChecked = false;
			PhoneCheckBox.IsChecked = false;
			BirthDateCheckBox.IsChecked = false;
			PolCheckBox.IsChecked = false;

			FioTextBox.Text = "";
			SnilsTextBox.Text = "";
			EnpTextBox.Text = "";
			PhoneTextBox.Text = "";
			PolTextBox.Text = "";
			BirthDatePicker.SelectedDate = null;

			// Показываем всех пациентов
			DGPatients.ItemsSource = allPatients;
		}

		private void PatientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (DGPatients.SelectedItem is Patient selectedPatient)
			{
				Window2 patientDetailsWindow = new Window2(selectedPatient);
				patientDetailsWindow.Show();
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
			this.Hide();
		}
	}
}
