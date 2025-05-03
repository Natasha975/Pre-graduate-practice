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

				txtPatientCondition.Text = medicalCase.PatientCondition;
				txtPreliminaryDiagnosis.Text = medicalCase.PreliminaryDiagnosis;
				txtTheMainDiagnosis.Text = medicalCase.TheMainDiagnosis;

				// Жалобы и рекомендации
				tbMedicalHistory.Text =medicalCase.Anamnesis;
				txtSuspicionOfHeat.Text = medicalCase.SuspicionOfHeat;
				ComplaintsTextBox.Text = medicalCase.Complaints ?? "";
				txtTreatment.Text = medicalCase.Treatment ?? "";
				txtRecommendations.Text = medicalCase.Recommendations ?? "";
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
							  MessageBoxButton.OK, MessageBoxImage.Error);
			}
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
				FlowDocument doc = (FlowDocument)Application.LoadComponent(
					new Uri("/ЭМК;component/PrintTemplate.xaml", UriKind.Relative));

				// Заполняем данные (используем стандартную ширину для предпросмотра)
				FillPrintDocument(doc, 800);

				// Создаем окно предпросмотра
				Window previewWindow = new Window
				{
					Title = "Предпросмотр печати",
					Width = 850,
					Height = 1100,
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
