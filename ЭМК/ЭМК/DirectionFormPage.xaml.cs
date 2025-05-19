using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ЭМК.Model;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для DirectionFormPage.xaml
	/// </summary>
	public partial class DirectionFormPage : Page
	{
		private ObservableCollection<MkbDiagnosis> _diagnoses;
		private string _patientName;
		private string _patientBirthDate;
		private string _doctorInfo;
		private MkbDiagnosis _preliminaryDiagnosis;
		private MkbDiagnosis _mainDiagnosis;
		private int? _inspectionId;

		public DirectionFormPage(string patientName, string patientBirthDate, string doctorInfo, MkbDiagnosis preliminaryDiagnosis, MkbDiagnosis mainDiagnosis, int? inspectionId)
		{
			InitializeComponent();

			_patientName = patientName;
			_patientBirthDate = patientBirthDate;
			_doctorInfo = doctorInfo;
			_preliminaryDiagnosis = preliminaryDiagnosis;
			_mainDiagnosis = mainDiagnosis;
			_inspectionId = inspectionId;

			LoadData();
			_ = NumberVoidAsync();
		}

		public async Task NumberVoidAsync()
		{
			try
			{
				// Создаем новый контекст для этой операции
				using (var localDbContext = new MedCardDBEntities())
				{
					int referralCount = await localDbContext.referral.CountAsync()+1;
					NumberTextBlock.Text = referralCount.ToString();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при получении номера направления: {ex.Message}");
			}
		}

		private void LoadData()
		{
			// Заполняем данные пациента
			PatientNameTextBox.Content = _patientName;
			PatientBirthDatePicker.Content = _patientBirthDate;
			DateNaprTB.Text = DateTime.Now.ToString("dd.MM.yyyy");
			AppointmentDatePicker.Text = DateTime.Now.ToString("dd.MM.yyyy");

			LoadOrganizations();
			LoadDoctors();
			DisplayDiagnosis();
		}

		private void DisplayDiagnosis()
		{
			string diagnosisText = "";

			if (_mainDiagnosis != null)
			{
				diagnosisText = $"{_mainDiagnosis.Code} {_mainDiagnosis.Name}";
			}
			else if (_preliminaryDiagnosis != null)
			{
				diagnosisText = $"{_preliminaryDiagnosis.Code} {_preliminaryDiagnosis.Name} (предварительный)";
			}
			else
			{
				diagnosisText = "Диагноз не указан";
			}

			LblNDiagnosis.Content = diagnosisText;
		}

		private void LoadDoctors()
		{
			var doctors = new List<string>
			{
				"Иванов А.И. (терапевт)",
				"Петрова Е.С. (кардиолог)",
				"Сидоров В.М. (невролог)",
				"Кузнецова О.П. (педиатр)",
				"Смирнов Д.А. (хирург)",
				"Васильева Т.К. (офтальмолог)",
				"Николаев П.С. (отоларинголог)",
				"Федорова Л.Д. (гинеколог)"
			};

			cbDoctors.ItemsSource = doctors;
		}

		private void LoadOrganizations()
		{
			var organizations = new List<string>
			{
				"Городская больница №1",
				"Детская клиническая больница",
				"Областной онкологический диспансер",
				"Поликлиника №3",
				"Стоматологическая поликлиника",
				"Центр восстановительной медицины"
			};

			cbOrganization.ItemsSource = organizations;
		}

		private void PrintDirection_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				PrintDialog printDialog = new PrintDialog();
				if (printDialog.ShowDialog() == true)
				{
					FlowDocument document = CreatePrintableDocument();

					document.PageHeight = printDialog.PrintableAreaHeight;
					document.PageWidth = printDialog.PrintableAreaWidth;
					document.PagePadding = new Thickness(50);
					document.ColumnGap = 0;
					document.ColumnWidth = printDialog.PrintableAreaWidth;

					printDialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, "Направление на консультацию");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при печати: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private FlowDocument CreatePrintableDocument()
		{
			FlowDocument document = new FlowDocument();

			// Заголовок
			System.Windows.Documents.Paragraph header = new System.Windows.Documents.Paragraph(new Run("НАПРАВЛЕНИЕ"))
			{
				FontSize = 16,
				FontWeight = FontWeights.Bold,
				TextAlignment = TextAlignment.Center
			};
			document.Blocks.Add(header);

			// Номер и дата
			document.Blocks.Add(new System.Windows.Documents.Paragraph(new Run($"№ ______ от «{DateTime.Now:dd}» {GetMonthName(DateTime.Now.Month)} {DateTime.Now:yyyy} г."))
			{
				TextAlignment = TextAlignment.Center
			});

			// Данные пациента
			AddFieldToDocument(document, "Пациент:", PatientNameTextBox.Content.ToString());
			AddFieldToDocument(document, "Дата рождения:", PatientBirthDatePicker.Content.ToString());
			AddFieldToDocument(document, "Направлен в:", ((ComboBoxItem)DirectionComboBox.SelectedItem)?.Content.ToString());
			AddFieldToDocument(document, "Диагноз:", LblNDiagnosis.Content.ToString());
			AddFieldToDocument(document, "Примечания:", NotesTextBox.Text);

			// Подпись
			document.Blocks.Add(new System.Windows.Documents.Paragraph(new Run("\nВрач: _________________________")));
			document.Blocks.Add(new System.Windows.Documents.Paragraph(new Run("Подпись: _______________________")));

			return document;
		}

		private void AddFieldToDocument(FlowDocument document, string fieldName, string fieldValue)
		{
			System.Windows.Documents.Paragraph paragraph = new System.Windows.Documents.Paragraph();
			paragraph.Inlines.Add(new Run(fieldName) { FontWeight = FontWeights.Bold });
			paragraph.Inlines.Add(new Run($" {fieldValue}"));
			document.Blocks.Add(paragraph);
		}

		private string GetMonthName(int month)
		{
			string[] months = { "января", "февраля", "марта", "апреля", "мая", "июня",
							   "июля", "августа", "сентября", "октября", "ноября", "декабря" };
			return months[month - 1];
		}

		private void SaveDirection_Click(object sender, RoutedEventArgs e)
		{
			if (!_inspectionId.HasValue)
			{
				MessageBox.Show("Не найден осмотр. Сохраните осмотр перед созданием направления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			// Проверка выбранного направления
			if (DirectionComboBox.SelectedItem == null && string.IsNullOrWhiteSpace(DirectionComboBox.Text))
			{
				MessageBox.Show("Выберите или введите тип направления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
				DirectionComboBox.Focus();
				return;
			}

			// Проверка источника оплаты
			if (PaymentSourcecComboBox.SelectedItem == null && string.IsNullOrWhiteSpace(PaymentSourcecComboBox.Text))
			{
				MessageBox.Show("Выберите или введите источник оплаты!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
				PaymentSourcecComboBox.Focus();
				return;
			}

			// Проверка выбранной услуги
			if (string.IsNullOrWhiteSpace(SelectedProcedureText.Text) || SelectedProcedureText.Text == "(не выбрано)")
			{
				MessageBox.Show("Выберите медицинскую услугу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
				SelectProcedureButton.Focus();
				return;
			}

			// Проверка организации
			if (cbOrganization.SelectedItem == null && string.IsNullOrWhiteSpace(cbOrganization.Text))
			{
				MessageBox.Show("Выберите медицинскую организацию!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
				cbOrganization.Focus();
				return;
			}

			// Проверка даты приема
			var datePicker = (DatePicker)FindName("AppointmentDatePicker");
			if (datePicker.SelectedDate == null)
			{
				MessageBox.Show("Укажите дату приема!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
				datePicker.Focus();
				return;
			}

			if (datePicker.SelectedDate < DateTime.Today)
			{
				MessageBox.Show("Дата приема не может быть раньше сегодняшнего дня!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
				datePicker.Focus();
				return;
			}

			try
			{
				using (var db = new MedCardDBEntities())
				{
					var newDirection = new referral
					{
						id_inspection = _inspectionId.Value,
						type_of_direction = DirectionComboBox.Text,
						service = SelectedProcedureText.Text,
						date_of_admission = AppointmentDatePicker.SelectedDate ?? DateTime.Now,
						organization = cbOrganization.Text,
						doctor = cbDoctors.Text,
						justification = NotesTextBox.Text,
						payment_source = PaymentSourcecComboBox.Text
					};


					db.referral.Add(newDirection);
					db.SaveChanges();
				}
					

				MessageBox.Show("Направление сохранено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

				// Возвращаемся назад
				var parentWindow = Window.GetWindow(this) as MedicalCaseWindow;
				parentWindow?.ShowDirectionsList();

				DirectionSaved?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении направления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void CancelDirection_Click(object sender, RoutedEventArgs e)
		{
			// Получаем родительское окно
			var parentWindow = Window.GetWindow(this) as MedicalCaseWindow;

			if (parentWindow != null)
			{
				// Вызываем метод показа списка направлений
				parentWindow.ShowDirectionsList();
			}
			else
			{
				// Альтернативный вариант закрытия, если не удалось получить родительское окно
				var frame = Parent as Frame;
				if (frame != null)
				{
					frame.Visibility = Visibility.Collapsed;
					frame.Navigate(null);
				}
			}
		}

		private void SelectProcedureButton_Click(object sender, RoutedEventArgs e)
		{
			var proceduresWindow = new ProceduresWindow();
			if (proceduresWindow.ShowDialog() == true)
			{
				// Получаем выбранную процедуру
				var selectedProcedure = proceduresWindow.SelectedProcedure;
				// Отображаем выбранную процедуру
				SelectedProcedureText.Text = selectedProcedure.DisplayText;

				// Можно также сохранить выбранную процедуру в свойствах страницы
				this.SelectedProcedure = selectedProcedure;
			}
		}

		// Свойство для хранения выбранной процедуры
		public Procedure SelectedProcedure { get; private set; }

		public event EventHandler DirectionSaved;
	}
}
