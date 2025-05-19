using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace ЭМК.PrescriptionForms
{
	/// <summary>
	/// Логика взаимодействия для PrescriptionForm107.xaml
	/// </summary>
	public partial class PrescriptionForm107 : Page
	{
		public class DrugItem
		{
			public string DisplayName { get; set; } 
			public string Type { get; set; } 
		}

		private int? _inspectionId;
		public PrescriptionForm107(string fullName, string birthDate, string doctorName, int? inspectionId)
		{
			InitializeComponent();

			InitializeControls();
			SubscribeToEvents();

			LoadDrugDataFromFile("medicine.txt");
			_inspectionId=inspectionId;
		}

		private void LoadDrugDataFromFile(string filePath)
		{
			List<DrugItem> drugList = new List<DrugItem>();

			try
			{
				using (StreamReader reader = new StreamReader(filePath))
				{
					// Пропускаем заголовок, если он есть
					reader.ReadLine();

					string line;
					while ((line = reader.ReadLine()) != null)
					{
						string[] parts = line.Split(';');
						if (parts.Length == 2)
						{
							drugList.Add(new DrugItem { DisplayName = parts[0].Trim(), Type = parts[1].Trim() });
						}
						else
						{
							MessageBox.Show($"Неверный формат строки в файле: {line}"); // Обработка ошибок формата
						}
					}
				}

				DrugNameComboBox.ItemsSource = drugList;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при чтении файла: {ex.Message}"); // Обработка ошибок чтения файла
			}
		}

		private void InitializeControls()
		{
			// Инициализация ComboBox'ов
			var commonItems = new Dictionary<string, IEnumerable<string>>
			{
				{ nameof(AmountComboBox), new List<string> { "1", "5", "10", "25", "50", "100", "200", "500", "1000" }},
				{ nameof(UnitComboBox), new List<string> { "мг", "г", "мкг", "мл", "ЕД", "МЕ" }},
				{ nameof(FrequencyComboBox), new List<string> { "1 раз в день", "2 раза в день", "3 раза в день", "Каждые 4 часа", "По необходимости" }},
				{ nameof(RouteComboBox), new List<string> { "Перорально", "Внутримышечно", "Внутривенно", "Подкожно", "Местно" }},
				{ nameof(AdministrationMethodComboBox), new List<string> { "Перорально", "Внутривенно", "Внутримышечно", "Подкожно", "Местно", "Ингаляционно", "Ректально", "Сублингвально" }},
				{ nameof(DosageRegimenComboBox), new List<string> { "Утром", "Вечером", "Утром и вечером", "Перед сном", "По требованию", "Каждые 6 часов", "Каждые 8 часов" }}
			};

			foreach (var item in commonItems)
			{
				((ComboBox)FindName(item.Key)).ItemsSource = item.Value;
			}
		}

		private void SubscribeToEvents()
		{
			// Подписка на события для всех ComboBox'ов
			var comboBoxes = new[] { AmountComboBox, UnitComboBox, FrequencyComboBox, RouteComboBox, AdministrationMethodComboBox, DosageRegimenComboBox };

			foreach (var comboBox in comboBoxes)
			{
				comboBox.SelectionChanged += ComboBox_SelectionChanged;
			}

			// Подписка на события TextBox'ов
			var textBoxes = new[] { AdministrationDetailsTextBox, DosageRegimenDetailsTextBox, TreatmentDurationTextBox, TreatmentDurationCommentsTextBox };

			foreach (var textBox in textBoxes)
			{
				textBox.TextChanged += TextBox_TextChanged;
			}

			TreatmentDurationUnitComboBox.SelectionChanged += ComboBox_SelectionChanged;
		}

		private void UpdateDosageText()
		{
			var components = new List<string>();

			if (AmountComboBox.SelectedItem != null && UnitComboBox.SelectedItem != null)
			{
				components.Add($"{AmountComboBox.SelectedItem} {UnitComboBox.SelectedItem}");

				AddIfNotNull(FrequencyComboBox.SelectedItem, ref components);
				AddIfNotNull(RouteComboBox.SelectedItem, ref components);

				DosageTextBlock.Text = string.Join(", ", components);
			}
			else
			{
				DosageTextBlock.Text = "Дозировка не выбрана";
			}
		}

		private void AddIfNotNull(object value, ref List<string> list)
		{
			if (value != null) list.Add(value.ToString());
		}

		private void AddIfNotEmpty(string value, string prefix, ref List<string> list)
		{
			if (!string.IsNullOrEmpty(value))
				list.Add($"{prefix}: {value}");
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UpdateDosageText();
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			UpdateDosageText();
		}

		private void SaveBt_Click(object sender, RoutedEventArgs e)
		{
			if (!_inspectionId.HasValue)
			{
				MessageBox.Show("Не найден осмотр. Сохраните осмотр перед созданием направления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			// Проверка выбранного направления
			if (DrugNameComboBox.SelectedItem == null && string.IsNullOrWhiteSpace(DrugNameComboBox.Text) &&
				AmountComboBox.SelectedItem == null && string.IsNullOrWhiteSpace(AmountComboBox.Text) &&
				UnitComboBox.SelectedItem == null && string.IsNullOrWhiteSpace(UnitComboBox.Text) &&
				FrequencyComboBox.SelectedItem == null && string.IsNullOrWhiteSpace(FrequencyComboBox.Text) &&
				RouteComboBox.SelectedItem == null && string.IsNullOrWhiteSpace(RouteComboBox.Text) &&
				AdministrationMethodComboBox.SelectedItem == null && string.IsNullOrWhiteSpace(AdministrationMethodComboBox.Text) &&
				DosageRegimenComboBox.SelectedItem == null && string.IsNullOrWhiteSpace(DosageRegimenComboBox.Text) &&
				TreatmentDurationUnitComboBox.SelectedItem == null && string.IsNullOrWhiteSpace(TreatmentDurationUnitComboBox.Text))
			{
				MessageBox.Show("Заполните все обязательные поля корректными данными!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
				DrugNameComboBox.Focus();
				return;
			}

			try
			{
				using (var db = new MedCardDBEntities())
				{
					var newPrescription_form_107у = new prescription_form_107у
					{
						id_inspection = _inspectionId.Value,
						name_of_the_drug = DrugNameComboBox.Text,
						dosage = DosageTextBlock.Text,
						method_of_administration = AdministrationMethodComboBox.Text,
						method_of_administration_details = AdministrationDetailsTextBox.Text,
						dosage_regimen = DosageRegimenComboBox.Text,
						dosage_regimen_optional = DosageRegimenDetailsTextBox.Text,
						duration_of_treatment_number = TreatmentDurationTextBox.Text,
						duration_of_treatment_duration = TreatmentDurationUnitComboBox.Text,
						duration_of_treatment_comments = TreatmentDurationCommentsTextBox.Text,
						justification_of_appointment = JustificationTextBox.Text,
					};

					db.prescription_form_107у.Add(newPrescription_form_107у);
					db.SaveChanges();
				}

				MessageBox.Show("Рецепт сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

				// Возвращаемся назад
				var parentWindow = Window.GetWindow(this) as MedicalCaseWindow;
				parentWindow?.ShowDirectionsList();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении рецепта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
	}
}
