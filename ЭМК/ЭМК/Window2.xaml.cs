using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using ЭМК.Model;

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для Window2.xaml
	/// </summary>
	public partial class Window2 : Window
	{
		private diplomEntities dbContext;
		private Patient currentPatient;
		private List<InspectionView> allInspections; // Храним все осмотры
		private List<InspectionView> filteredInspections; // Храним отфильтрованные осмотры
		//private List<InspectionView> inspections;

		public Window2(Patient patient)
		{
			InitializeComponent();

			dbContext = new diplomEntities();
			currentPatient = patient;

			// Устанавливаем сегодняшнюю дату по умолчанию
			FilterDatePicker.SelectedDate = DateTime.Today;
			FilterDatePicker.DisplayDateEnd = DateTime.Today;

			//// Инициализация DatePicker
			//// FilterDatePicker.SelectedDate = DateTime.Today;
			//// Нельзя выбрать дату в будущем
			//FilterDatePicker.DisplayDateEnd = DateTime.Today; 

			LoadDoctorInfo();
		}

		private async Task LoadPatientDetails()
		{
			try
			{
				// Заполняем основную информацию
				txtFullName.Text = currentPatient.FullName;
				txtBirthDate.Text = currentPatient.BirthDate;
				txtAge.Text = currentPatient.Age;
				txtPol.Text = currentPatient.Pol; // Добавляем отображение пола
				txtSnils.Text = currentPatient.Snils;
				txtEnp.Text = currentPatient.Enp;
				txtPolicy.Text = currentPatient.Policy;
				txtPhone.Text = currentPatient.Phone;
				txtAddress.Text = currentPatient.Address ?? "Отсутствует";
				txtHospital.Text = currentPatient.HospitalName ?? "Отсутствует";

				await LoadInspections();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке данных пациента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private async Task LoadInspections()
		{
			try
			{
				var inspectionsData = await dbContext.inspection
									.Where(i => i.id_patient == currentPatient.Id)
									.Include(i => i.doctor)
									.OrderByDescending(i => i.date_inspection)
									.ThenByDescending(i => i.time_inspection)
									.ToListAsync();

				allInspections = inspectionsData.Select(i => new InspectionView
				{
					Id = i.id_inspection,
					Date = i.date_inspection.ToString("dd.MM.yyyy"),
					Time = i.time_inspection.ToString(@"hh\:mm"),
					DoctorName = $"{i.doctor.lastname} {i.doctor.name[0]}.{i.doctor.surname?[0]}.",
					Diagnosis = i.the_main_diagnosis ?? "Диагноз не указан",
					Inspection = i
				}).ToList();

				// По умолчанию показываем ВСЕ записи, несмотря на выбранную дату
				DocumentsListBox.ItemsSource = allInspections;
				UpdateStatusText();
			}
			//try
			//{
			//	var inspectionsData = await dbContext.inspection
			//						.Where(i => i.id_patient == currentPatient.Id)
			//						.Include(i => i.doctor)
			//						.OrderByDescending(i => i.date_inspection)
			//						.ThenByDescending(i => i.time_inspection)
			//						.ToListAsync();

			//	allInspections = inspectionsData.Select(i => new InspectionView
			//	{
			//		Id = i.id_inspection,
			//		Date = i.date_inspection.ToString("dd.MM.yyyy"),
			//		Time = i.time_inspection.ToString(@"hh\:mm"),
			//		DoctorName = $"{i.doctor.lastname} {i.doctor.name[0]}.{i.doctor.surname?[0]}.",
			//		Diagnosis = i.the_main_diagnosis ?? "Диагноз не указан",
			//		Inspection = i
			//	}).ToList();

			//	// Применяем фильтр по умолчанию (сегодняшняя дата)
			//	ApplyDateFilter();
			//}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке осмотров: {ex.Message}",
							  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ApplyDateFilter()
		{
			if (allInspections == null) return;

			if (FilterDatePicker.SelectedDate.HasValue)
			{
				var selectedDate = FilterDatePicker.SelectedDate.Value;
				filteredInspections = allInspections
					.Where(i => DateTime.ParseExact(i.Date, "dd.MM.yyyy", null).Date == selectedDate.Date)
					.ToList();

				// Показываем только отфильтрованные записи
				DocumentsListBox.ItemsSource = filteredInspections;
			}
			else
			{
				// Если дата не выбрана (например, после сброса), показываем все
				DocumentsListBox.ItemsSource = allInspections;
			}

			UpdateStatusText();
			//if (FilterDatePicker.SelectedDate.HasValue && allInspections != null)
			//{
			//	var selectedDate = FilterDatePicker.SelectedDate.Value;
			//	filteredInspections = allInspections
			//		.Where(i => DateTime.ParseExact(i.Date, "dd.MM.yyyy", null) == selectedDate)
			//		.ToList();

			//	DocumentsListBox.ItemsSource = filteredInspections;
			//	UpdateStatusText();
			//}
		}

		private void UpdateStatusText()
		{
			if (allInspections == null) return;

			if (FilterDatePicker.SelectedDate.HasValue && DocumentsListBox.ItemsSource == filteredInspections)
			{
				// Если выбрана дата и отображаются отфильтрованные записи
				StatusText.Text = $"Записи за {FilterDatePicker.SelectedDate.Value:dd.MM.yyyy}: {filteredInspections?.Count ?? 0} (всего: {allInspections.Count})";
			}
			else
			{
				// Если отображаются все записи (по умолчанию или после сброса)
				StatusText.Text = $"Всего записей: {allInspections.Count}";
			}

			//if (filteredInspections != null)
			//{
			//	StatusText.Text = $"Найдено записей: {filteredInspections.Count}";
			//}
		}

		private void FilterDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			ApplyDateFilter();
		}

		private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
		{
			// Устанавливаем сегодняшнюю дату, но показываем ВСЕ записи
			FilterDatePicker.SelectedDate = DateTime.Today;
			DocumentsListBox.ItemsSource = allInspections;
			UpdateStatusText();
			//	// Сбрасываем фильтр и показываем все записи
			//	FilterDatePicker.SelectedDate = null;
			//	DocumentsListBox.ItemsSource = allInspections;
			//	StatusText.Text = $"Всего записей: {allInspections?.Count ?? 0}";
		}

		//private async Task LoadInspections()
		//{
		//	try
		//	{
		//		var inspectionsData = await dbContext.inspection
		//							.Where(i => i.id_patient == currentPatient.Id)
		//							.Include(i => i.doctor)
		//							.OrderByDescending(i => i.date_inspection)
		//							.ThenByDescending(i => i.time_inspection)
		//							.ToListAsync();

		//		// Отладочный вывод (проверьте в Output -> Debug)
		//		Debug.WriteLine($"Найдено осмотров: {inspectionsData.Count}");

		//		if (inspectionsData.Count == 0)
		//		{
		//			MessageBox.Show("Осмотры не найдены для этого пациента.");
		//			return;
		//		}

		//		var inspections = inspectionsData.Select(i => new InspectionView
		//		{
		//			Date = i.date_inspection.ToString("dd.MM.yyyy"),
		//			Time = i.time_inspection.ToString(@"hh\:mm"),
		//			DoctorName = $"{i.doctor.lastname} {i.doctor.name[0]}.{i.doctor.surname?[0]}.",
		//			Diagnosis = i.the_main_diagnosis ?? "Диагноз не указан",
		//			Inspection = i
		//		}).ToList();

		//		DocumentsListBox.ItemsSource = inspections;
		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show($"Ошибка при загрузке осмотров: {ex.Message}",
		//					  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
		//	}
		//}

		private async void AddBt_Click(object sender, RoutedEventArgs e)
		{
			// Создаем новый медицинский случай с данными текущего пациента
			var medicalCase = new MedicalCase
			{
				IdPatient = currentPatient.Id,
				PatientName = txtFullName.Text,
				PatientBirthDate = txtBirthDate.Text,
				PatientEnp = txtEnp.Text,
				PatientSnils = txtSnils.Text,
				PatientAge = txtAge.Text,
			};

			// Открываем окно медицинского случая
			//var medicalCaseWindow = new MedicalCaseWindow(medicalCase);
			//medicalCaseWindow.Owner = this; // Устанавливаем владельца, чтобы окна были связаны
											//medicalCaseWindow.ShowDialog(); // ShowDialog для модального окна

			var medicalCaseWindow = new MedicalCaseWindow(medicalCase);
			medicalCaseWindow.Owner = this;

			if (medicalCaseWindow.ShowDialog() == true)
			{
				// Обновляем данные асинхронно
				await LoadInspections();
			}
		}

		private void DocumentsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (DocumentsListBox.SelectedItem is InspectionView selectedInspection)
			{
				// Создаем объект MedicalCase с данными из осмотра
				var medicalCase = new MedicalCase
				{
					IdPatient = currentPatient.Id,
					PatientName = txtFullName.Text,
					PatientBirthDate = txtBirthDate.Text,
					PatientAge = txtAge.Text,
					PatientEnp = txtEnp.Text,
					PatientSnils = txtSnils.Text,

					// Данные из осмотра
					IdInspection = selectedInspection.Inspection.id_inspection,
					Date = selectedInspection.Date,
					Time = selectedInspection.Time,
					DoctorName = selectedInspection.DoctorName,
					Diagnosis = selectedInspection.Diagnosis,

					PlaceOfService = selectedInspection.Inspection.place_of_service,
					TypeOfServiceCase = selectedInspection.Inspection.type_of_service_case,
					TypeOfPayment = selectedInspection.Inspection.type_of_payment,
					PurposeOfTheService = selectedInspection.Inspection.purpose_of_the_service,

					// Здесь добавьте другие данные из inspection, если они есть
					Height = selectedInspection.Inspection.height,
					Weight = selectedInspection.Inspection.weight,
					BloodPressureUpper =selectedInspection.Inspection.blood_pressure_upper,
					BloodPressureLower = selectedInspection.Inspection.blood_pressure_lower,
					Temperature = selectedInspection.Inspection.temperature,
					HeartRate = selectedInspection.Inspection.heart_rate,
					RespiratoryRate = selectedInspection.Inspection.respiratory_rate,
					OxygenSaturation = selectedInspection.Inspection.oxygen_saturation,

					PatientCondition = selectedInspection.Inspection.patient_condition,
					PreliminaryDiagnosis =selectedInspection.Inspection.preliminary_diagnosis,
					TheMainDiagnosis = selectedInspection.Inspection.the_main_diagnosis,

					SuspicionOfHeat = selectedInspection.Inspection.suspicion_of_heat,
					Complaints = selectedInspection.Inspection.complaints,
					Anamnesis = selectedInspection.Inspection.medical_history,
					Treatment = selectedInspection.Inspection.treatment,
					Recommendations = selectedInspection.Inspection.recommendations
				};

				// Открываем окно с данными осмотра
				MedicalCaseWindowReg caseWindowReg = new MedicalCaseWindowReg(medicalCase);
				caseWindowReg.Owner = this;
				caseWindowReg.ShowDialog();
			}
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

				await LoadPatientDetails();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке врача: {ex.Message}",
							  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}

	public class MedicalDocument
	{
		public string DateAndDescription { get; set; }
		public string Details { get; set; }
		public string DiagnosisCode { get; set; }
		public MedicalCase CaseData { get; set; } // Данные для передачи в MedicalCaseWindow
	}

	public class InspectionView
	{
		public int Id { get; set; }
		public string Date { get; set; }
		public string Time { get; set; }
		public string DoctorName { get; set; }
		public string Diagnosis { get; set; }
		public inspection Inspection { get; set; } // Ссылка на оригинальный объект

		// Вычисляемое свойство для заголовка
		public string DateAndDescription => $"{Date} {Time} - {DoctorName}";

		// Дополнительные детали для отображения в расширенном виде
		public string Details => $"Врач: {DoctorName}\nДиагноз: {Diagnosis}";

	}
}
