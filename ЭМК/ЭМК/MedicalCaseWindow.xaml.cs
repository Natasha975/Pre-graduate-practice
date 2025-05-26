using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ЭМК.Model;
using ЭМК.PrescriptionForms;

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для MedicalCaseWindow.xaml
	/// </summary>
	public partial class MedicalCaseWindow : Window
	{
		// Коллекция для хранения периодов
		private ObservableCollection<object> _dateRanges = new ObservableCollection<object>();

		private MedCardDBEntities dbContext = new MedCardDBEntities();

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

			// Добавляем первый период по умолчанию
			AddDateRange();
		}

		public async Task NumberVoidAsync()
		{
			try
			{
				// Создаем новый контекст для этой операции
				using (var localDbContext = new MedCardDBEntities())
				{
					int inspectionCount = await localDbContext.inspection.CountAsync()+1;
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
					txtDoctorInfoTwo.Text = doctorInfo;
					//txtDoctorInfoFree.Text = doctorInfo;
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
			//cbNDiagnosis.ItemsSource = _diagnoses;
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

			LoadDirections(medicalCase.Directions);
		}

		private void LoadDirections(List<Direction> directions)
		{
			// Очищаем существующий контент
			DirectionsListScrollViewer.Content = null;

			if (directions == null || directions.Count == 0)
			{
				var noDirectionsText = new TextBlock
				{
					Text = "Нет сохраненных направлений",
					Margin = new Thickness(10),
					FontStyle = FontStyles.Italic
				};
				DirectionsListScrollViewer.Content = noDirectionsText;
				return;
			}

			// Создаем таблицу для отображения направлений
			var grid = new Grid();
			grid.Margin = new Thickness(5);

			// Добавляем колонки
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });

			// Добавляем заголовки
			AddHeader(grid, "№", 0, 0);
			AddHeader(grid, "Тип направления", 1, 0);
			AddHeader(grid, "Услуга", 2, 0);
			AddHeader(grid, "Дата приема", 3, 0);
			AddHeader(grid, "Действия", 4, 0);

			// Добавляем строки с направлениями
			for (int i = 0; i < directions.Count; i++)
			{
				var direction = directions[i];

				// Номер строки (начинаем с 1)
				AddCell(grid, (i + 1).ToString(), 0, i + 1);

				// Тип направления
				AddCell(grid, direction.DirectionType, 1, i + 1);

				// Услуга
				AddCell(grid, direction.Service, 2, i + 1);

				// Дата приема
				AddCell(grid, direction.AppointmentDate.ToString("dd.MM.yyyy"), 3, i + 1);

				var viewButton = new Button
				{
					Content = "Просмотр",
					Margin = new Thickness(2),
					Tag = direction.Id,
					Style = (Style)FindResource("MaterialDesignFlatButton")
				};
				viewButton.Click += ViewDirectionButton_Click;

				Grid.SetColumn(viewButton, 4);
				Grid.SetRow(viewButton, i + 1);
				grid.Children.Add(viewButton);


				//var printButton = new Button
				//{
				//	Content = "Печать",
				//	Margin = new Thickness(2),
				//	Tag = direction.Id,
				//	Style = (Style)FindResource("MaterialDesignFlatButton")
				//};
				//printButton.Click += PrintDirectionButton_Click;

				//Grid.SetColumn(printButton, 5);
				//Grid.SetRow(printButton, i + 1);
				//grid.Children.Add(printButton);
			}

			DirectionsListScrollViewer.Content = grid;
		}

		//private void PrintDirectionButton_Click(object sender, RoutedEventArgs e)
		//{
		//	var button = sender as Button;
		//	int directionId = (int)button.Tag;

		//	// Получаем данные направления из базы
		//	var direction = dbContext.referral.FirstOrDefault(d => d.id_referral == directionId);
		//	if (direction == null)
		//	{
		//		MessageBox.Show("Направление не найдено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
		//		return;
		//	}

		//	// Получаем данные пациента
		//	var patient = dbContext.patient.FirstOrDefault(p => p.id_patient == direction.);

		//	// Получаем данные врача
		//	var doctor = dbContext.doctor.FirstOrDefault(d => d.id_doctor == direction.id_doctor);

		//	// Создаем PDF документ
		//	CreateDirectionPdf(direction, patient, doctor);
		//}

		//private void CreateDirectionPdf(referral direction, patient patient, doctor doctor)
		//{
		//	try
		//	{
		//		// Диалог сохранения файла
		//		var saveFileDialog = new Microsoft.Win32.SaveFileDialog
		//		{
		//			Filter = "PDF файлы (*.pdf)|*.pdf",
		//			FileName = $"Направление_{direction.id_referral}_{DateTime.Now:yyyyMMdd}.pdf"
		//		};

		//		if (saveFileDialog.ShowDialog() == true)
		//		{
		//			// Создаем документ PDF
		//			Document document = new Document(PageSize.A4, 50, 50, 50, 50);
		//			PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));

		//			document.Open();

		//			// Добавляем шрифт с поддержкой кириллицы
		//			string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
		//			BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
		//			Font regularFont = new Font(baseFont, 12);
		//			Font boldFont = new Font(baseFont, 12, Font.BOLD);
		//			Font headerFont = new Font(baseFont, 14, Font.BOLD);

		//			// Заголовок
		//			iTextSharp.text.Paragraph header = new iTextSharp.text.Paragraph("НАПРАВЛЕНИЕ", headerFont);
		//			header.Alignment = Element.ALIGN_CENTER;
		//			header.SpacingAfter = 20;
		//			document.Add(header);

		//			// Номер и дата направления
		//			iTextSharp.text.Paragraph numberDate = new iTextSharp.text.Paragraph();
		//			numberDate.Add(new Phrase("№ ", regularFont));
		//			numberDate.Add(new Phrase(direction.id_referral.ToString(), boldFont));
		//			numberDate.Add(new Phrase($" от {direction.date_of_creation:dd.MM.yyyy}", regularFont));
		//			numberDate.Alignment = Element.ALIGN_CENTER;
		//			document.Add(numberDate);

		//			// Пустая строка
		//			document.Add(new iTextSharp.text.Paragraph(" "));

		//			// Данные пациента
		//			AddPdfField(document, "Пациент:", $"{patient.lastname} {patient.name} {patient.surname}", regularFont, boldFont);
		//			AddPdfField(document, "Дата рождения:", patient.birthday?.ToString("dd.MM.yyyy"), regularFont, boldFont);

		//			// Данные направления
		//			AddPdfField(document, "Тип направления:", direction.type_of_direction, regularFont, boldFont);
		//			AddPdfField(document, "Услуга:", direction.service, regularFont, boldFont);

		//			// Fix for CS0023 - check for default DateTime value
		//			string admissionDate = direction.date_of_admission.HasValue && direction.date_of_admission.Value != DateTime.MinValue
		//				? direction.date_of_admission.Value.ToString("dd.MM.yyyy")
		//				: "не указано";

		//			AddPdfField(document, "Дата приема:", admissionDate, regularFont, boldFont);
		//			AddPdfField(document, "Организация:", direction.organization, regularFont, boldFont);
		//			AddPdfField(document, "Врач:", direction.doctor, regularFont, boldFont);
		//			AddPdfField(document, "Обоснование:", direction.justification, regularFont, boldFont);

		//			// Подпись врача
		//			document.Add(new iTextSharp.text.Paragraph(" "));
		//			iTextSharp.text.Paragraph signature = new iTextSharp.text.Paragraph();
		//			signature.Add(new Phrase("Врач: ", regularFont));
		//			signature.Add(new Phrase($"{doctor.lastname} {doctor.name[0]}.{doctor.surname[0]}.", boldFont));
		//			signature.Alignment = Element.ALIGN_RIGHT;
		//			document.Add(signature);

		//			// Печать
		//			iTextSharp.text.Paragraph stamp = new iTextSharp.text.Paragraph("М.П.");
		//			stamp.Alignment = Element.ALIGN_RIGHT;
		//			document.Add(stamp);

		//			document.Close();

		//			MessageBox.Show("Направление успешно сохранено в PDF!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show($"Ошибка при создании PDF: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
		//	}
		//}

		private void AddPdfField(Document document, string label, string value, Font regularFont, Font boldFont)
		{
			if (string.IsNullOrEmpty(value))
				value = "не указано";

			iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph();
			paragraph.Add(new Phrase(label + " ", regularFont));
			paragraph.Add(new Phrase(value, boldFont));
			paragraph.SpacingAfter = 5;
			document.Add(paragraph);
		}

		private void AddHeader(Grid grid, string text, int column, int row)
		{
			var header = new TextBlock
			{
				Text = text,
				FontWeight = FontWeights.Bold,
				Margin = new Thickness(5),
				HorizontalAlignment = HorizontalAlignment.Left
			};

			Grid.SetColumn(header, column);
			Grid.SetRow(header, row);
			grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
			grid.Children.Add(header);
		}

		private void AddCell(Grid grid, string text, int column, int row)
		{
			var cell = new TextBlock
			{
				Text = text,
				Margin = new Thickness(5),
				TextWrapping = TextWrapping.Wrap,
				VerticalAlignment = VerticalAlignment.Center
			};

			Grid.SetColumn(cell, column);
			Grid.SetRow(cell, row);
			grid.Children.Add(cell);
		}

		private void ViewDirectionButton_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			int directionId = (int)button.Tag;

			// Получаем выбранный диагноз
			var selectedDiagnosis = cbMainDiagnosis.SelectedItem as MkbDiagnosis;
			string diagnosisName = selectedDiagnosis?.Name ?? string.Empty;

			DirectionViewerHelper.ShowDirectionDetails(
				directionId,
				txtPatientName.Text,
				txtPatientBirthDate.Text,
				txtPatientSnils.Text,
				txtDoctorInfo.Text,
				diagnosisName,
				this);
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

		//private void PrintDirection_Click(object sender, RoutedEventArgs e)
		//{
		//	try
		//	{
		//		// Создаем диалог печати
		//		PrintDialog printDialog = new PrintDialog();
		//		if (printDialog.ShowDialog() == true)
		//		{
		//			// Создаем документ для печати
		//			FlowDocument document = CreatePrintableDocument();

		//			// Настройка документа
		//			document.PageHeight = printDialog.PrintableAreaHeight;
		//			document.PageWidth = printDialog.PrintableAreaWidth;
		//			document.PagePadding = new Thickness(50);
		//			document.ColumnGap = 0;
		//			document.ColumnWidth = printDialog.PrintableAreaWidth;

		//			// Печать
		//			printDialog.PrintDocument(
		//				((IDocumentPaginatorSource)document).DocumentPaginator,
		//				"Направление на консультацию");
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show($"Ошибка при печати: {ex.Message}", "Ошибка",
		//			MessageBoxButton.OK, MessageBoxImage.Error);
		//	}
		//}

		//private FlowDocument CreatePrintableDocument()
		//{
		//	// Создаем документ с содержимым для печати
		//	FlowDocument document = new FlowDocument();

		//	// Добавляем заголовок
		//	System.Windows.Documents.Paragraph header = new System.Windows.Documents.Paragraph(new Run("НАПРАВЛЕНИЕ"))
		//	{
		//		FontSize = 16,
		//		FontWeight = FontWeights.Bold,
		//		TextAlignment = TextAlignment.Center
		//	};
		//	document.Blocks.Add(header);

		//	// Добавляем номер и дату
		//	document.Blocks.Add(new System.Windows.Documents.Paragraph(new Run($"№ ______ от «{DateTime.Now:dd}» {GetMonthName(DateTime.Now.Month)} {DateTime.Now:yyyy} г."))
		//	{
		//		TextAlignment = TextAlignment.Center
		//	});

		//	// Добавляем данные пациента
		//	AddFieldToDocument(document, "Пациент:", PatientNameTextBox.Text);
		//	AddFieldToDocument(document, "Дата рождения:", PatientBirthDatePicker.Text);
		//	AddFieldToDocument(document, "Направлен в:", ((ComboBoxItem)DirectionComboBox.SelectedItem)?.Content.ToString());
		//	AddFieldToDocument(document, "Диагноз:", cbNDiagnosis.Text);
		//	AddFieldToDocument(document, "Примечания:", NotesTextBox.Text);

		//	// Добавляем подпись
		//	document.Blocks.Add(new System.Windows.Documents.Paragraph(new Run("\nВрач: _________________________")));
		//	document.Blocks.Add(new System.Windows.Documents.Paragraph(new Run("Подпись: _______________________")));

		//	return document;
		//}

		private void AddFieldToDocument(FlowDocument document, string fieldName, string fieldValue)
		{
			System.Windows.Documents.Paragraph paragraph = new System.Windows.Documents.Paragraph();
			paragraph.Inlines.Add(new Run(fieldName) { FontWeight = FontWeights.Bold });
			paragraph.Inlines.Add(new Run($" {fieldValue}"));
			document.Blocks.Add(paragraph);
		}

		private string GetMonthName(int month)
		{
			string[] months = { "января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря" };
			return months[month - 1];
		}

		//private void SaveToWord_Click(object sender, RoutedEventArgs e)
		//{
		//	DirectionComboBox.SelectedIndex = -1;
		//	cbNDiagnosis.Text = string.Empty;
		//	NotesTextBox.Text = string.Empty;
		//}

		private void AddFieldToPdf(Document document, string fieldName, string fieldValue, Font font)
		{
			if (string.IsNullOrWhiteSpace(fieldValue))
				fieldValue = "не указано";

			iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph
			{
				new Chunk(fieldName + " ", new Font(font.BaseFont, 12, Font.BOLD)),
				new Chunk(fieldValue, font)
			};
			paragraph.SpacingAfter = 10;
			document.Add(paragraph);
		}

		private void AddDateRange_Click(object sender, RoutedEventArgs e)
		{
			AddDateRange();
		}

		private void AddDateRange()
		{
			_dateRanges.Add(new object()); // Просто добавляем пустой объект как placeholder
		}

		private void RemoveDateRange_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button button && button.DataContext is object item)
			{
				_dateRanges.Remove(item);
			}
		}

		//private void ExportToPdf_Click(object sender, RoutedEventArgs e)
		//{
		//	try
		//	{
		//		// Создаем диалог сохранения файла
		//		var saveFileDialog = new Microsoft.Win32.SaveFileDialog
		//		{
		//			Filter = "PDF файлы (*.pdf)|*.pdf",
		//			FileName = $"Справка_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
		//		};

		//		if (saveFileDialog.ShowDialog() == true)
		//		{
		//			// Создаем документ PDF
		//			Document document = new Document(PageSize.A4, 50, 50, 50, 50);
		//			PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));

		//			document.Open();

		//			// Добавляем шрифт с поддержкой кириллицы
		//			string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
		//			BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
		//			Font regularFont = new Font(baseFont, 9);
		//			Font font = new Font(baseFont, 12, Font.NORMAL);
		//			Font boldFont = new Font(baseFont, 11, Font.BOLD);
		//			Font headerFont = new Font(baseFont, 14, Font.BOLD);
		//			Font underlineFont = new Font(baseFont, 13, Font.BOLD | Font.UNDERLINE);

		//			// Создаем таблицу с 2 колонками
		//			PdfPTable table = new PdfPTable(2);
		//			table.WidthPercentage = 100;
		//			table.SetWidths(new float[] { 1, 1 });

		//			// Правая ячейка (Column 1) - Код формы по ОКУД
		//			PdfPCell codesCell = new PdfPCell(new Phrase("Код формы по ОКУД _______________", regularFont));
		//			codesCell.HorizontalAlignment = Element.ALIGN_RIGHT;
		//			codesCell.Border = PdfPCell.NO_BORDER;
		//			codesCell.Colspan = 2;
		//			codesCell.PaddingLeft = 200;
		//			codesCell.PaddingRight = 30;
		//			table.AddCell(codesCell);

		//			// Вторая строка таблицы (Row 1)
		//			// Левая ячейка (Column 0) - пустая
		//			table.AddCell(new PdfPCell() { Border = PdfPCell.NO_BORDER });

		//			// Правая ячейка (Column 1) - Код учреждения по ОКПО
		//			codesCell = new PdfPCell(new Phrase("Код учреж. по ОКПО _______________", regularFont));
		//			codesCell.HorizontalAlignment = Element.ALIGN_RIGHT;
		//			codesCell.Border = PdfPCell.NO_BORDER;
		//			codesCell.Colspan = 2;
		//			codesCell.PaddingLeft = 50;
		//			codesCell.PaddingRight = 30;
		//			table.AddCell(codesCell);

		//			table.AddCell(new PdfPCell(new Phrase("")) { Border = PdfPCell.NO_BORDER });

		//			// Вторая строка - Правый столбец
		//			PdfPCell rightCell = new PdfPCell(new Phrase("Медицинская документация", regularFont));
		//			rightCell.HorizontalAlignment = Element.ALIGN_RIGHT;
		//			rightCell.Border = PdfPCell.NO_BORDER;
		//			rightCell.PaddingRight = 30;
		//			table.AddCell(rightCell);

		//			// Вторая строка - Левый столбец
		//			PdfPCell leftCell = new PdfPCell(new Phrase("Министерство здравоохранения РФ", regularFont));
		//			leftCell.HorizontalAlignment = Element.ALIGN_LEFT;
		//			leftCell.Border = PdfPCell.NO_BORDER;
		//			leftCell.PaddingLeft = 30;
		//			table.AddCell(leftCell);

		//			// Третья строка - Правый столбец
		//			rightCell = new PdfPCell(new Phrase("Форма № 095/у", regularFont));
		//			rightCell.HorizontalAlignment = Element.ALIGN_RIGHT;
		//			rightCell.Border = PdfPCell.NO_BORDER;
		//			rightCell.PaddingRight = 60;
		//			table.AddCell(rightCell);

		//			// Четвертая строка - Левый столбец (пустой)
		//			table.AddCell(new PdfPCell() { Border = PdfPCell.NO_BORDER });

		//			// Четвертая строка - Правый столбец
		//			rightCell = new PdfPCell(new Phrase("Утверждена Минздравом СССР", regularFont));
		//			rightCell.HorizontalAlignment = Element.ALIGN_RIGHT;
		//			rightCell.Border = PdfPCell.NO_BORDER;
		//			rightCell.PaddingRight = 20;
		//			table.AddCell(rightCell);

		//			// Пятая строка - Левый столбец (пустой)
		//			table.AddCell(new PdfPCell() { Border = PdfPCell.NO_BORDER });

		//			// Пятая строка - Правый столбец
		//			rightCell = new PdfPCell(new Phrase("04.10.80 г. № 1030", regularFont));
		//			rightCell.HorizontalAlignment = Element.ALIGN_RIGHT;
		//			rightCell.Border = PdfPCell.NO_BORDER;
		//			rightCell.PaddingRight = 50;
		//			table.AddCell(rightCell);

		//			// Добавляем таблицу в документ
		//			document.Add(table);

		//			// Заголовок "СПРАВКА" (Row 6)
		//			iTextSharp.text.Paragraph header = new iTextSharp.text.Paragraph("СПРАВКА", headerFont);
		//			header.Alignment = Element.ALIGN_CENTER;
		//			header.SpacingAfter = 20;
		//			document.Add(header);
		//			// Основной текст справки
		//			iTextSharp.text.Paragraph mainText = new iTextSharp.text.Paragraph();
		//			mainText.Add(new Phrase("О временной нетрудоспособности студента, учащегося техникума,\n", boldFont));
		//			mainText.Add(new Phrase("профессионально-технического училища, о болезни, карантине и прочих\n", boldFont));
		//			mainText.Add(new Phrase("причинах отсутствия ребенка, посещающего школу, детское дошкольное\n", boldFont));
		//			mainText.Add(new Phrase("учреждение.\n", boldFont));
		//			mainText.Add(new Phrase("(нужное подчеркнуть)\n\n", regularFont));
		//			mainText.Alignment = Element.ALIGN_CENTER;
		//			document.Add(mainText);
		//			// Дата выдачи
		//			iTextSharp.text.Paragraph dateParagraph = new iTextSharp.text.Paragraph();
		//			dateParagraph.Add(new Phrase("Дата выдачи ", regularFont));

		//			// Получаем дату из поля DateCpNaprTB и добавляем как подчеркнутый текст
		//			if (!string.IsNullOrEmpty(DateCpNaprTB.Text))
		//			{
		//				// Создаем подчеркнутый шрифт с поддержкой кириллицы
		//				Font underlinedFont = new Font(baseFont, 9, Font.UNDERLINE);
		//				dateParagraph.Add(new Phrase(DateCpNaprTB.Text, underlinedFont));
		//			}
		//			else
		//			{
		//				// Если дата не указана, добавляем подчеркнутое подчеркивание
		//				dateParagraph.Add(new Chunk("_________________", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.UNDERLINE)));
		//			}
		//			dateParagraph.Alignment = Element.ALIGN_CENTER;
		//			document.Add(dateParagraph);

		//			// Создаем параграф для подчеркнутой строки
		//			iTextSharp.text.Paragraph underlinedParagraph = new iTextSharp.text.Paragraph();

		//			// Получаем выбранное значение или "не указано"
		//			string recipientType = RecipientTypeComboBox.SelectedItem != null
		//				? ((ComboBoxItem)RecipientTypeComboBox.SelectedItem).Content.ToString()
		//				: "не указано";

		//			// Создаем текст с подчеркиванием
		//			string fullText = "Тип получателя: " + recipientType;

		//			// Создаем подчеркнутый шрифт
		//			Font underlinedFontы = new Font(baseFont, 9, Font.UNDERLINE);

		//			// Добавляем весь текст с подчеркиванием
		//			underlinedParagraph.Add(new Phrase(fullText, underlinedFontы));

		//			// Настройки форматирования
		//			underlinedParagraph.Alignment = Element.ALIGN_LEFT;
		//			underlinedParagraph.IndentationLeft = 30; // Отступ слева 30 пунктов
		//			underlinedParagraph.SpacingAfter = 10;    // Отступ после строки

		//			// Добавляем в документ
		//			document.Add(underlinedParagraph);

		//			// Первая строка - подчеркнутая на всю ширину
		//			iTextSharp.text.Paragraph nameinstitutionParagraph = new iTextSharp.text.Paragraph();

		//			// Создаем подчеркнутый шрифт
		//			Font nameinstitutionFontы = new Font(baseFont, 9, Font.UNDERLINE);

		//			// Получаем выбранное значение или "не указано"
		//			string nameinstitutionType = InstitutionTextBox.Text != null && InstitutionTextBox.Text != ""
		//				? InstitutionTextBox.Text
		//				: "не указано";

		//			// Создаем фразу с подчеркиванием на всю строку
		//			Phrase nameinstitutionPhrase = new Phrase(nameinstitutionType, nameinstitutionFontы);
		//			nameinstitutionParagraph.Add(nameinstitutionPhrase);

		//			// Выравниваем по левому краю с отступом
		//			nameinstitutionParagraph.Alignment = Element.ALIGN_LEFT;
		//			nameinstitutionParagraph.IndentationLeft = 30;
		//			nameinstitutionParagraph.SpacingAfter = 5;
		//			document.Add(nameinstitutionParagraph);

		//			// Добавление информации о ФИО
		//			iTextSharp.text.Paragraph FullNameParagraph = new iTextSharp.text.Paragraph();
		//			string FullNameType = "не указано";
		//			if (FullNameTextBox.Text != null && FullNameTextBox.Text != "")
		//			{
		//				FullNameType = FullNameTextBox.Text;
		//			}
		//			FullNameParagraph.Add(new Phrase("Фамилия, имя, отчество: ", regularFont));
		//			Font FullNameFont = new Font(baseFont, 9, Font.UNDERLINE);
		//			FullNameParagraph.Add(new Phrase(FullNameType, FullNameFont));
		//			FullNameParagraph.Alignment = Element.ALIGN_LEFT;
		//			FullNameParagraph.IndentationLeft = 30;
		//			FullNameParagraph.SpacingAfter = 10;
		//			document.Add(FullNameParagraph);

		//			// Добавление информации о дате рождения (год, месяц, для детей до 1-го года – день)
		//			iTextSharp.text.Paragraph BirthDateParagraph = new iTextSharp.text.Paragraph();
		//			string BirthDateType = "не указано";
		//			if (BirthDatePicker.Text != null && BirthDatePicker.Text != "")
		//			{
		//				BirthDateType = BirthDatePicker.Text;
		//			}
		//			BirthDateParagraph.Add(new Phrase("Дата рождения (год, месяц, для детей до 1-го года – день): ", regularFont));
		//			Font BirthDateFont = new Font(baseFont, 9, Font.UNDERLINE);
		//			BirthDateParagraph.Add(new Phrase(BirthDateType, BirthDateFont));
		//			BirthDateParagraph.Alignment = Element.ALIGN_LEFT;
		//			BirthDateParagraph.IndentationLeft = 30;
		//			BirthDateParagraph.SpacingAfter = 10;
		//			document.Add(BirthDateParagraph);

		//			// Добавление информации о диагнозе заболевания (прочие причины отсутствия)
		//			iTextSharp.text.Paragraph DiagnosisParagraph = new iTextSharp.text.Paragraph();
		//			string DiagnosisType = "не указано";
		//			if (DiagnosisTextBox.Text != null && DiagnosisTextBox.Text != "")
		//			{
		//				DiagnosisType = DiagnosisTextBox.Text;
		//			}
		//			DiagnosisParagraph.Add(new Phrase("Диагноз заболевания (прочие причины отсутствия): ", regularFont));
		//			Font DiagnosisFont = new Font(baseFont, 9, Font.UNDERLINE);
		//			DiagnosisParagraph.Add(new Phrase(DiagnosisType, DiagnosisFont));
		//			DiagnosisParagraph.Alignment = Element.ALIGN_LEFT;
		//			DiagnosisParagraph.IndentationLeft = 30;
		//			DiagnosisParagraph.SpacingAfter = 10;
		//			document.Add(DiagnosisParagraph);

		//			// Добавление информации об освобождении от занятий, посещений детского дошкольного учреждения
		//			iTextSharp.text.Paragraph LiberationParagraph = new iTextSharp.text.Paragraph();
		//			LiberationParagraph.Add(new Phrase("Освобожден от занятий, посещений детского дошкольного учреждения", regularFont));
		//			LiberationParagraph.Alignment = Element.ALIGN_LEFT;
		//			LiberationParagraph.IndentationLeft = 30;
		//			LiberationParagraph.SpacingAfter = 10;
		//			document.Add(LiberationParagraph);

		//			// Закрываем документ
		//			document.Close();
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show($"Ошибка при создании PDF: {ex.Message}", "Ошибка",
		//					  MessageBoxButton.OK, MessageBoxImage.Error);
		//	}
		//}


		private void GenerateFormBt_Click(object sender, RoutedEventArgs e)
		{
			if (PrescriptionFormComboBox.SelectedItem == null)
			{
				MessageBox.Show("Выберите форму рецептурного бланка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			string patientFullName = txtPatientName.Text;
			string patientBirthDate = txtPatientBirthDate.Text;
			string DoctorInfo = txtDoctorInfo.Text;

			string selectedForm = ((ComboBoxItem)PrescriptionFormComboBox.SelectedItem).Content.ToString();

			switch (selectedForm)
			{
				case "ФОРМА РЕЦЕПТУРНОГО БЛАНКА N 107-1/у":
					var prescriptionPage = new PrescriptionForm107(patientFullName, patientBirthDate, DoctorInfo, GetCurrentInspectionId());
					PrescriptionFrame.Navigate(prescriptionPage);
					break;

				//case "ФОРМА РЕЦЕПТУРНОГО БЛАНКА N 148-1/у-88":
				//	PrescriptionFrame.Navigate(new Uri("PrescriptionForms/PrescriptionForm14888.xaml", UriKind.Relative));
				//	break;

					//case "ФОРМА РЕЦЕПТУРНОГО БЛАНКА N 148-1/у-04(л)":
					//	PrescriptionFrame.Navigate(new Uri("PrescriptionForms/PrescriptionForm148_04.xaml", UriKind.Relative));
					//	break;

					//case "ФОРМА РЕЦЕПТУРНОГО БЛАНКА N 107/у-НП":
					//	PrescriptionFrame.Navigate(new Uri("PrescriptionForms/PrescriptionForm107_HP.xaml", UriKind.Relative));
					//	break;
			}
		}

		private void CompleteCaseButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Случай успешно сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
			this.Close();
		}

		private void SaveBt_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// Собираем данные с формы
				var newInspection = CollectFormData();

				// Сохраняем в базу данных
				SaveMedicalCaseToDatabase(newInspection);

				MessageBox.Show("Данные осмотра успешно сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
				MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void btnAddDirection_Click(object sender, RoutedEventArgs e)
		{
			// Проверяем, сохранены ли данные осмотра
			if (!IsInspectionSaved())
			{
				MessageBox.Show("Сначала необходимо сохранить данные осмотра!", "Внимание",  MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			var preliminaryDiagnosis = cbPreliminaryDiagnosis.SelectedItem as MkbDiagnosis;
			var mainDiagnosis = cbMainDiagnosis.SelectedItem as MkbDiagnosis;

			// Создаем страницу с формой направления
			var directionFormPage = new DirectionFormPage(txtPatientName.Text, txtPatientBirthDate.Text, txtDoctorInfo.Text,
				preliminaryDiagnosis, mainDiagnosis, int.Parse(NumberTextBlock.Text));

			// Подписываемся на событие закрытия формы направления
			directionFormPage.DirectionSaved += (s, args) =>
			{
				// Когда направление сохранено, обновляем список
				LoadDirectionsFromDatabase();
				ShowDirectionsList();
			};

			// Отображаем форму во фрейме
			DirectionFrame.Navigate(directionFormPage);
			DirectionFrame.Visibility = Visibility.Visible;
			DirectionsListScrollViewer.Visibility = Visibility.Collapsed;
			btnAddDirection.Visibility = Visibility.Collapsed;
		}

		private bool IsInspectionSaved()
		{
			// Проверяем, есть ли сохраненный осмотр для этого пациента
			if (string.IsNullOrEmpty(IDPatientTextBlock.Text))
				return false;

			int patientId = int.Parse(IDPatientTextBlock.Text);
			return dbContext.inspection.Any(i => i.id_patient == patientId);
		}

		public void ShowDirectionsList()
		{
			LoadDirectionsFromDatabase();

			DirectionsListScrollViewer.Visibility = Visibility.Visible;
			btnAddDirection.Visibility = Visibility.Visible;
			DirectionFrame.Visibility = Visibility.Collapsed;
		}

		private int? GetCurrentInspectionId()
		{
			return int.Parse(NumberTextBlock.Text);
		}

		private void LoadDirectionsFromDatabase()
		{
			if (!IsInspectionSaved()) return;

			int? inspectionId = GetCurrentInspectionId();
			if (inspectionId == null) return;

			var directions = dbContext.referral
				.Where(d => d.id_inspection == inspectionId)
				.Select(d => new Direction
				{
					Id = d.id_referral,
					DirectionType = d.type_of_direction,
					Service = d.service,
					AppointmentDate = d.date_of_admission ?? DateTime.MinValue,
					Organization = d.organization,
					Doctor = d.doctor,
					DirectionNumber = d.justification
				})
				.ToList();

			LoadDirections(directions);
		}

		private void btnAddSickLeave_Click(object sender, RoutedEventArgs e)
		{
			// Проверяем, сохранены ли данные осмотра
			if (!IsInspectionSaved())
			{
				MessageBox.Show("Сначала необходимо сохранить данные осмотра!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			// Создаем страницу с формой листка нетрудоспособности
			var sickLeaveFormPage = new SickLeaveFormPage(txtPatientName.Text, txtPatientBirthDate.Text, int.Parse(NumberTextBlock.Text));

			// Отображаем форму во фрейме
			SickLeaveFrame.Navigate(sickLeaveFormPage);
			SickLeaveFrame.Visibility = Visibility.Visible;

			// Скрываем список листков
			SickLeaveListScrollViewer.Visibility = Visibility.Collapsed;

			// Скрываем кнопку добавления
			btnAddSickLeave.Visibility = Visibility.Collapsed;
		}

		public void ShowSickLeaveList()
		{
			SickLeaveListScrollViewer.Visibility = Visibility.Visible;
			btnAddSickLeave.Visibility = Visibility.Visible;
			SickLeaveFrame.Visibility = Visibility.Collapsed;
		}		
	}
}