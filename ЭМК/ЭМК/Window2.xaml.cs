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
		private MedCardDBEntities dbContext;
		private Patients currentPatient;
		private List<InspectionView> allInspections; // Храним все осмотры
		private List<InspectionView> filteredInspections; // Храним отфильтрованные осмотры

		public Window2(Patients patient)
		{
			InitializeComponent();

			dbContext = new MedCardDBEntities();
			currentPatient = patient;

			// Устанавливаем сегодняшнюю дату по умолчанию
			FilterDatePicker.SelectedDate = DateTime.Today;
			FilterDatePicker.DisplayDateEnd = DateTime.Today;

			LoadDoctorInfo();

			// Устанавливаем сегодняшнюю дату по умолчанию
			FilterDatePicker.SelectedDate = DateTime.Today;
			FilterDatePicker.DisplayDateEnd = DateTime.Today;

			InitializeReportDates(); // Добавьте эту строку
		}

		private void InitializeReportDates()
		{
			// Устанавливаем даты для отчета: текущий месяц
			ReportStartDate.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
			ReportEndDate.SelectedDate = DateTime.Today;
			ReportEndDate.DisplayDateEnd = DateTime.Today;
		}

		private async void GenerateReport_Click(object sender, RoutedEventArgs e)
		{
			if (!ReportStartDate.SelectedDate.HasValue || !ReportEndDate.SelectedDate.HasValue)
			{
				MessageBox.Show("Выберите период для отчета", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			var startDate = ReportStartDate.SelectedDate.Value;
			var endDate = ReportEndDate.SelectedDate.Value;

			if (startDate > endDate)
			{
				MessageBox.Show("Дата начала не может быть позже даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			await GenerateAttendanceReport(startDate, endDate);
		}

		private async Task GenerateAttendanceReport(DateTime startDate, DateTime endDate)
		{
			try
			{
				var visits = await dbContext.inspection
					.Where(i => i.id_patient == currentPatient.Id &&
								i.date_inspection >= startDate &&
								i.date_inspection <= endDate)
					.GroupBy(i => DbFunctions.TruncateTime(i.date_inspection))
					.Select(g => new { Date = g.Key, Count = g.Count() })
					.OrderBy(x => x.Date)
					.ToListAsync();

				// Подготовка данных для диаграммы
				var dates = new List<DateTime>();
				var counts = new List<int>();

				// Заполняем все даты в диапазоне (даже если посещений не было)
				for (var date = startDate; date <= endDate; date = date.AddDays(1))
				{
					dates.Add(date);
					var visit = visits.FirstOrDefault(v => v.Date == date);
					counts.Add(visit?.Count ?? 0);
				}

				// Обновляем статистику
				UpdateReportStats(visits, startDate, endDate);

				// Рисуем диаграмму
				DrawAttendanceChart(dates, counts);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при формировании отчета: {ex.Message}",
							   "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void UpdateReportStats(IEnumerable<dynamic> visits, DateTime startDate, DateTime endDate)
		{
			int totalVisits = visits.Sum(v => (int)v.Count);
			int daysWithVisits = visits.Count();
			int totalDays = (endDate - startDate).Days + 1;

			string stats = $"Период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}\n" +
						   $"Всего посещений: {totalVisits}\n" +
						   $"Дней с посещениями: {daysWithVisits} из {totalDays}\n" +
						   $"Среднее посещений в день: {(daysWithVisits > 0 ? (double)totalVisits / daysWithVisits : 0):F1}";

			ReportStatsText.Text = stats;
		}

		private void DrawAttendanceChart(List<DateTime> dates, List<int> counts)
		{
			AttendanceChartCanvas.Children.Clear();

			if (dates == null || dates.Count == 0) return;

			const int margin = 30;
			const int columnWidth = 20;
			const int spacing = 5;
			double maxCount = counts.Max() > 0 ? counts.Max() : 1;
			// Фиксированная высота (можно взять из XAML или задать здесь)
			double canvasHeight = 300 - 2 * margin;

			// Ширина рассчитывается исходя из количества данных
			double canvasWidth = dates.Count * (columnWidth + spacing) + 2 * margin;

			// Устанавливаем ширину Canvas, чтобы ScrollViewer мог прокручивать
			AttendanceChartCanvas.Width = canvasWidth + 2 * margin;
			AttendanceChartCanvas.Height = canvasHeight + 2 * margin;

			// Оси
			var xAxis = new Line
			{
				X1 = margin,
				Y1 = margin + canvasHeight,
				X2 = margin + canvasWidth,
				Y2 = margin + canvasHeight,
				Stroke = Brushes.Black,
				StrokeThickness = 1
			};

			var yAxis = new Line
			{
				X1 = margin,
				Y1 = margin,
				X2 = margin,
				Y2 = margin + canvasHeight,
				Stroke = Brushes.Black,
				StrokeThickness = 1
			};

			AttendanceChartCanvas.Children.Add(xAxis);
			AttendanceChartCanvas.Children.Add(yAxis);

			// Подписи оси Y
			for (int i = 0; i <= maxCount; i++)
			{
				double y = margin + canvasHeight - (i / maxCount * canvasHeight);

				var label = new TextBlock
				{
					Text = i.ToString(),
					FontSize = 10,
					Foreground = Brushes.Black,
					TextAlignment = TextAlignment.Right,
					Width = 20
				};

				Canvas.SetLeft(label, margin - 25);
				Canvas.SetTop(label, y - 8);
				AttendanceChartCanvas.Children.Add(label);
			}

			// Столбцы диаграммы
			for (int i = 0; i < dates.Count; i++)
			{
				if (i * (columnWidth + spacing) > canvasWidth) break;

				double columnHeight = counts[i] / maxCount * canvasHeight;
				double left = margin + 5 + i * (columnWidth + spacing);
				double top = margin + canvasHeight - columnHeight;

				var column = new Rectangle
				{
					Width = columnWidth,
					Height = columnHeight,
					Fill = Brushes.SteelBlue,
					Stroke = Brushes.DarkSlateBlue,
					StrokeThickness = 1
				};

				Canvas.SetLeft(column, left);
				Canvas.SetTop(column, top);
				AttendanceChartCanvas.Children.Add(column);

				// Подпись количества
				var countLabel = new TextBlock
				{
					Text = counts[i].ToString(),
					FontSize = 10,
					Foreground = Brushes.Black
				};

				Canvas.SetLeft(countLabel, left + columnWidth / 2 - 5);
				Canvas.SetTop(countLabel, top - 20);
				AttendanceChartCanvas.Children.Add(countLabel);

				// Подпись даты
				var dateLabel = new TextBlock
				{
					Text = dates[i].ToString("dd.MM"),
					FontSize = 9,
					Foreground = Brushes.Black,
					LayoutTransform = new RotateTransform(-45)
				};

				Canvas.SetLeft(dateLabel, left - 5);
				Canvas.SetTop(dateLabel, margin + canvasHeight + 5);
				AttendanceChartCanvas.Children.Add(dateLabel);
			}
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
		}

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
