using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ЭМК.Model;

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для MedicalCaseWindow.xaml
	/// </summary>
	public partial class MedicalCaseWindow : Window
	{
		// Коллекция для хранения периодов
		private ObservableCollection<object> _dateRanges = new ObservableCollection<object>();

		private diplomEntities dbContext = new diplomEntities();

		//Коллекция доступных жалоб
		public ObservableCollection<string> AvailableComplaints { get; } = new ObservableCollection<string>
		{
			"Активно жалоб не предъявляет",
			"Боли в грудной клетке",
			"Головные боли",
			"Головокружение",
			"Одышка",
			"Кашель",
			"Тошнота",
			"Слабость",
			"Повышенная температура",
			"Насморк",
			"Боль в горле",
			"Боль в животе"
		};

		// Коллекция выбранных жалоб
		private ObservableCollection<string> _selectedComplaints = new ObservableCollection<string>();

		public MedicalCaseWindow(MedicalCase medicalCase)
		{
			InitializeComponent();

			LoadMedicalCaseData(medicalCase);
			LoadDiagnosesData();
			LoadDoctorInfo();

			// Запускаем асинхронную операцию без ожидания
			_ = NumberVoidAsync();

			this.DataContext = this;

			var today = DateTime.Now;
			dateTextBlock.Text = today.ToString("dd.MM.yyyy");

			// Инициализация ComboBox
			ComplaintsComboBox.ItemsSource = AvailableComplaints;
		}

		public async Task NumberVoidAsync()
		{
			try
			{
				// Создаем новый контекст для этой операции
				using (var localDbContext = new diplomEntities())
				{
					int inspectionCount = await localDbContext.inspection.CountAsync();
					NumberTextBlock.Text = inspectionCount.ToString();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при получении количества осмотров: {ex.Message}");
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
					txtDoctorInfoTwo.Text = doctorInfo;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке врачач: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);

			// Получаем текущую дату и время
			DateTime now = DateTime.Now;

			// Создаем DateTime с текущей датой и временем
			timePicker.SelectedTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
		}

		private void TimeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			// Разрешаем ввод только цифр и двоеточия
			foreach (char c in e.Text)
			{
				if (!char.IsDigit(c) && c != ':')
				{
					e.Handled = true;
					return;
				}
			}

			// Проверяем, что двоеточие вводится только один раз
			if (e.Text == ":" && ((TextBox)sender).Text.Contains(":"))
			{
				e.Handled = true;
			}
		}

		// Вспомогательный метод для поиска элементов
		public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
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

		// Загрузка данных
		private ObservableCollection<MkbDiagnosis> _diagnoses;

		// В конструкторе или методе загрузки окна
		private void LoadDiagnosesData()
		{
			_diagnoses = new ObservableCollection<MkbDiagnosis>
			{
				new MkbDiagnosis { Code = "A00", Name = "Холера" },
				new MkbDiagnosis { Code = "A01", Name = "Брюшной тиф и паратифы" },
				new MkbDiagnosis { Code = "A02", Name = "Другие сальмонеллезные инфекции" },

				// Инфекционные болезни
				new MkbDiagnosis { Code = "A09", Name = "Другие гастроэнтериты и колиты инфекционного происхождения" },
				new MkbDiagnosis { Code = "J18", Name = "Пневмония без уточнения возбудителя" },
				new MkbDiagnosis { Code = "J06", Name = "Острые инфекции верхних дыхательных путей" },
    
				// Сердечно-сосудистые
				new MkbDiagnosis { Code = "I10", Name = "Эссенциальная (первичная) гипертензия" },
				new MkbDiagnosis { Code = "I25", Name = "Хроническая ишемическая болезнь сердца" },
    
				// Неврологические
				new MkbDiagnosis { Code = "G40", Name = "Эпилепсия" },
				new MkbDiagnosis { Code = "M54", Name = "Дорсалгия" },
    
				// Желудочно-кишечные
				new MkbDiagnosis { Code = "K21", Name = "Гастроэзофагеальная рефлюксная болезнь" },
				new MkbDiagnosis { Code = "K57", Name = "Дивертикулярная болезнь кишечника" },
    
				// Эндокринные
				new MkbDiagnosis { Code = "E11", Name = "Сахарный диабет 2 типа" },
				new MkbDiagnosis { Code = "E78", Name = "Нарушения обмена липопротеинов" },
    
				// Травмы
				new MkbDiagnosis { Code = "S72", Name = "Перелом бедренной кости" },
				new MkbDiagnosis { Code = "S42", Name = "Перелом плечевой кости" },
    
				// Другие распространённые
				new MkbDiagnosis { Code = "J45", Name = "Астма" },
				new MkbDiagnosis { Code = "N39", Name = "Другие болезни мочевой системы" },
				new MkbDiagnosis { Code = "M17", Name = "Гонартроз [артроз коленного сустава]" },
				new MkbDiagnosis { Code = "H25", Name = "Старческая катаракта" },
				new MkbDiagnosis { Code = "F32", Name = "Депрессивный эпизод" },
    
				// COVID-19
				new MkbDiagnosis { Code = "U07.1", Name = "COVID-19, вирус идентифицирован" },
				new MkbDiagnosis { Code = "U07.2", Name = "COVID-19, вирус не идентифицирован" },
    
				// Профилактические
				new MkbDiagnosis { Code = "Z23", Name = "Необходимость иммунизации против одной бактериальной болезни" },
				new MkbDiagnosis { Code = "Z00", Name = "Общее обследование и исследование лиц без жалоб" }
			};

			cbPreliminaryDiagnosis.ItemsSource = _diagnoses;
			cbMainDiagnosis.ItemsSource = _diagnoses;
		}

		// Обработчик поиска для обоих ComboBox
		private void DiagnosisComboBox_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			var comboBox = sender as ComboBox;
			if (comboBox == null) return;

			comboBox.IsDropDownOpen = true;

			var searchText = comboBox.Text;
			var collectionView = CollectionViewSource.GetDefaultView(comboBox.ItemsSource);
			if (collectionView == null) return;

			collectionView.Filter = item =>
			{
				if (string.IsNullOrEmpty(searchText)) return true;

				var diagnosis = item as MkbDiagnosis;
				if (diagnosis == null) return false;

				return diagnosis.Code.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
					   diagnosis.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
			};
		}

		//Метод получения выбранных диагнозов
		public SelectedDiagnoses GetSelectedDiagnoses()
		{
			return new SelectedDiagnoses
			{
				PreliminaryDiagnosis = cbPreliminaryDiagnosis.SelectedItem as MkbDiagnosis,
				MainDiagnosis = cbMainDiagnosis.SelectedItem as MkbDiagnosis
			};
		}

		private void LoadMedicalCaseData(MedicalCase medicalCase)
		{
			// Заполняем данные пациента
			txtPatientName.Text = medicalCase.PatientName;
			txtPatientBirthDate.Text = medicalCase.PatientBirthDate;
			txtPatientEnp.Text = medicalCase.PatientEnp;
			txtPatientSnils.Text = medicalCase.PatientSnils;
			txtPatientAge.Text = medicalCase.PatientAge;
			IDPatientTextBlock.Text = medicalCase.IdPatient.ToString();
		}

		// Текущая вводимая жалоба
		private string _currentComplaint;
		public string CurrentComplaint
		{
			get => _currentComplaint;
			set => _currentComplaint = value;
		}

		// Добавление жалобы
		private void AddComplaintButton_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(CurrentComplaint))
			{
				// Добавляем только если такой жалобы еще нет
				if (!_selectedComplaints.Contains(CurrentComplaint))
				{
					_selectedComplaints.Add(CurrentComplaint);

					// Обновляем текст с разделителями
					ComplaintsTextBox.Text = string.Join(", ", _selectedComplaints);

					// Добавляем в доступные, если это новая жалоба
					if (!AvailableComplaints.Contains(CurrentComplaint))
					{
						AvailableComplaints.Add(CurrentComplaint);
					}
				}

				// Очищаем поле ввода
				CurrentComplaint = string.Empty;
				ComplaintsComboBox.Text = string.Empty;
				ComplaintsComboBox.Focus();
			}
		}

		// Получение всех выбранных жалоб
		public string GetSelectedComplaints()
		{
			return ComplaintsTextBox.Text;
		}

		private void HeightUp_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtHeight.Text, out int height))
			{
				txtHeight.Text = (height + 1).ToString();
			}
		}

		private void HeightDown_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtHeight.Text, out int height) && height > 0)
			{
				txtHeight.Text = (height - 1).ToString();
			}
		}

		private void TxtHeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !char.IsDigit(e.Text, 0);
		}

		private void WeightUp_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtWeight.Text, out int height))
			{
				txtWeight.Text = (height + 1).ToString();
			}
		}

		private void WeightDown_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtWeight.Text, out int height) && height > 0)
			{
				txtWeight.Text = (height - 1).ToString();
			}
		}

		private void BloodPressureUpperUp_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtBloodPressureUpper.Text, out int height))
			{
				txtBloodPressureUpper.Text = (height + 1).ToString();
			}
		}

		private void BloodPressureUpperDown_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtBloodPressureUpper.Text, out int height) && height > 0)
			{
				txtBloodPressureUpper.Text = (height - 1).ToString();
			}
		}

		private void BloodPressureLowerUp_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtBloodPressureLower.Text, out int height))
			{
				txtBloodPressureLower.Text = (height + 1).ToString();
			}
		}

		private void BloodPressureLowerDown_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtBloodPressureLower.Text, out int height) && height > 0)
			{
				txtBloodPressureLower.Text = (height - 1).ToString();
			}
		}

		private void HeartRateUp_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtHeartRate.Text, out int height))
			{
				txtHeartRate.Text = (height + 1).ToString();
			}
		}

		private void HeartRateDown_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtHeartRate.Text, out int height) && height > 0)
			{
				txtHeartRate.Text = (height - 1).ToString();
			}
		}

		private void RespiratoryRateUp_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtRespiratoryRate.Text, out int height))
			{
				txtRespiratoryRate.Text = (height + 1).ToString();
			}
		}

		private void RespiratoryRateDown_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtRespiratoryRate.Text, out int height) && height > 0)
			{
				txtRespiratoryRate.Text = (height - 1).ToString();
			}
		}

		private void OxygenSaturationUp_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtOxygenSaturation.Text, out int height))
			{
				txtOxygenSaturation.Text = (height + 1).ToString();
			}
		}

		private void OxygenSaturationDown_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtOxygenSaturation.Text, out int height) && height > 0)
			{
				txtOxygenSaturation.Text = (height - 1).ToString();
			}
		}


		private void TemperatureUp_Click(object sender, RoutedEventArgs e)
		{
			if (double.TryParse(txtTemperature.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double temp))
			{
				temp = Math.Round(temp + 0.1, 1); // Увеличиваем на 0.1 и округляем
				txtTemperature.Text = temp.ToString("0.0", CultureInfo.InvariantCulture);
			}
			else
			{
				txtTemperature.Text = "36.6"; // Значение по умолчанию при ошибке
			}
		}

		private void TemperatureDown_Click(object sender, RoutedEventArgs e)
		{
			if (double.TryParse(txtTemperature.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double temp))
			{
				temp = Math.Round(temp - 0.1, 1); // Уменьшаем на 0.1 и округляем
				txtTemperature.Text = temp.ToString("0.0", CultureInfo.InvariantCulture);
			}
			else
			{
				txtTemperature.Text = "36.6"; // Значение по умолчанию при ошибке
			}
		}

		private void TxtTemperature_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			var textBox = sender as TextBox;
			string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);

			// Разрешаем только цифры и одну точку
			e.Handled = !Regex.IsMatch(newText, @"^[0-9]*\.?[0-9]{0,1}$");
		}

		private void TxtTemperature_TextChanged(object sender, TextChangedEventArgs e)
		{
			// Автоматически добавляем ".0" если пользователь ввел целое число
			if (txtTemperature.Text.Contains(".") && !txtTemperature.Text.EndsWith("."))
			{
				return;
			}

			if (double.TryParse(txtTemperature.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
			{
				if (!txtTemperature.Text.Contains("."))
				{
					txtTemperature.Text += ".0";
					txtTemperature.CaretIndex = txtTemperature.Text.Length;
				}
			}
		}

		private void CompleteCaseButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// Собираем данные с формы
				var newInspection = CollectFormData();

				// Сохраняем в базу данных
				SaveMedicalCaseToDatabase(newInspection);

				MessageBox.Show("Случай успешно сохранен", "Успех",
							  MessageBoxButton.OK, MessageBoxImage.Information);

				this.DialogResult = true;
				this.Close(); // Закрываем окно после сохранения
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
							  MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private NewInspection CollectFormData()
		{
			try
			{
				// Собираем все данные с формы
				var newInspection = new NewInspection
				{
					// Данные пациента
					IdPatient = Convert.ToInt32(IDPatientTextBlock.Text),

					// Данные врача
					IdDoctor = App.CurrentDoctor.id_doctor,

					// Данные приема
					Date = dateTextBlock.Text,
					Time = timePicker.SelectedTime?.ToString("HH:mm:ss") ?? string.Empty,

					// Место обслуживания
					PlaceOfService = ((ComboBoxItem)((ComboBox)FindName("cmbPlaceOfService")).SelectedItem)?.Content.ToString(),
					TypeOfServiceCase = ((ComboBoxItem)((ComboBox)FindName("cmbServiceType")).SelectedItem)?.Content.ToString(),
					TypeOfPayment = ((ComboBoxItem)((ComboBox)FindName("cmbTypeOfPayment")).SelectedItem)?.Content.ToString(),
					PurposeOfTheService = ((ComboBoxItem)((ComboBox)FindName("cmbServicePurpose")).SelectedItem)?.Content.ToString(),

					// Осмотр
					Complaints = ComplaintsTextBox.Text,
					MedicalHistory = tbMedicalHistory.Text + tbMedicalHistory1.Text + tbMedicalHistory2.Text + tbMedicalHistory3.Text,

					// Физические параметры
					Height = int.TryParse(txtHeight.Text, out int height) ? height : 0,
					Weight = int.TryParse(txtWeight.Text, out int weight) ? weight : 0,
					BloodPressureUpper = int.TryParse(txtBloodPressureUpper.Text, out int bpUpper) ? bpUpper : 0,
					BloodPressureLower = int.TryParse(txtBloodPressureLower.Text, out int bpLower) ? bpLower : 0,
					Temperature = double.TryParse(txtTemperature.Text, out double temp) ? temp : 0,
					HeartRate = int.TryParse(txtHeartRate.Text, out int hr) ? hr : 0,
					RespiratoryRate = int.TryParse(txtRespiratoryRate.Text, out int rr) ? rr : 0,
					OxygenSaturation = int.TryParse(txtOxygenSaturation.Text, out int spo2) ? spo2 : 0,

					SuspicionOfHeat = ZnoCheckBox.IsChecked == true ? "Да" : "Нет",
					PatientCondition = ((ComboBoxItem)((ComboBox)FindName("cmbPatientCondition")).SelectedItem)?.Content.ToString(),
					PreliminaryDiagnosis = (cbPreliminaryDiagnosis.SelectedItem as MkbDiagnosis)?.Name,
					TheMainDiagnosis = (cbMainDiagnosis.SelectedItem as MkbDiagnosis)?.Name,
					Treatment = tbTreatment.Text,
					Recommendations = tbRecommendations.Text,
				};

				return newInspection;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return null;
			}
		}

		private void SaveMedicalCaseToDatabase(NewInspection newInspection)
		{
			try
			{
				// Создаем новую запись осмотра
				var inspection = new inspection
				{
					id_patient = newInspection.IdPatient,
					id_doctor = App.CurrentDoctor.id_doctor,
					date_inspection = DateTime.ParseExact(newInspection.Date, "dd.MM.yyyy", CultureInfo.InvariantCulture),
					time_inspection = TimeSpan.Parse(newInspection.Time),

					// Физические параметры
					height = newInspection.Height,
					weight = newInspection.Weight,
					blood_pressure_upper = newInspection.BloodPressureUpper,
					blood_pressure_lower = newInspection.BloodPressureLower,
					temperature = newInspection.Temperature,
					heart_rate = newInspection.HeartRate,
					respiratory_rate = newInspection.RespiratoryRate,
					oxygen_saturation = newInspection.OxygenSaturation,

					// Диагнозы и лечение
					complaints = newInspection.Complaints,
					medical_history = newInspection.MedicalHistory,
					preliminary_diagnosis = newInspection.PreliminaryDiagnosis,
					the_main_diagnosis = newInspection.TheMainDiagnosis,
					patient_condition = newInspection.PatientCondition,
					suspicion_of_heat = newInspection.SuspicionOfHeat,
					treatment = newInspection.Treatment,
					recommendations = newInspection.Recommendations,

					// Дополнительные параметры
					place_of_service = newInspection.PlaceOfService,
					type_of_service_case = newInspection.TypeOfServiceCase,
					type_of_payment = newInspection.TypeOfPayment,
					purpose_of_the_service = newInspection.PurposeOfTheService,
				};

				dbContext.inspection.Add(inspection);
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
							  MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}