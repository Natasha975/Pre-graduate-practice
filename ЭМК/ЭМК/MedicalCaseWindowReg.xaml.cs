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

		private void CloseBt_Click(object sender, RoutedEventArgs e)
		{
			this.Hide();
		}
	}
}
