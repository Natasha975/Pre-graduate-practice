using System.Linq;
using System.Windows;
using System.Windows.Input;
using ЭМК.Model;
using System.Data.Entity;

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		private MedCardDBEntities dbContext;

		public LoginWindow()
		{
			InitializeComponent();
			dbContext = new MedCardDBEntities();

			// Установка фокуса на поле логина при загрузке
			Loaded += (s, e) => txtUsername.Focus();

			// Обработка нажатия Enter для авторизации
			txtPassword.KeyDown += (s, e) =>
			{
				if (e.Key == Key.Enter) LoginButton_Click(null, null);
			};
		}

		private async void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			string username = txtUsername.Text.Trim();
			string password = txtPassword.Password;

			// Проверка на пустые поля
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			try
			{
				// Проверяем вход для доктора
				var doctor = await dbContext.doctor
					.Where(u => u.login == username && u.password == password)
					.FirstOrDefaultAsync();

				// Фиксированные данные для сотрудника регистратуры
				const string receptionLogin = "reception";
				const string receptionPassword = "reception123";

				if (doctor != null)
				{
					// Сохраняем информацию о текущем пользователе (докторе)
					App.CurrentDoctor = doctor;
					App.CurrentUserType = "Doctor";

					// Открываем главное окно
					MainWindow mainWindow = new MainWindow();
					mainWindow.Show();
					this.Close();
				}
				else if (username == receptionLogin && password == receptionPassword)
				{
					// Создаем запись для сотрудника регистратуры
					App.CurrentUserType = "Reception";

					// Открываем главное окно
					PatientsListWindow patientsListWindow = new PatientsListWindow(isReception: true);
					patientsListWindow.Show();
					this.Close();
				}
				else
				{
					MessageBox.Show("Неверный логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
					txtPassword.Password = "";
					txtPassword.Focus();
				}
				//var user = await dbContext.doctor.Where(u => u.login == username && u.password == password).FirstOrDefaultAsync();

				//if (user != null)
				//{
				//	// Сохраняем информацию о текущем пользователе
				//	App.CurrentDoctor = user;

				//	// Открываем главное окно
				//	MainWindow mainWindow = new MainWindow();
				//	mainWindow.Show();
				//	this.Close();
				//}
				//else
				//{
				//	MessageBox.Show("Неверный логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
				//	txtPassword.Password = "";
				//	txtPassword.Focus();
				//}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show($"Ошибка при подключении к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}