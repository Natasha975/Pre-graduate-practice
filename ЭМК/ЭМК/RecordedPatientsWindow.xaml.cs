using System;
using System.Collections.Generic;
using System.Data.Entity;
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
	/// Логика взаимодействия для RecordedPatientsWindow.xaml
	/// </summary>
	public partial class RecordedPatientsWindow : Window
	{
		private MedCardDBEntities dbContext;

		public RecordedPatientsWindow()
		{
			InitializeComponent();
			DataContext = this;
			dbContext = new MedCardDBEntities();

			LoadDoctorInfo();
			LoadTodayAppointments();
		}

		private async void LoadDoctorInfo()
		{
			if (App.CurrentDoctor != null)
			{
				txtDoctorInfo.Text = $"Добро пожаловать, {App.CurrentDoctor.lastname} {App.CurrentDoctor.name}";

				// Если есть отчество, можно добавить и его
				if (!string.IsNullOrEmpty(App.CurrentDoctor.surname))
				{
					txtDoctorInfo.Text += $" {App.CurrentDoctor.surname}";
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
						txtDoctorInfo.Text += $" ({specialization.name})";
					}
				}
			}
		}

		private async void LoadTodayAppointments()
		{
			try
			{
				var today = DateTime.Today;

				var appointments = await dbContext.appointment
					.Where(a => DbFunctions.TruncateTime(a.date) == today && a.id_doctor == App.CurrentDoctor.id_doctor)
					.Join(dbContext.patient,
						a => a.id_patient,
						p => p.id_patient,
						(a, p) => new TodayAppointment
						{
							Time = a.time,
							AppointmentType = a.type_of_appointment,
							PatientFullName = $"{p.lastname} {p.name} {p.surname}",
							PatientBirthDate = p.birthday.HasValue ? p.birthday.Value.ToString("dd.MM.yyyy") : "",
							PatientSnils = p.snils,
							PatientId = p.id_patient
						})
					.OrderBy(a => a.Time)
					.ToListAsync();

				AppointmentsDataGrid.ItemsSource = appointments;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке записей: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void Next_Click(object sender, RoutedEventArgs e)
		{
			Window1 window1 = new Window1();
			window1.Show();
			this.Close();
		}

		private void ExitBt_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
			this.Close();
        }
    }

	public class TodayAppointment
	{
		public DateTime Time { get; set; }
		public string AppointmentType { get; set; }
		public string PatientFullName { get; set; }
		public string PatientBirthDate { get; set; }
		public string PatientSnils { get; set; }
		public int PatientId { get; set; }
	}
}
