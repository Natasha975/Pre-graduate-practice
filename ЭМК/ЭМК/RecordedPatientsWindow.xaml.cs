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

				// Устанавливаем текст в Label
			}
		}

		private void Next_Click(object sender, RoutedEventArgs e)
		{
			Window1 window1 = new Window1();
			window1.Show();
			this.Close();
		}
	}
}
