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

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для PatientPage.xaml
	/// </summary>
	public partial class PatientPage : Page
	{
		//private diplomEntities db = new diplomEntities();

		//private diplomEntities dbContext;
		//private List<Patients> allPatients;

		private readonly MedCardDBEntities _dbContext;
		private List<Patients> _allPatients;
		private List<Patients> _filteredPatients;

		public PatientPage()
		{
			InitializeComponent();

			_dbContext = new MedCardDBEntities();
			Loaded += async (s, e) => await LoadPatientsAsync();
		}

		private async Task LoadPatientsAsync()
		{
			try
			{
				var patients = await _dbContext.patient
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
						HospitalName = _dbContext.attachment
							.Where(a => a.id_patient == p.id_patient)
							.Join(_dbContext.hospital,
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
				MessageBox.Show($"Ошибка загрузки пациентов: {ex.Message}",
					"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
			if (PatientsGrid.SelectedItem is Patients selectedPatient)
			{
				OpenPatientDetails(selectedPatient);
			}
		}

		private void AddPatient_Click(object sender, RoutedEventArgs e)
		{
			//var addWindow = new AddEditPatientWindow(_dbContext)
			//{
			//	Owner = Window.GetWindow(this)
			//};

			//if (addWindow.ShowDialog() == true)
			//{
			//	_ = LoadPatientsAsync(); // Обновляем список после добавления
			//}
		}

		private void OpenPatientDetails(Patients patient)
		{
			//var detailsWindow = new PatientDetailsWindow(_dbContext, patient.Id)
			//{
			//	Owner = Window.GetWindow(this)
			//};
			//detailsWindow.ShowDialog();
		}
	}	
}
