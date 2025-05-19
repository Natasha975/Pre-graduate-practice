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
using ЭМК.Model;

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для SickLeaveFormPage.xaml
	/// </summary>
	public partial class SickLeaveFormPage : Page
	{
		private int _periodCounter = 1;

		private int? _inspectionId;

		public SickLeaveFormPage(string patientName, string patientBirthDate, int? inspectionId)
		{
			InitializeComponent();
			LoadInitialData();
			_inspectionId=inspectionId;
		}

		private void LoadInitialData()
		{
			AddNewPeriod(DateTime.Now, DateTime.Now.AddDays(7), "Иванов И.И.");
		}

		private void AddPeriodButton_Click(object sender, RoutedEventArgs e)
		{
			// Добавляем новый период с датами по умолчанию
			AddNewPeriod(DateTime.Now, DateTime.Now.AddDays(7), "Иванов И.И.");
		}

		private void AddNewPeriod(DateTime startDate, DateTime endDate, string doctor)
		{
			PeriodsDataGrid.Items.Add(new SickLeavePeriod
			{
				Number = _periodCounter++,
				StartDate = startDate,
				EndDate = endDate,
				Doctor = doctor
			});
		}

		private void DeletePeriodButton_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button button && button.DataContext is SickLeavePeriod period)
			{
				PeriodsDataGrid.Items.Remove(period);
				// Перенумеруем оставшиеся периоды
				int counter = 1;
				foreach (SickLeavePeriod item in PeriodsDataGrid.Items)
				{
					item.Number = counter++;
				}
				PeriodsDataGrid.Items.Refresh();
			}
		}

		private void SaveSickLeave_Click(object sender, RoutedEventArgs e)
		{
			if (!_inspectionId.HasValue)
			{
				MessageBox.Show("Не найден осмотр. Сохраните осмотр перед созданием направления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			} 

			try
			{
				// Проверяем, что есть хотя бы один период
				if (PeriodsDataGrid.Items.Count == 0)
				{
					MessageBox.Show("Добавьте хотя бы один период нетрудоспособности", "Ошибка",
						MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}

				// Получаем выбранные значения из ComboBox
				var selectedType = (SickLeaveTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
				var selectedReason = (ReasonCodeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

				using (var db = new MedCardDBEntities())
				{
					var newCertificate = new disability_certificate
					{
						id_inspection = _inspectionId.Value,
						type_of_certificate = selectedType,
						cause_of_illness = selectedReason,
						period = PeriodsDataGrid.Items.Count
					};

					db.disability_certificate.Add(newCertificate);
					db.SaveChanges();

					// Теперь добавляем периоды
					foreach (SickLeavePeriod period in PeriodsDataGrid.Items)
					{
						var newPeriod = new period
						{
							id_disability_certificate = newCertificate.id_disability_certificate,
							date_start = period.StartDate,
							date_end = period.EndDate
						};
						db.period.Add(newPeriod);
					}

					// Сохраняем периоды
					db.SaveChanges();
				}

				MessageBox.Show("Листок нетрудоспособности сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

				// Возвращаемся к списку
				var parentWindow = Window.GetWindow(this) as MedicalCaseWindow;
				parentWindow?.ShowSickLeaveList();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void CancelSickLeave_Click(object sender, RoutedEventArgs e)
		{
			// Возвращаемся к списку
			var parentWindow = Window.GetWindow(this) as MedicalCaseWindow;
			parentWindow?.ShowSickLeaveList();
		}
	}

	public class SickLeavePeriod
	{
		public int Number { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Doctor { get; set; }
	}
}
