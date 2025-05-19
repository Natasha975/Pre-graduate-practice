using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ЭМК.Model;
using System.Globalization;
using System.Text;

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		private MedCardDBEntities dbContext;
		private List<Patients> allPatients;

		public Window1()
		{
			InitializeComponent();
			DataContext = this;
			dbContext = new MedCardDBEntities();

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
					.Select(p => new Patients
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

				// Применяем фильтр
				ApplyFilter();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке пациентов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
				filteredPatients = filteredPatients.Where(p => p.lastname.ToLower().Contains(searchText) ||
					p.name.ToLower().Contains(searchText) || (p.surname != null && p.surname.ToLower().Contains(searchText)));
			}

			// Фильтр по СНИЛС
			if (SnilsCheckBox.IsChecked == true && !string.IsNullOrWhiteSpace(SnilsTextBox.Text))
			{
				var searchText = SnilsTextBox.Text;
				filteredPatients = filteredPatients.Where(p => p.Snils.Contains(searchText));
			}

			if (PolCheckBox.IsChecked == true && !string.IsNullOrWhiteSpace(PolTextBox.Text))
			{
				var searchText = PolTextBox.Text;
				filteredPatients = filteredPatients.Where(p => p.Age.Contains(searchText));
			}

			// Фильтр по полису
			if (EnpCheckBox.IsChecked == true && !string.IsNullOrWhiteSpace(EnpTextBox.Text))
			{
				var searchText = EnpTextBox.Text;
				filteredPatients = filteredPatients.Where(p => p.Policy.Contains(searchText));
			}

			// Фильтр по телефону
			if (PhoneCheckBox.IsChecked == true && !string.IsNullOrWhiteSpace(PhoneTextBox.Text))
			{
				var searchText = PhoneTextBox.Text;
				filteredPatients = filteredPatients.Where(p => p.Phone != null && p.Phone.Contains(searchText));
			}

			// Фильтр по дате рождения
			if (BirthDateCheckBox.IsChecked == true && BirthDatePicker.SelectedDate != null)
			{
				var selectedDate = BirthDatePicker.SelectedDate.Value;
				filteredPatients = filteredPatients.Where(p => p.birthday.Year == selectedDate.Year &&
					p.birthday.Month == selectedDate.Month && p.birthday.Day == selectedDate.Day);
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
			if (DGPatients.SelectedItem is Patients selectedPatient)
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

		private void ExportToPdfButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var saveFileDialog = new Microsoft.Win32.SaveFileDialog
				{
					Filter = "PDF files (*.pdf)|*.pdf",
					FileName = $"Список пациентов {DateTime.Now:dd-MM-yyyy}.pdf"
				};

				if (saveFileDialog.ShowDialog() == true)
				{
					// Создаем документ PDF с альбомной ориентацией
					Document document = new Document(PageSize.A4.Rotate());

					using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
					{
						PdfWriter writer = PdfWriter.GetInstance(document, fs);
						document.Open();

						// Шрифты
						BaseFont baseFont = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

						Font titleFont = new Font(baseFont, 16, Font.BOLD);
						Font headerFont = new Font(baseFont, 10, Font.BOLD);
						Font regularFont = new Font(baseFont, 9);
						Font dateFont = new Font(baseFont, 10, Font.ITALIC);

						// Заголовок
						Paragraph title = new Paragraph("Список пациентов", titleFont);
						title.Alignment = Element.ALIGN_CENTER;
						title.SpacingAfter = 20f;
						document.Add(title);

						// Дата формирования
						Paragraph dateParagraph = new Paragraph($"Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm}", dateFont);
						dateParagraph.Alignment = Element.ALIGN_LEFT;
						document.Add(dateParagraph);

						// Пустая строка
						document.Add(new Paragraph(" ")); 

						// Создаем таблицу
						PdfPTable table = new PdfPTable(DGPatients.Columns.Count);
						table.WidthPercentage = 100;
						table.SpacingBefore = 10f;
						table.SpacingAfter = 10f;

						// Настройка ширины столбцов
						float[] columnWidths = new float[] { 2f, 1.5f, 1f, 1.5f, 2f, 1.5f, 1.5f, 1f };
						table.SetWidths(columnWidths);

						// Добавляем заголовки колонок
						foreach (DataGridColumn column in DGPatients.Columns)
						{
							if (column is DataGridTextColumn textColumn)
							{
								PdfPCell headerCell = new PdfPCell(new Phrase(textColumn.Header.ToString(), headerFont));
								headerCell.BackgroundColor = new BaseColor(220, 220, 220);
								headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
								headerCell.Padding = 5;
								table.AddCell(headerCell);
							}
						}

						// Добавляем данные пациентов
						foreach (Patients patient in DGPatients.Items)
						{
							AddCell(table, patient.FullName ?? "-", regularFont);
							DateTime birthDate;
							var dateString = DateTime.TryParse(patient.BirthDate, out birthDate)
								? birthDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture) : patient.BirthDate ?? "";
							table.AddCell(new Phrase(dateString, regularFont));
							AddCell(table, patient.Age ?? "-", regularFont);
							AddCell(table, patient.Snils ?? "-", regularFont);
							AddCell(table, patient.Enp ?? "-", regularFont);
							AddCell(table, patient.Phone ?? "-", regularFont);
							AddCell(table, patient.Policy ?? "-", regularFont);
							AddCell(table, patient.Pol ?? "-", regularFont);
						}

						document.Add(table);
						document.Close();
					}

					Process.Start(new ProcessStartInfo(saveFileDialog.FileName) { UseShellExecute = true });
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при экспорте в PDF: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// Вспомогательный метод для добавления ячеек
		private void AddCell(PdfPTable table, string text, Font font)
		{
			PdfPCell cell = new PdfPCell(new Phrase(text, font));
			cell.Padding = 4;
			cell.HorizontalAlignment = Element.ALIGN_LEFT;
			table.AddCell(cell);
		}

		// Обработчик кнопки обновления статистики
		private void UpdateStatsButton_Click(object sender, RoutedEventArgs e)
		{
			GenerateGenderReport();
		}

		// Метод для генерации отчета по полу
		private void GenerateGenderReport()
		{
			try
			{
				if (allPatients == null || allPatients.Count == 0)
				{
					GenderStatsText.Text = "Нет данных для отображения статистики";
					return;
				}

				// Получаем статистику по полу с явным преобразованием в dynamic
				var genderStats = allPatients
					.GroupBy(p => p.Pol ?? "Не указан")
					.Select(g => new
					{
						Gender = g.Key,
						Count = g.Count(),
						Percentage = (double)g.Count() / allPatients.Count * 100
					})
					.OrderByDescending(x => x.Count)
					.Cast<dynamic>() // Преобразуем в dynamic
					.ToList();

				// Обновляем текстовую статистику
				UpdateGenderStatsText(genderStats);

				// Рисуем диаграмму
				DrawGenderChart(genderStats);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при формировании отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// Обновление текстовой статистики
		private void UpdateGenderStatsText(List<dynamic> genderStats)
		{
			if (genderStats == null || genderStats.Count == 0)
			{
				GenderStatsText.Text = "Нет данных о поле пациентов";
				return;
			}

			var statsText = new StringBuilder();
			statsText.AppendLine($"Всего пациентов: {allPatients.Count}");
			statsText.AppendLine();

			foreach (var stat in genderStats)
			{
				statsText.AppendLine($"{stat.Gender}: {stat.Count} ({stat.Percentage:F1}%)");
			}

			GenderStatsText.Text = statsText.ToString();
		}

		// Метод для рисования диаграммы
		private void DrawGenderChart(List<dynamic> genderStats)
		{
			GenderChartCanvas.Children.Clear();

			if (genderStats == null || genderStats.Count == 0) return;

			const int margin = 30;
			double canvasWidth = GenderChartCanvas.ActualWidth - 2 * margin;
			double canvasHeight = GenderChartCanvas.ActualHeight - 2 * margin;
			double maxCount = genderStats.Max(g => (double)g.Count);
			if (maxCount == 0) maxCount = 1;

			// Цвета для разных полов
			var genderColors = new Dictionary<string, Brush>
			{
				{ "Мужской", Brushes.SteelBlue },
				{ "Женский", Brushes.LightPink },
				{ "Не указан", Brushes.LightGray }
			};

			// Рисуем столбцы диаграммы
			double columnWidth = canvasWidth / (genderStats.Count * 2);
			double spacing = columnWidth / 2;

			for (int i = 0; i < genderStats.Count; i++)
			{
				var stat = genderStats[i];
				double columnHeight = (stat.Count / maxCount) * canvasHeight;
				double left = margin + i * (columnWidth + spacing);
				double top = margin + canvasHeight - columnHeight;

				// Получаем цвет для текущего пола (явно указываем тип Brush)
				if (!genderColors.TryGetValue(stat.Gender.ToString(), out Brush color))
				{
					color = Brushes.Gray;
				}

				// Рисуем столбец (используем System.Windows.Shapes.Rectangle)
				var column = new System.Windows.Shapes.Rectangle
				{
					Width = columnWidth,
					Height = columnHeight,
					Fill = color,
					Stroke = Brushes.DarkSlateBlue,
					StrokeThickness = 1
				};

				Canvas.SetLeft(column, left);
				Canvas.SetTop(column, top);
				GenderChartCanvas.Children.Add(column);

				// Подпись количества
				var countLabel = new TextBlock
				{
					Text = stat.Count.ToString(),
					FontSize = 10,
					Foreground = Brushes.Black
				};

				Canvas.SetLeft(countLabel, left + columnWidth / 2 - 5);
				Canvas.SetTop(countLabel, top - 20);
				GenderChartCanvas.Children.Add(countLabel);

				// Подпись пола
				var genderLabel = new TextBlock
				{
					Text = stat.Gender.ToString(),
					FontSize = 10,
					Foreground = Brushes.Black,
					TextAlignment = TextAlignment.Center,
					Width = columnWidth + 10
				};

				Canvas.SetLeft(genderLabel, left - 5);
				Canvas.SetTop(genderLabel, margin + canvasHeight + 5);
				GenderChartCanvas.Children.Add(genderLabel);
			}

			// Оси (используем System.Windows.Shapes.Line)
			var xAxis = new System.Windows.Shapes.Line
			{
				X1 = margin,
				Y1 = margin + canvasHeight,
				X2 = margin + canvasWidth,
				Y2 = margin + canvasHeight,
				Stroke = Brushes.Black,
				StrokeThickness = 1
			};

			var yAxis = new System.Windows.Shapes.Line
			{
				X1 = margin,
				Y1 = margin,
				X2 = margin,
				Y2 = margin + canvasHeight,
				Stroke = Brushes.Black,
				StrokeThickness = 1
			};

			GenderChartCanvas.Children.Add(xAxis);
			GenderChartCanvas.Children.Add(yAxis);

			// Подписи оси Y
			for (int i = 0; i <= maxCount; i += Math.Max(1, (int)maxCount / 5))
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
				GenderChartCanvas.Children.Add(label);
			}
		}

		// Обработчик кнопки обновления возрастной статистики
		private void UpdateAgeStatsButton_Click(object sender, RoutedEventArgs e)
		{
			GenerateAgeGroupReport();
		}

		// Метод для генерации отчета по возрастным группам
		private void GenerateAgeGroupReport()
		{
			try
			{
				if (allPatients == null || allPatients.Count == 0)
				{
					AgeStatsText.Text = "Нет данных для отображения статистики";
					return;
				}

				// Добавим отладочный вывод для проверки данных
				Debug.WriteLine($"Всего пациентов: {allPatients.Count}");
				foreach (var patient in allPatients.Take(5))
				{
					Debug.WriteLine($"Пациент: {patient.FullName}, Возраст: {patient.Age}");
				}

				var ageStats = allPatients
					.Select(p => new
					{
						AgeGroup = GetAgeGroup(p.Age),
						Age = p.Age
					})
					.GroupBy(g => g.AgeGroup)
					.Select(g => new
					{
						Group = g.Key,
						Count = g.Count(),
						Percentage = (double)g.Count() / allPatients.Count * 100
					})
					.OrderByDescending(x => x.Count)
					.Cast<dynamic>()
					.ToList();

				// Отладочный вывод статистики
				Debug.WriteLine("Статистика по возрастным группам:");
				foreach (var stat in ageStats)
				{
					Debug.WriteLine($"{stat.Group}: {stat.Count} пациентов");
				}

				UpdateAgeStatsText(ageStats);
				DrawAgeChart(ageStats);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при формировании отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}		

		// Метод для определения возрастной группы
		private string GetAgeGroup(string ageString)
		{
			if (!int.TryParse(ageString, out int age)) return "Не указан";

			if (age >= 18 && age <= 44)
				return "Молодой возраст (18-44)";
			else if (age >= 45 && age <= 59)
				return "Средний возраст (45-59)";
			else if (age >= 60 && age <= 74)
				return "Пожилой возраст (60-74)";
			else if (age >= 75 && age <= 90)
				return "Старческий возраст (75-90)";
			else if (age > 90)
				return "Долголетие (90+)";
			else
				return "Дети (до 18)";
		}

		// Обновление текстовой статистики по возрастам
		private void UpdateAgeStatsText(List<dynamic> ageStats)
		{
			if (ageStats == null || ageStats.Count == 0)
			{
				AgeStatsText.Text = "Нет данных о возрасте пациентов";
				return;
			}

			var statsText = new StringBuilder();
			statsText.AppendLine($"Всего пациентов: {allPatients.Count}");
			statsText.AppendLine();

			foreach (var stat in ageStats)
			{
				statsText.AppendLine($"{stat.Group}: {stat.Count} ({stat.Percentage:F1}%)");
			}

			AgeStatsText.Text = statsText.ToString();
		}

		// Метод для рисования диаграммы возрастных групп
		private void DrawAgeChart(List<dynamic> ageStats)
		{
			AgeChartCanvas.Children.Clear();

			if (ageStats == null || ageStats.Count == 0)
			{
				Debug.WriteLine("Нет данных для построения диаграммы");
				return;
			}

			// Проверка размеров Canvas
			if (AgeChartCanvas.ActualWidth <= 0 || AgeChartCanvas.ActualHeight <= 0)
			{
				Debug.WriteLine($"Размеры Canvas: {AgeChartCanvas.ActualWidth}x{AgeChartCanvas.ActualHeight}");
				// Установите явные размеры для теста
				AgeChartCanvas.Width = 500;
				AgeChartCanvas.Height = 300;
			}

			AgeChartCanvas.Children.Clear();

			if (ageStats == null || ageStats.Count == 0) return;

			const int margin = 30;
			double canvasWidth = AgeChartCanvas.ActualWidth - 2 * margin;
			double canvasHeight = AgeChartCanvas.ActualHeight - 2 * margin;
			double maxCount = ageStats.Max(g => (double)g.Count);
			if (maxCount == 0) maxCount = 1;

			// Цвета для разных возрастных групп
			var ageColors = new Dictionary<string, Brush>
			{
				{ "Молодой возраст (18-44)", Brushes.LightGreen },
				{ "Средний возраст (45-59)", Brushes.LightBlue },
				{ "Пожилой возраст (60-74)", Brushes.Orange },
				{ "Старческий возраст (75-90)", Brushes.Tomato },
				{ "Долголетие (90+)", Brushes.Purple },
				{ "Дети (до 18)", Brushes.Pink },
				{ "Не указан", Brushes.LightGray }
			};

			// Рисуем столбцы диаграммы
			double columnWidth = canvasWidth / (ageStats.Count * 2);
			double spacing = columnWidth / 2;

			for (int i = 0; i < ageStats.Count; i++)
			{
				var stat = ageStats[i];
				double columnHeight = (stat.Count / maxCount) * canvasHeight;
				double left = margin + i * (columnWidth + spacing);
				double top = margin + canvasHeight - columnHeight;

				// Получаем цвет для текущей возрастной группы
				if (!ageColors.TryGetValue(stat.Group.ToString(), out Brush color))
				{
					color = Brushes.Gray;
				}

				// Рисуем столбец
				var column = new System.Windows.Shapes.Rectangle
				{
					Width = columnWidth,
					Height = columnHeight,
					Fill = color,
					Stroke = Brushes.DarkSlateBlue,
					StrokeThickness = 1
				};

				Canvas.SetLeft(column, left);
				Canvas.SetTop(column, top);
				AgeChartCanvas.Children.Add(column);

				// Подпись количества
				var countLabel = new TextBlock
				{
					Text = stat.Count.ToString(),
					FontSize = 10,
					Foreground = Brushes.Black
				};

				Canvas.SetLeft(countLabel, left + columnWidth / 2 - 5);
				Canvas.SetTop(countLabel, top - 20);
				AgeChartCanvas.Children.Add(countLabel);

				// Подпись возрастной группы (с переносом слов)
				var ageLabel = new TextBlock
				{
					Text = stat.Group.ToString(),
					FontSize = 10,
					Foreground = Brushes.Black,
					TextAlignment = TextAlignment.Center,
					TextWrapping = TextWrapping.Wrap,
					Width = columnWidth + 10
				};

				Canvas.SetLeft(ageLabel, left - 5);
				Canvas.SetTop(ageLabel, margin + canvasHeight + 5);
				AgeChartCanvas.Children.Add(ageLabel);
			}

			// Оси
			var xAxis = new System.Windows.Shapes.Line
			{
				X1 = margin,
				Y1 = margin + canvasHeight,
				X2 = margin + canvasWidth,
				Y2 = margin + canvasHeight,
				Stroke = Brushes.Black,
				StrokeThickness = 1
			};

			var yAxis = new System.Windows.Shapes.Line
			{
				X1 = margin,
				Y1 = margin,
				X2 = margin,
				Y2 = margin + canvasHeight,
				Stroke = Brushes.Black,
				StrokeThickness = 1
			};

			AgeChartCanvas.Children.Add(xAxis);
			AgeChartCanvas.Children.Add(yAxis);

			// Подписи оси Y
			for (int i = 0; i <= maxCount; i += Math.Max(1, (int)maxCount / 5))
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
				AgeChartCanvas.Children.Add(label);
			}
		}
	}
}
