using MaterialDesignThemes.Wpf;
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
using System.Windows.Shapes;
using ЭМК.Model;

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для MedicalCaseWindowReg.xaml
	/// </summary>
	public partial class MedicalCaseWindowReg : Window
	{
		public MedicalCaseWindowReg(MedicalCase medicalCase)
		{
			InitializeComponent();
			LoadMedicalCaseData(medicalCase);
		}

		private void LoadMedicalCaseData(MedicalCase medicalCase)
		{
			try
			{
				// Основные данные пациента
				txtPatientName.Text = medicalCase.PatientName;
				txtPatientBirthDate.Text = medicalCase.PatientBirthDate;
				txtPatientEnp.Text = medicalCase.PatientEnp;
				txtPatientSnils.Text = medicalCase.PatientSnils;
				txtPatientAge.Text = medicalCase.PatientAge;

				// Данные осмотра
				NumberTextBlock.Text = medicalCase.IdInspection.ToString();
				dateTextBlock.Text = medicalCase.Date;
				timePicker.Text = medicalCase.Time;
				txtDoctorInfo.Text = medicalCase.DoctorName;
				txtDoctorInfoTwo.Text = medicalCase.DoctorName;

				txtPlaceOfService.Text = medicalCase.PlaceOfService;
				txtCaseTypeService.Text = medicalCase.TypeOfServiceCase;
				txtTypeOfPayment.Text = medicalCase.TypeOfPayment;
				txtPurposeOfTheService.Text = medicalCase.PurposeOfTheService;

				// Физические параметры с проверкой на null
				txtHeight.Text = medicalCase.Height.ToString() ?? "";
				txtWeight.Text = medicalCase.Weight.ToString() ?? "";
				txtBloodPressureUpper.Text = medicalCase.BloodPressureUpper.ToString() ?? "";
				txtBloodPressureLower.Text = medicalCase.BloodPressureLower?.ToString() ?? "";
				txtTemperature.Text = medicalCase.Temperature?.ToString() ?? "";
				txtHeartRate.Text = medicalCase.HeartRate?.ToString() ?? "";
				txtRespiratoryRate.Text = medicalCase.RespiratoryRate?.ToString() ?? "";
				txtOxygenSaturation.Text = medicalCase.OxygenSaturation?.ToString() ?? "";

				// Диагнозы и другая информация
				txtPatientCondition.Text = medicalCase.PatientCondition;
				txtPreliminaryDiagnosis.Text = medicalCase.PreliminaryDiagnosis;
				txtTheMainDiagnosis.Text = medicalCase.TheMainDiagnosis;
				tbMedicalHistory.Text =medicalCase.Anamnesis;
				txtSuspicionOfHeat.Text = medicalCase.SuspicionOfHeat;
				ComplaintsTextBox.Text = medicalCase.Complaints ?? "";
				txtTreatment.Text = medicalCase.Treatment ?? "";
				txtRecommendations.Text = medicalCase.Recommendations ?? "";

				// Загружаем направления для данного осмотра
				LoadReferrals(medicalCase.IdInspection);
				LoadPrescriptions(medicalCase.IdInspection);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void LoadReferrals(int inspectionId)
		{
			try
			{
				using (var db = new MedCardDBEntities())
				{
					// Получаем все направления для данного осмотра
					var referrals = db.referral
						.Where(r => r.id_inspection == inspectionId)
						.OrderBy(r => r.date_of_creation)
						.ToList();

					// Преобразуем в список Direction для отображения
					var directions = referrals.Select(r => new Direction
					{
						Id = r.id_referral,
						DirectionNumber = $"Н-{r.id_referral}",
						DirectionType = r.type_of_direction,
						Service = r.service,
						AppointmentDate = r.date_of_admission ?? DateTime.Now,
						Organization = r.organization,
						Doctor = r.doctor
					}).ToList();

					// Загружаем направления в интерфейс
					LoadDirections(directions);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке направлений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void LoadPrescriptions(int inspectionId)
		{
			try
			{
				using (var db = new MedCardDBEntities())
				{
					var prescriptionList = new List<Prescription>();

					// Загружаем рецепты формы 107-1/у
					var prescriptions107 = db.prescription_form_107у
						.Where(p => p.id_inspection == inspectionId)
						.Select(p => new Prescription
						{
							number = p.number,
							type = "Форма 107-1/у",
							name_of_the_drug = p.name_of_the_drug,
							date = p.date,
							dosage = p.dosage,
							// остальные свойства
						});
					prescriptionList.AddRange(prescriptions107);

					//// Загружаем рецепты формы 148-1/у-88 (если есть такая таблица)
					//var prescriptions14888 = db.prescription_form_14888у
					//	.Where(p => p.id_inspection == inspectionId)
					//	.Select(p => new Prescription
					//	{
					//		number = p.number,
					//		type = "Форма 148-1/у-88",
					//		name_of_the_drug = p.name_of_the_drug,
					//		date = p.date,
					//		dosage = p.dosage,
					//		// остальные свойства
					//	});
					//prescriptionList.AddRange(prescriptions14888);

					//// Загружаем рецепты формы 148-1/у-04(л) (если есть такая таблица)
					//var prescriptions14804 = db.prescription_form_14804у
					//	.Where(p => p.id_inspection == inspectionId)
					//	.Select(p => new Prescription
					//	{
					//		number = p.number,
					//		type = "Форма 148-1/у-04(л)",
					//		name_of_the_drug = p.name_of_the_drug,
					//		date = p.date,
					//		dosage = p.dosage,
					//		// остальные свойства
					//	});
					//prescriptionList.AddRange(prescriptions14804);

					// Сортируем по дате
					prescriptionList = prescriptionList.OrderBy(p => p.date).ToList();

					// Загружаем рецепты в интерфейс
					LoadPrescriptionsList(prescriptionList);

					//// Получаем все рецепты для данного осмотра
					//var prescriptions = db.prescription_form_107у
					//	.Where(p => p.id_inspection == inspectionId)
					//	.ToList();

					//// Преобразуем в список Prescription для отображения
					//var prescriptionList = prescriptions.Select(p => new Prescription { 
					//	number = p.number,
					//	name_of_the_drug = p.name_of_the_drug,
					//	date = p.date,
					//	dosage = p.dosage,
					//	method_of_administration = p.method_of_administration,
					//	method_of_administration_details = p.method_of_administration_details,
					//	dosage_regimen = p.dosage_regimen,
					//	dosage_regimen_optional = p.dosage_regimen_optional,
					//	duration_of_treatment_number = p.duration_of_treatment_number,
					//	duration_of_treatment_duration = p.duration_of_treatment_duration,
					//	duration_of_treatment_comments = p.duration_of_treatment_comments,
					//	justification_of_appointment = p.justification_of_appointment,
					//}).ToList();

					//// Загружаем рецепты в интерфейс
					//LoadPrescriptionsList(prescriptionList);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке рецептов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void LoadPrescriptionsList(List<Prescription> prescriptions)
		{
			// Очищаем существующий контент
			PrescriptionsListScrollViewer.Content = null;

			if (prescriptions == null || prescriptions.Count == 0)
			{
				var noPrescriptionsText = new TextBlock
				{
					Text = "Нет выписанных рецептов",
					Margin = new Thickness(10),
					FontStyle = FontStyles.Italic
				};
				PrescriptionsListScrollViewer.Content = noPrescriptionsText;
				return;
			}

			// Создаем таблицу для отображения рецептов
			var grid = new Grid();
			grid.Margin = new Thickness(5);

			// Добавляем колонки
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });

			// Добавляем заголовки
			AddPrescriptionHeader(grid, "№", 0, 0);
			AddPrescriptionHeader(grid, "Тип рецепта", 1, 0);
			AddPrescriptionHeader(grid, "Препарат", 2, 0);
			AddPrescriptionHeader(grid, "Дата выписки", 3, 0);
			AddPrescriptionHeader(grid, "Действия", 4, 0);

			// Добавляем строки с рецептами
			for (int i = 0; i < prescriptions.Count; i++)
			{
				var prescription = prescriptions[i];

				// Номер рецепта
				AddPrescriptionCell(grid, prescription.number.ToString(), 0, i + 1);

				// Тип рецепта
				AddPrescriptionCell(grid, prescription.type, 1, i + 1);

				// Препарат и дозировка
				AddPrescriptionCell(grid, $"{prescription.name_of_the_drug} ({prescription.dosage})", 2, i + 1);

				// Дата выписки
				AddPrescriptionCell(grid, prescription.date.ToString("dd.MM.yyyy"), 3, i + 1);

				// Кнопка просмотра
				var viewButton = new Button
				{
					Content = "Просмотр",
					Margin = new Thickness(2),
					Tag = new Tuple<string, int>(prescription.type, prescription.number), // Сохраняем и тип, и номер
					Style = (Style)FindResource("MaterialDesignFlatButton")
				};
				viewButton.Click += ViewPrescriptionButton_Click;

				Grid.SetColumn(viewButton, 4);
				Grid.SetRow(viewButton, i + 1);
				grid.Children.Add(viewButton);
			}

			PrescriptionsListScrollViewer.Content = grid;

			//	// Очищаем существующий контент
			//	PrescriptionsListScrollViewer.Content = null;

			//	if (prescriptions == null || prescriptions.Count == 0)
			//	{
			//		var noPrescriptionsText = new TextBlock
			//		{
			//			Text = "Нет выписанных рецептов",
			//			Margin = new Thickness(10),
			//			FontStyle = FontStyles.Italic
			//		};
			//		PrescriptionsListScrollViewer.Content = noPrescriptionsText;
			//		return;
			//	}

			//	// Создаем таблицу для отображения рецептов
			//	var grid = new Grid();
			//	grid.Margin = new Thickness(5);

			//	// Добавляем колонки
			//	grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
			//	grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
			//	grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
			//	grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
			//	grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });

			//	// Добавляем заголовки
			//	AddPrescriptionHeader(grid, "№", 0, 0);
			//	AddPrescriptionHeader(grid, "Тип рецепта", 1, 0);
			//	AddPrescriptionHeader(grid, "Препарат", 2, 0);
			//	AddPrescriptionHeader(grid, "Дата выписки", 3, 0);
			//	AddPrescriptionHeader(grid, "Действия", 4, 0);

			//	// Добавляем строки с рецептами
			//	for (int i = 0; i < prescriptions.Count; i++)
			//	{
			//		var prescription = prescriptions[i];

			//		// Номер рецепта
			//		AddPrescriptionCell(grid, prescription.number.ToString(), 0, i + 1);

			//		// Препарат и дозировка
			//		AddPrescriptionCell(grid, $"{prescription.name_of_the_drug} ({prescription.dosage})", 2, i + 1);

			//		// Дата выписки
			//		AddPrescriptionCell(grid, prescription.date.ToString("dd.MM.yyyy"), 3, i + 1);

			//		// Кнопка просмотра
			//		var viewButton = new Button
			//		{
			//			Content = "Просмотр",
			//			Margin = new Thickness(2),
			//			Tag = prescription.number,
			//			Style = (Style)FindResource("MaterialDesignFlatButton")
			//		};
			//		viewButton.Click += ViewPrescriptionButton_Click;

			//		Grid.SetColumn(viewButton, 4);
			//		Grid.SetRow(viewButton, i + 1);
			//		grid.Children.Add(viewButton);
			//	}

			//	PrescriptionsListScrollViewer.Content = grid;
		}

		private void AddPrescriptionHeader(Grid grid, string text, int column, int row)
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

		private void AddPrescriptionCell(Grid grid, string text, int column, int row)
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

		private void ViewPrescriptionButton_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			var tag = button.Tag as Tuple<string, int>;
			string prescriptionType = tag.Item1;
			int prescriptionId = tag.Item2;

			try
			{
				using (var db = new MedCardDBEntities())
				{
					switch (prescriptionType)
					{
						case "Форма 107-1/у":
							var prescription107 = db.prescription_form_107у.FirstOrDefault(p => p.number == prescriptionId);
							if (prescription107 != null)
							{
								ShowPrescriptionForm107(prescription107);
							}
							break;

						//case "Форма 148-1/у-88":
						//	var prescription14888 = db.prescription_form_14888у.FirstOrDefault(p => p.number == prescriptionId);
						//	if (prescription14888 != null)
						//	{
						//		ShowPrescriptionForm14888(prescription14888);
						//	}
						//	break;

						//case "Форма 148-1/у-04(л)":
						//	var prescription14804 = db.prescription_form_14804у.FirstOrDefault(p => p.number == prescriptionId);
						//	if (prescription14804 != null)
						//	{
						//		ShowPrescriptionForm14804(prescription14804);
						//	}
						//	break;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке рецепта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			//var button = sender as Button;
			//int prescriptionId = (int)button.Tag;

			//try
			//{
			//	using (var db = new diplomEntities())
			//	{
			//		var prescription = db.prescription_form_107у.FirstOrDefault(p => p.number == prescriptionId);
			//		if (prescription != null)
			//		{
			//			// Создаем FlowDocument с макетом рецепта
			//			FlowDocument doc = new FlowDocument
			//			{
			//				PageWidth = 800,
			//				PagePadding = new Thickness(40),
			//				ColumnWidth = 700,
			//				FontFamily = new FontFamily("Arial"),
			//				FontSize = 12
			//			};

			//			// Добавляем заголовок
			//			Paragraph header = new Paragraph(new Run("РЕЦЕПТ"))
			//			{
			//				FontSize = 16,
			//				FontWeight = FontWeights.Bold,
			//				TextAlignment = TextAlignment.Center,
			//				Margin = new Thickness(0, 0, 0, 20)
			//			};
			//			doc.Blocks.Add(header);

			//			// Добавляем номер и дату рецепта
			//			Paragraph numberParagraph = new Paragraph();
			//			numberParagraph.Inlines.Add(new Run($"№ Р-{prescription.number}"));
			//			numberParagraph.Inlines.Add(new Run($" от {prescription.date.ToString("dd.MM.yyyy")}"));
			//			doc.Blocks.Add(numberParagraph);

			//			// Добавляем разделитель
			//			doc.Blocks.Add(new Paragraph(new Run(new string('_', 100)))
			//			{
			//				Margin = new Thickness(0, 10, 0, 20)
			//			});

			//			// Добавляем информацию о пациенте
			//			Table patientTable = new Table();
			//			patientTable.CellSpacing = 0;
			//			patientTable.Margin = new Thickness(0, 0, 0, 20);

			//			// Создаем колонки
			//			patientTable.Columns.Add(new TableColumn { Width = new GridLength(150) });
			//			patientTable.Columns.Add(new TableColumn { Width = new GridLength(500) });

			//			// Создаем строки
			//			patientTable.RowGroups.Add(new TableRowGroup());

			//			// Пациент
			//			TableRow patientRow = new TableRow();
			//			patientRow.Cells.Add(new TableCell(new Paragraph(new Run("Пациент:")) { FontWeight = FontWeights.Bold }));
			//			patientRow.Cells.Add(new TableCell(new Paragraph(new Run($"{txtPatientName.Text} ({txtPatientBirthDate.Text})"))));
			//			patientTable.RowGroups[0].Rows.Add(patientRow);

			//			// СНИЛС
			//			TableRow snilsRow = new TableRow();
			//			snilsRow.Cells.Add(new TableCell(new Paragraph(new Run("СНИЛС:")) { FontWeight = FontWeights.Bold }));
			//			snilsRow.Cells.Add(new TableCell(new Paragraph(new Run(txtPatientSnils.Text))));
			//			patientTable.RowGroups[0].Rows.Add(snilsRow);

			//			doc.Blocks.Add(patientTable);

			//			// Добавляем информацию о рецепте
			//			Table prescriptionTable = new Table();
			//			prescriptionTable.CellSpacing = 0;
			//			prescriptionTable.Margin = new Thickness(0, 0, 0, 20);

			//			// Создаем колонки
			//			prescriptionTable.Columns.Add(new TableColumn { Width = new GridLength(150) });
			//			prescriptionTable.Columns.Add(new TableColumn { Width = new GridLength(500) });

			//			// Создаем строки
			//			prescriptionTable.RowGroups.Add(new TableRowGroup());

			//			// Препарат
			//			TableRow drugRow = new TableRow();
			//			drugRow.Cells.Add(new TableCell(new Paragraph(new Run("Препарат:")) { FontWeight = FontWeights.Bold }));
			//			drugRow.Cells.Add(new TableCell(new Paragraph(new Run(prescription.name_of_the_drug))));
			//			prescriptionTable.RowGroups[0].Rows.Add(drugRow);

			//			// Дозировка
			//			TableRow dosageRow = new TableRow();
			//			dosageRow.Cells.Add(new TableCell(new Paragraph(new Run("Дозировка:")) { FontWeight = FontWeights.Bold }));
			//			dosageRow.Cells.Add(new TableCell(new Paragraph(new Run(prescription.dosage))));
			//			prescriptionTable.RowGroups[0].Rows.Add(dosageRow);

			//			// Способ применения
			//			TableRow usageRow = new TableRow();
			//			usageRow.Cells.Add(new TableCell(new Paragraph(new Run("Способ применения:")) { FontWeight = FontWeights.Bold }));
			//			usageRow.Cells.Add(new TableCell(new Paragraph(new Run(prescription.method_of_administration))));
			//			prescriptionTable.RowGroups[0].Rows.Add(usageRow);

			//			// Количество
			//			TableRow quantityRow = new TableRow();
			//			quantityRow.Cells.Add(new TableCell(new Paragraph(new Run("Количество:")) { FontWeight = FontWeights.Bold }));
			//			quantityRow.Cells.Add(new TableCell(new Paragraph(new Run(prescription.method_of_administration_details.ToString()))));
			//			prescriptionTable.RowGroups[0].Rows.Add(quantityRow);

			//			// Срок действия
			//			TableRow validityRow = new TableRow();
			//			validityRow.Cells.Add(new TableCell(new Paragraph(new Run("Срок действия:")) { FontWeight = FontWeights.Bold }));
			//			validityRow.Cells.Add(new TableCell(new Paragraph(new Run(20 + " дней"))));
			//			prescriptionTable.RowGroups[0].Rows.Add(validityRow);

			//			doc.Blocks.Add(prescriptionTable);

			//			// Добавляем подпись
			//			Paragraph signParagraph = new Paragraph();
			//			signParagraph.Inlines.Add(new LineBreak());
			//			signParagraph.Inlines.Add(new LineBreak());

			//			// Врач (левое выравнивание)
			//			signParagraph.Inlines.Add(new Run("Врач, выписавший рецепт: " + txtDoctorInfo.Text));
			//			signParagraph.Inlines.Add(new LineBreak());

			//			// Подпись и М.П. (центрирование через отдельный Span)
			//			Span centeredSpan = new Span();
			//			centeredSpan.Inlines.Add(new Run("Подпись: _______________________"));
			//			centeredSpan.Inlines.Add(new LineBreak());
			//			centeredSpan.Inlines.Add(new Run("М. П."));
			//			signParagraph.Inlines.Add(centeredSpan);

			//			// Устанавливаем выравнивание для всего параграфа
			//			signParagraph.TextAlignment = TextAlignment.Left;
			//			doc.Blocks.Add(signParagraph);

			//			// Создаем окно для просмотра документа
			//			Window viewerWindow = new Window
			//			{
			//				Title = $"Рецепт Р-{prescription.number}",
			//				Width = 850,
			//				Height = 550,
			//				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			//				Owner = this
			//			};

			//			FlowDocumentScrollViewer viewer = new FlowDocumentScrollViewer
			//			{
			//				Document = doc,
			//				VerticalScrollBarVisibility = ScrollBarVisibility.Auto
			//			};

			//			// Добавляем кнопки
			//			DockPanel dockPanel = new DockPanel();

			//			Button printButton = new Button
			//			{
			//				Content = "Печать",
			//				Width = 100,
			//				Margin = new Thickness(5),
			//				HorizontalAlignment = HorizontalAlignment.Right
			//			};
			//			printButton.Click += (s, args) =>
			//			{
			//				PrintDocument(doc);
			//				viewerWindow.Close();
			//			};

			//			DockPanel.SetDock(printButton, Dock.Bottom);
			//			dockPanel.Children.Add(printButton);
			//			dockPanel.Children.Add(viewer);

			//			viewerWindow.Content = dockPanel;
			//			viewerWindow.ShowDialog();
			//		}
			//	}
			//}
			//catch (Exception ex)
			//{
			//	MessageBox.Show($"Ошибка при загрузке рецепта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			//} 
		}

		private void ShowPrescriptionForm107(prescription_form_107у prescription)
		{
			try
			{
				using (var db = new MedCardDBEntities())
				{
						// Создаем FlowDocument с макетом рецепта
						FlowDocument doc = new FlowDocument
						{
							PageWidth = 800,
							PagePadding = new Thickness(40),
							ColumnWidth = 700,
							FontFamily = new FontFamily("Arial"),
							FontSize = 12
						};

						// Добавляем заголовок
						Paragraph header = new Paragraph(new Run("РЕЦЕПТ"))
						{
							FontSize = 16,
							FontWeight = FontWeights.Bold,
							TextAlignment = TextAlignment.Center,
							Margin = new Thickness(0, 0, 0, 20)
						};
						doc.Blocks.Add(header);

						// Добавляем номер и дату рецепта
						Paragraph numberParagraph = new Paragraph();
						numberParagraph.Inlines.Add(new Run($"№ Р-{prescription.number}"));
						numberParagraph.Inlines.Add(new Run($" от {prescription.date.ToString("dd.MM.yyyy")}"));
						doc.Blocks.Add(numberParagraph);

						// Добавляем разделитель
						doc.Blocks.Add(new Paragraph(new Run(new string('_', 100)))
						{
							Margin = new Thickness(0, 10, 0, 20)
						});

						// Добавляем информацию о пациенте
						Table patientTable = new Table();
						patientTable.CellSpacing = 0;
						patientTable.Margin = new Thickness(0, 0, 0, 20);

						// Создаем колонки
						patientTable.Columns.Add(new TableColumn { Width = new GridLength(150) });
						patientTable.Columns.Add(new TableColumn { Width = new GridLength(500) });

						// Создаем строки
						patientTable.RowGroups.Add(new TableRowGroup());

						// Пациент
						TableRow patientRow = new TableRow();
						patientRow.Cells.Add(new TableCell(new Paragraph(new Run("Пациент:")) { FontWeight = FontWeights.Bold }));
						patientRow.Cells.Add(new TableCell(new Paragraph(new Run($"{txtPatientName.Text} ({txtPatientBirthDate.Text})"))));
						patientTable.RowGroups[0].Rows.Add(patientRow);

						// СНИЛС
						TableRow snilsRow = new TableRow();
						snilsRow.Cells.Add(new TableCell(new Paragraph(new Run("СНИЛС:")) { FontWeight = FontWeights.Bold }));
						snilsRow.Cells.Add(new TableCell(new Paragraph(new Run(txtPatientSnils.Text))));
						patientTable.RowGroups[0].Rows.Add(snilsRow);

						doc.Blocks.Add(patientTable);

						// Добавляем информацию о рецепте
						Table prescriptionTable = new Table();
						prescriptionTable.CellSpacing = 0;
						prescriptionTable.Margin = new Thickness(0, 0, 0, 20);

						// Создаем колонки
						prescriptionTable.Columns.Add(new TableColumn { Width = new GridLength(150) });
						prescriptionTable.Columns.Add(new TableColumn { Width = new GridLength(500) });

						// Создаем строки
						prescriptionTable.RowGroups.Add(new TableRowGroup());

						// Препарат
						TableRow drugRow = new TableRow();
						drugRow.Cells.Add(new TableCell(new Paragraph(new Run("Препарат:")) { FontWeight = FontWeights.Bold }));
						drugRow.Cells.Add(new TableCell(new Paragraph(new Run(prescription.name_of_the_drug))));
						prescriptionTable.RowGroups[0].Rows.Add(drugRow);

						// Дозировка
						TableRow dosageRow = new TableRow();
						dosageRow.Cells.Add(new TableCell(new Paragraph(new Run("Дозировка:")) { FontWeight = FontWeights.Bold }));
						dosageRow.Cells.Add(new TableCell(new Paragraph(new Run(prescription.dosage))));
						prescriptionTable.RowGroups[0].Rows.Add(dosageRow);

						// Способ применения
						TableRow usageRow = new TableRow();
						usageRow.Cells.Add(new TableCell(new Paragraph(new Run("Способ применения:")) { FontWeight = FontWeights.Bold }));
						usageRow.Cells.Add(new TableCell(new Paragraph(new Run(prescription.method_of_administration))));
						prescriptionTable.RowGroups[0].Rows.Add(usageRow);

						// Количество
						TableRow quantityRow = new TableRow();
						quantityRow.Cells.Add(new TableCell(new Paragraph(new Run("Количество:")) { FontWeight = FontWeights.Bold }));
						quantityRow.Cells.Add(new TableCell(new Paragraph(new Run(prescription.method_of_administration_details.ToString()))));
						prescriptionTable.RowGroups[0].Rows.Add(quantityRow);

						// Срок действия
						TableRow validityRow = new TableRow();
						validityRow.Cells.Add(new TableCell(new Paragraph(new Run("Срок действия:")) { FontWeight = FontWeights.Bold }));
						validityRow.Cells.Add(new TableCell(new Paragraph(new Run(20 + " дней"))));
						prescriptionTable.RowGroups[0].Rows.Add(validityRow);

						doc.Blocks.Add(prescriptionTable);

						// Добавляем подпись
						Paragraph signParagraph = new Paragraph();
						signParagraph.Inlines.Add(new LineBreak());
						signParagraph.Inlines.Add(new LineBreak());

						// Врач (левое выравнивание)
						signParagraph.Inlines.Add(new Run("Врач, выписавший рецепт: " + txtDoctorInfo.Text));
						signParagraph.Inlines.Add(new LineBreak());

						// Подпись и М.П. (центрирование через отдельный Span)
						Span centeredSpan = new Span();
						centeredSpan.Inlines.Add(new Run("Подпись: _______________________"));
						centeredSpan.Inlines.Add(new LineBreak());
						centeredSpan.Inlines.Add(new Run("М. П."));
						signParagraph.Inlines.Add(centeredSpan);

						// Устанавливаем выравнивание для всего параграфа
						signParagraph.TextAlignment = TextAlignment.Left;
						doc.Blocks.Add(signParagraph);

						// Создаем окно для просмотра документа
						Window viewerWindow = new Window
						{
							Title = $"Рецепт Р-{prescription.number}",
							Width = 850,
							Height = 550,
							WindowStartupLocation = WindowStartupLocation.CenterOwner,
							Owner = this
						};

						FlowDocumentScrollViewer viewer = new FlowDocumentScrollViewer
						{
							Document = doc,
							VerticalScrollBarVisibility = ScrollBarVisibility.Auto
						};

						// Добавляем кнопки
						DockPanel dockPanel = new DockPanel();

						Button printButton = new Button
						{
							Content = "Печать",
							Width = 100,
							Margin = new Thickness(5),
							HorizontalAlignment = HorizontalAlignment.Right
						};
						printButton.Click += (s, args) =>
						{
							PrintDocument(doc);
							viewerWindow.Close();
						};

						DockPanel.SetDock(printButton, Dock.Bottom);
						dockPanel.Children.Add(printButton);
						dockPanel.Children.Add(viewer);

						viewerWindow.Content = dockPanel;
						viewerWindow.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке рецепта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		//private void ShowPrescriptionForm14888(prescription_form_14888у prescription)
		//{
		//	// Логика отображения рецепта формы 148-88
		//	// ...
		//}

		//private void ShowPrescriptionForm14804(prescription_form_14804у prescription)
		//{
		//	// Логика отображения рецепта формы 148-04
		//	// ...
		//}

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
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });

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

				// Номер направления
				AddCell(grid, direction.DirectionNumber, 0, i + 1);

				// Тип направления
				AddCell(grid, direction.DirectionType, 1, i + 1);

				// Услуга
				AddCell(grid, direction.Service, 2, i + 1);

				// Дата приема
				AddCell(grid, direction.AppointmentDate.ToString("dd.MM.yyyy"), 3, i + 1);

				// Кнопка просмотра
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
			}

			DirectionsListScrollViewer.Content = grid;
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

			DirectionViewerHelper.ShowDirectionDetails(
				directionId,
				txtPatientName.Text,
				txtPatientBirthDate.Text,
				txtPatientSnils.Text,
				txtDoctorInfo.Text,
				txtTheMainDiagnosis.Text,
				this);

			//var button = sender as Button;
			//int directionId = (int)button.Tag;

			//try
			//{
			//	using (var db = new diplomEntities())
			//	{
			//		var referral = db.referral.FirstOrDefault(r => r.id_referral == directionId);
			//		if (referral != null)
			//		{
			//			// Создаем FlowDocument с макетом направления
			//			FlowDocument doc = new FlowDocument
			//			{
			//				PageWidth = 800,
			//				PagePadding = new Thickness(40),
			//				ColumnWidth = 700,
			//				FontFamily = new FontFamily("Arial"),
			//				FontSize = 12
			//			};

			//			// Добавляем заголовок
			//			Paragraph header = new Paragraph(new Run("НАПРАВЛЕНИЕ"))
			//			{
			//				FontSize = 16,
			//				FontWeight = FontWeights.Bold,
			//				TextAlignment = TextAlignment.Center,
			//				Margin = new Thickness(0, 0, 0, 20)
			//			};
			//			doc.Blocks.Add(header);

			//			// Добавляем номер и дату направления
			//			Paragraph numberParagraph = new Paragraph();
			//			numberParagraph.Inlines.Add(new Run($"№ Н-{referral.id_referral}"));
			//			numberParagraph.Inlines.Add(new Run($" от {referral.date_of_creation?.ToString("dd.MM.yyyy")}"));
			//			doc.Blocks.Add(numberParagraph);

			//			// Добавляем разделитель
			//			doc.Blocks.Add(new Paragraph(new Run(new string('_', 100)))
			//			{
			//				Margin = new Thickness(0, 10, 0, 20)
			//			});

			//			// Добавляем информацию о пациенте
			//			Table patientTable = new Table();
			//			patientTable.CellSpacing = 0;
			//			patientTable.Margin = new Thickness(0, 0, 0, 20);

			//			// Создаем колонки
			//			patientTable.Columns.Add(new TableColumn { Width = new GridLength(150) });
			//			patientTable.Columns.Add(new TableColumn { Width = new GridLength(500) });

			//			// Создаем строки
			//			patientTable.RowGroups.Add(new TableRowGroup());

			//			// Пациент
			//			TableRow patientRow = new TableRow();
			//			patientRow.Cells.Add(new TableCell(new Paragraph(new Run("Пациент:")) { FontWeight = FontWeights.Bold }));
			//			patientRow.Cells.Add(new TableCell(new Paragraph(new Run($"{txtPatientName.Text} ({txtPatientBirthDate.Text})"))));
			//			patientTable.RowGroups[0].Rows.Add(patientRow);

			//			// СНИЛС
			//			TableRow snilsRow = new TableRow();
			//			snilsRow.Cells.Add(new TableCell(new Paragraph(new Run("СНИЛС:")) { FontWeight = FontWeights.Bold }));
			//			snilsRow.Cells.Add(new TableCell(new Paragraph(new Run(txtPatientSnils.Text))));
			//			patientTable.RowGroups[0].Rows.Add(snilsRow);

			//			doc.Blocks.Add(patientTable);

			//			// Добавляем информацию о направлении
			//			Table directionTable = new Table();
			//			directionTable.CellSpacing = 0;
			//			directionTable.Margin = new Thickness(0, 0, 0, 20);

			//			// Создаем колонки
			//			directionTable.Columns.Add(new TableColumn { Width = new GridLength(150) });
			//			directionTable.Columns.Add(new TableColumn { Width = new GridLength(500) });

			//			// Создаем строки
			//			directionTable.RowGroups.Add(new TableRowGroup());

			//			// Тип направления
			//			TableRow typeRow = new TableRow();
			//			typeRow.Cells.Add(new TableCell(new Paragraph(new Run("Тип направления:")) { FontWeight = FontWeights.Bold }));
			//			typeRow.Cells.Add(new TableCell(new Paragraph(new Run(referral.type_of_direction))));
			//			directionTable.RowGroups[0].Rows.Add(typeRow);

			//			// Услуга
			//			TableRow serviceRow = new TableRow();
			//			serviceRow.Cells.Add(new TableCell(new Paragraph(new Run("Услуга:")) { FontWeight = FontWeights.Bold }));
			//			serviceRow.Cells.Add(new TableCell(new Paragraph(new Run(referral.service))));
			//			directionTable.RowGroups[0].Rows.Add(serviceRow);

			//			// Источник оплаты
			//			TableRow paymentRow = new TableRow();
			//			paymentRow.Cells.Add(new TableCell(new Paragraph(new Run("Источник оплаты:")) { FontWeight = FontWeights.Bold }));
			//			paymentRow.Cells.Add(new TableCell(new Paragraph(new Run(referral.payment_source))));
			//			directionTable.RowGroups[0].Rows.Add(paymentRow);

			//			// Организация
			//			TableRow orgRow = new TableRow();
			//			orgRow.Cells.Add(new TableCell(new Paragraph(new Run("Организация:")) { FontWeight = FontWeights.Bold }));
			//			orgRow.Cells.Add(new TableCell(new Paragraph(new Run(string.IsNullOrEmpty(referral.organization) ? "-" : referral.organization))));
			//			directionTable.RowGroups[0].Rows.Add(orgRow);

			//			// Врач
			//			TableRow doctorRow = new TableRow();
			//			doctorRow.Cells.Add(new TableCell(new Paragraph(new Run("Врач:")) { FontWeight = FontWeights.Bold }));
			//			doctorRow.Cells.Add(new TableCell(new Paragraph(new Run(referral.doctor))));
			//			directionTable.RowGroups[0].Rows.Add(doctorRow);

			//			// Дата приема
			//			TableRow dateRow = new TableRow();
			//			dateRow.Cells.Add(new TableCell(new Paragraph(new Run("Дата приема:")) { FontWeight = FontWeights.Bold }));
			//			dateRow.Cells.Add(new TableCell(new Paragraph(new Run(referral.date_of_admission?.ToString("dd.MM.yyyy")))));
			//			directionTable.RowGroups[0].Rows.Add(dateRow);

			//			// Диагноз
			//			TableRow diagnosisRow = new TableRow();
			//			diagnosisRow.Cells.Add(new TableCell(new Paragraph(new Run("Диагноз:")) { FontWeight = FontWeights.Bold }));
			//			diagnosisRow.Cells.Add(new TableCell(new Paragraph(new Run(txtTheMainDiagnosis.Text))));
			//			directionTable.RowGroups[0].Rows.Add(diagnosisRow);

			//			// Основание
			//			TableRow notesRow = new TableRow();
			//			notesRow.Cells.Add(new TableCell(new Paragraph(new Run("Основание:")) { FontWeight = FontWeights.Bold }));
			//			string justificationText = string.IsNullOrWhiteSpace(referral.justification) ? "-" : referral.justification;
			//			notesRow.Cells.Add(new TableCell(new Paragraph(new Run(justificationText))));

			//			doc.Blocks.Add(directionTable);

			//			// Добавляем подпись
			//			Paragraph signParagraph = new Paragraph();
			//			signParagraph.Inlines.Add(new LineBreak());
			//			signParagraph.Inlines.Add(new LineBreak());

			//			// Врач (левое выравнивание)
			//			signParagraph.Inlines.Add(new Run("Врач, оформивший направление: " + txtDoctorInfo.Text));
			//			signParagraph.Inlines.Add(new LineBreak());

			//			// Подпись и М.П. (центрирование через отдельный Span)
			//			Span centeredSpan = new Span();
			//			centeredSpan.Inlines.Add(new Run("Подпись: _______________________"));
			//			centeredSpan.Inlines.Add(new LineBreak());
			//			centeredSpan.Inlines.Add(new Run("М. П."));
			//			signParagraph.Inlines.Add(centeredSpan);

			//			// Устанавливаем выравнивание для всего параграфа
			//			signParagraph.TextAlignment = TextAlignment.Left;
			//			doc.Blocks.Add(signParagraph);

			//			// Создаем окно для просмотра документа
			//			Window viewerWindow = new Window
			//			{
			//				Title = $"Направление Н-{referral.id_referral}",
			//				Width = 850,
			//				Height = 550,
			//				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			//				Owner = this
			//			};

			//			FlowDocumentScrollViewer viewer = new FlowDocumentScrollViewer
			//			{
			//				Document = doc,
			//				VerticalScrollBarVisibility = ScrollBarVisibility.Auto
			//			};

			//			// Добавляем кнопки
			//			DockPanel dockPanel = new DockPanel();

			//			Button printButton = new Button
			//			{
			//				Content = "Печать",
			//				Width = 100,
			//				Margin = new Thickness(5),
			//				HorizontalAlignment = HorizontalAlignment.Right
			//			};
			//			printButton.Click += (s, args) =>
			//			{
			//				PrintDocument(doc);
			//				viewerWindow.Close();
			//			};

			//			DockPanel.SetDock(printButton, Dock.Bottom);
			//			dockPanel.Children.Add(printButton);
			//			dockPanel.Children.Add(viewer);

			//			viewerWindow.Content = dockPanel;
			//			viewerWindow.ShowDialog();
			//		}
			//	}
			//}
			//catch (Exception ex)
			//{
			//	MessageBox.Show($"Ошибка при загрузке направления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			//}
		}

		private void PrintButton_Click(object sender, RoutedEventArgs e)
		{
			ShowPrintPreview();
		}

		private void ShowPrintPreview()
		{
			try
			{
				// Загружаем шаблон
				FlowDocument doc = (FlowDocument)Application.LoadComponent(new Uri("/ЭМК;component/PrintTemplate.xaml", UriKind.Relative));

				// Заполняем данные (используем стандартную ширину для предпросмотра)
				FillPrintDocument(doc, 800);

				// Создаем окно предпросмотра
				Window previewWindow = new Window
				{
					Title = "Предпросмотр печати",
					Width = 850,
					Height = 700,
					WindowStartupLocation = WindowStartupLocation.CenterOwner,
					Owner = this
				};

				// Создаем просмотрщик документа
				FlowDocumentScrollViewer viewer = new FlowDocumentScrollViewer
				{
					Document = doc,
					VerticalScrollBarVisibility = ScrollBarVisibility.Auto
				};

				// Добавляем кнопки в окно
				DockPanel dockPanel = new DockPanel();
				Button printButton = new Button
				{
					Content = "Печать",
					Width = 100,
					Margin = new Thickness(5),
					HorizontalAlignment = HorizontalAlignment.Right
				};
				printButton.Click += (s, e) =>
				{
					PrintDocument(doc);
					previewWindow.Close();
				};

				DockPanel.SetDock(printButton, Dock.Bottom);
				dockPanel.Children.Add(printButton);
				dockPanel.Children.Add(viewer);

				previewWindow.Content = dockPanel;
				previewWindow.ShowDialog();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при создании предпросмотра: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void PrintDocument(FlowDocument doc)
		{
			PrintDialog printDialog = new PrintDialog();
			if (printDialog.ShowDialog() == true)
			{
				// Устанавливаем размер страницы
				doc.PageWidth = printDialog.PrintableAreaWidth;
				doc.PageHeight = printDialog.PrintableAreaHeight;
				doc.ColumnWidth = printDialog.PrintableAreaWidth;

				printDialog.PrintDocument(
					((IDocumentPaginatorSource)doc).DocumentPaginator, "Медицинская карта");
			}
		}

		private double CalculateBMI()
		{
			// Пытаемся преобразовать рост и вес в числа
			if (double.TryParse(txtHeight.Text, out double height) &&
				double.TryParse(txtWeight.Text, out double weight))
			{
				// Проверяем, что рост не равен нулю (чтобы избежать деления на ноль)
				if (height > 0)
				{
					// Рассчитываем ИМТ: вес (кг) / (рост (м))^2
					return weight / Math.Pow(height / 100, 2);
				}
			}
			return 0; // Возвращаем 0 в случае ошибки
		}

		private void FillPrintDocument(FlowDocument doc, double pageWidth)
		{
			// Основная информация
			((Run)doc.FindName("inspectionDateRun")).Text = $"{dateTextBlock.Text}  {timePicker.Text}";

			// Данные пациента
			((Paragraph)doc.FindName("patientNameParagraph")).Inlines.Clear();
			((Paragraph)doc.FindName("patientNameParagraph")).Inlines.Add(new Run(txtPatientName.Text));

			((Paragraph)doc.FindName("birthDateParagraph")).Inlines.Clear();
			((Paragraph)doc.FindName("birthDateParagraph")).Inlines.Add(new Run(txtPatientBirthDate.Text));

			((Paragraph)doc.FindName("ageParagraph")).Inlines.Clear();
			((Paragraph)doc.FindName("ageParagraph")).Inlines.Add(new Run(txtPatientAge.Text));

			((Paragraph)doc.FindName("genderParagraph")).Inlines.Clear();
			((Paragraph)doc.FindName("genderParagraph")).Inlines.Add(new Run("Женский")); // Здесь нужно получить пол из данных

			// Медицинская информация
			((Run)doc.FindName("diagnosisRun")).Text = txtTheMainDiagnosis.Text;
			((Run)doc.FindName("complaintsRun")).Text = ComplaintsTextBox.Text;
			((Run)doc.FindName("anamnesisRun")).Text = "Перенесенные заболевания: ..."; // Заполните из данных

			// Физические параметры
			((Run)doc.FindName("heightRun")).Text = txtHeight.Text;
			((Run)doc.FindName("weightRun")).Text = txtWeight.Text;
			((Run)doc.FindName("bmiRun")).Text = CalculateBMI().ToString("F2");
			((Run)doc.FindName("bloodPressureUpperRun")).Text = txtBloodPressureUpper.Text;
			((Run)doc.FindName("bloodPressureLowerRun")).Text = txtBloodPressureLower.Text;
			((Run)doc.FindName("temperatureRun")).Text = txtTemperature.Text;
			((Run)doc.FindName("heartRateRun")).Text = txtHeartRate.Text;
			((Run)doc.FindName("respiratoryRateRun")).Text = txtRespiratoryRate.Text;

			((Run)doc.FindName("diagnosisDescriptionRun")).Text = txtTheMainDiagnosis.Text;
			((Run)doc.FindName("treatmentRun")).Text = txtTreatment.Text;

			// Информация о приеме
			((Run)doc.FindName("serviceTypeRun")).Text = txtCaseTypeService.Text;
			((Run)doc.FindName("servicePurposeRun")).Text = txtPurposeOfTheService.Text;
			((Run)doc.FindName("servicePlaceRun")).Text = txtPlaceOfService.Text;

			// Врач
			((Run)doc.FindName("doctorNameRun")).Text = txtDoctorInfo.Text;
		}

		private void CloseBt_Click(object sender, RoutedEventArgs e)
		{
			this.Hide();
		}
	}
}
