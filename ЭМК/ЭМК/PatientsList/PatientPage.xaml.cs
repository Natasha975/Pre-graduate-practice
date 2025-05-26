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
using System.Windows.Threading;
using ЭМК.Model;
using ЭМК.PatientsList;

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для PatientPage.xaml
	/// </summary>
	public partial class PatientPage : Page
	{
		private readonly MedCardDBEntities dbContext;
		private List<Patients> _allPatients;
		private List<Patients> _filteredPatients;
		private readonly bool _isReception;


		public PatientPage(bool isReception = false)
		{
			InitializeComponent();

			_isReception = isReception;
			dbContext = new MedCardDBEntities();
			ConfigureForReception();
			Loaded += async (s, e) => await LoadPatientsAsync();

			
		}

		private void ConfigureForReception()
		{
			if (_isReception)
			{
				// Создаем кнопку "Добавить пациента"
				var addButton = new Button
				{
					Content = "Добавить пациента",
					Width = 170,
					Margin = new Thickness(5),
					HorizontalAlignment = HorizontalAlignment.Left
				};
				addButton.Click += AddPatient_Click;

				// Добавляем кнопку в StackPanel с кнопками
				var buttonsPanel = (StackPanel)FindName("ButtonsPanel");
				buttonsPanel.Children.Insert(0, addButton);
			}
		}

		private async Task LoadPatientsAsync()
		{
			try
			{
				var patients = await dbContext.patient
					.OrderBy(p => p.lastname)
					.ThenBy(p => p.name)
					.Select(p => new Patients
					{
						Id = p.id_patient,
						lastname = p.lastname,
						name = p.name,
						surname = p.surname,
						age = p.age,
						BirthDateValue = p.birthday,
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

				_allPatients = patients;
				_filteredPatients = new List<Patients>(_allPatients);
				PatientsGrid.ItemsSource = _filteredPatients;
			}
			catch (Exception ex)
			{
				//MessageBox.Show($"Ошибка загрузки пациентов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				MessageBox.Show($"Возврат к списку пациентов", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void SearchPatients(string searchText)
		{
			if (string.IsNullOrWhiteSpace(searchText))
			{
				PatientsGrid.ItemsSource = _allPatients;
				return;
			}

			searchText = searchText.ToLower();

			_filteredPatients = _allPatients.Where(p =>
				(p.FullName?.ToLower().Contains(searchText) ?? false) ||
				(p.number_policy_OMS?.ToLower().Contains(searchText) ?? false) ||
				(p.Snils?.ToLower().Contains(searchText) ?? false) ||
				(p.Phone?.ToLower().Contains(searchText) ?? false) ||
				(p.Address?.ToLower().Contains(searchText) ?? false) ||
				(p.HospitalName?.ToLower().Contains(searchText) ?? false) ||
				(p.BirthDate.ToLower().Contains(searchText)) ||
				(p.Age.ToString().Contains(searchText))
			).ToList();

			PatientsGrid.ItemsSource = _filteredPatients;
		}

		private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			// Добавляем небольшую задержку, чтобы не искать на каждый символ
			var textBox = sender as TextBox;
			if (textBox == null) return;

			// Отменяем предыдущий таймер, если он был
			_searchTimer?.Stop();

			// Запускаем новый таймер
			_searchTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
			_searchTimer.Tick += (s, args) =>
			{
				_searchTimer.Stop();
				SearchPatients(textBox.Text);
			};
			_searchTimer.Start();
		}

		// Добавляем в класс поле для таймера
		private DispatcherTimer _searchTimer;

		private void ResetSearch_Click(object sender, RoutedEventArgs e)
		{
			SearchTextBox.Text = "";
			_searchTimer?.Stop();
			PatientsGrid.ItemsSource = _allPatients;
		}

		private async void RefreshList_Click(object sender, RoutedEventArgs e)
		{
			await LoadPatientsAsync();
		}

		private void PatientsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			//// Проверяем, что клик был по строке, а не по заголовку или пустому месту
			//if (e.OriginalSource is Visual visual)
			//{
			//	var row = FindParent<DataGridRow>(visual);
			//	if (row != null && row.Item is PatientView selectedPatient)
			//	{
			//		OpenEditPatientWindow(selectedPatient);
			//	}
			//}
			if (PatientsGrid.SelectedItem is Patients selectedPatient)
			{
				OpenPatientDetails(selectedPatient);
			}
		}
		private static T FindParent<T>(DependencyObject child) where T : DependencyObject
		{
			while (child != null && !(child is T))
			{
				child = VisualTreeHelper.GetParent(child);
			}
			return child as T;
		}

		private void AddPatient_Click(object sender, RoutedEventArgs e)
		{
			// Создаем нового пациента с пустыми полями
			var newPatient = new patient();

			// Открываем страницу редактирования
			if (Window.GetWindow(this) is PatientsListWindow registrarWindow)
			{
				registrarWindow.MainFrame.Navigate(new PatientDetailsPage(dbContext, newPatient, isNewPatient: true));
			}
		}

		private void OpenPatientDetails(Patients patient)
		{
			//var patientDetails = new PatientDetailsPage(dbContext, patient);
			//NavigationService.Navigate(patientDetails);
			var patientFromDb = dbContext.patient.Find(patient.Id);
			if (patientFromDb == null) return;

			if (Window.GetWindow(this) is PatientsListWindow registrarWindow)
			{
				// Подписываемся на событие возвращения
				registrarWindow.MainFrame.Navigated += (sender, args) =>
				{
					if (args.Content is PatientPage) // Если вернулись на эту страницу
					{
						_=LoadPatientsAsync(); // Обновляем данные
					}
				};

				registrarWindow.MainFrame.Navigate(new PatientDetailsPage(dbContext, patientFromDb));
			}
		}
	}	
}
