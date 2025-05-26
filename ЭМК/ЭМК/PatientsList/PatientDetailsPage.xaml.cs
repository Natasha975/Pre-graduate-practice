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

namespace ЭМК.PatientsList
{
	/// <summary>
	/// Логика взаимодействия для PatientDetailsPage.xaml
	/// </summary>
	public partial class PatientDetailsPage : Page
	{
		private readonly MedCardDBEntities _db;
		
		private patient currentPatient;
		private readonly bool _isNewPatient;

		public PatientDetailsPage(MedCardDBEntities db, patient patient, bool isNewPatient = false)
		{
			InitializeComponent();

			_db = db;
			//DataContext = currentPatient;
			currentPatient = patient;
			_isNewPatient = isNewPatient;

			// Изменяем заголовок для нового пациента
			if (_isNewPatient)
			{
				TitleText.Text = "Добавление нового пациента";
			}

			LoadPatientData();
			//InitializeButtons();
			LoadData();
			//LoadPatientData();
		}

		public void LoadData()
		{
			// Заполнение ComboBox субъектами РФ
			RegionComboBox.ItemsSource = new List<Region>
			{
				new Region { Id = 1, Name = "Республика Адыгея" },
				new Region { Id = 2, Name = "Республика Алтай" },
				new Region { Id = 3, Name = "Республика Башкортостан" },
				new Region { Id = 4, Name = "Республика Бурятия" },
				new Region { Id = 5, Name = "Республика Дагестан" },
				new Region { Id = 6, Name = "Республика Ингушетия" },
				new Region { Id = 7, Name = "Кабардино-Балкарская Республика" },
				new Region { Id = 8, Name = "Республика Калмыкия" },
				new Region { Id = 9, Name = "Карачаево-Черкесская Республика" },
				new Region { Id = 10, Name = "Республика Карелия" },
				new Region { Id = 11, Name = "Республика Коми" },
				new Region { Id = 12, Name = "Республика Крым" },
				new Region { Id = 13, Name = "Республика Марий Эл" },
				new Region { Id = 14, Name = "Республика Мордовия" },
				new Region { Id = 15, Name = "Республика Саха (Якутия)" },
				new Region { Id = 16, Name = "Республика Северная Осетия - Алания" },
				new Region { Id = 17, Name = "Республика Татарстан" },
				new Region { Id = 18, Name = "Республика Тыва" },
				new Region { Id = 19, Name = "Удмуртская Республика" },
				new Region { Id = 20, Name = "Республика Хакасия" },
				new Region { Id = 21, Name = "Чеченская Республика" },
				new Region { Id = 22, Name = "Чувашская Республика" },
				new Region { Id = 23, Name = "Алтайский край" },
				new Region { Id = 24, Name = "Забайкальский край" },
				new Region { Id = 25, Name = "Камчатский край" },
				new Region { Id = 26, Name = "Краснодарский край" },
				new Region { Id = 27, Name = "Красноярский край" },
				new Region { Id = 28, Name = "Пермский край" },
				new Region { Id = 29, Name = "Приморский край" },
				new Region { Id = 30, Name = "Ставропольский край" },
				new Region { Id = 31, Name = "Хабаровский край" },
				new Region { Id = 32, Name = "Амурская область" },
				new Region { Id = 33, Name = "Архангельская область" },
				new Region { Id = 34, Name = "Астраханская область" },
				new Region { Id = 35, Name = "Белгородская область" },
				new Region { Id = 36, Name = "Брянская область" },
				new Region { Id = 37, Name = "Владимирская область" },
				new Region { Id = 38, Name = "Волгоградская область" },
				new Region { Id = 39, Name = "Вологодская область" },
				new Region { Id = 40, Name = "Воронежская область" },
				new Region { Id = 41, Name = "Ивановская область" },
				new Region { Id = 42, Name = "Иркутская область" },
				new Region { Id = 43, Name = "Калининградская область" },
				new Region { Id = 44, Name = "Калужская область" },
				new Region { Id = 45, Name = "Кемеровская область" },
				new Region { Id = 46, Name = "Кировская область" },
				new Region { Id = 47, Name = "Костромская область" },
				new Region { Id = 48, Name = "Курганская область" },
				new Region { Id = 49, Name = "Курская область" },
				new Region { Id = 50, Name = "Ленинградская область" },
				new Region { Id = 51, Name = "Липецкая область" },
				new Region { Id = 52, Name = "Магаданская область" },
				new Region { Id = 53, Name = "Московская область" },
				new Region { Id = 54, Name = "Мурманская область" },
				new Region { Id = 55, Name = "Нижегородская область" },
				new Region { Id = 56, Name = "Новгородская область" },
				new Region { Id = 57, Name = "Новосибирская область" },
				new Region { Id = 58, Name = "Омская область" },
				new Region { Id = 59, Name = "Оренбургская область" },
				new Region { Id = 60, Name = "Орловская область" },
				new Region { Id = 61, Name = "Пензенская область" },
				new Region { Id = 62, Name = "Псковская область" },
				new Region { Id = 63, Name = "Ростовская область" },
				new Region { Id = 64, Name = "Рязанская область" },
				new Region { Id = 65, Name = "Самарская область" },
				new Region { Id = 66, Name = "Саратовская область" },
				new Region { Id = 67, Name = "Сахалинская область" },
				new Region { Id = 68, Name = "Свердловская область" },
				new Region { Id = 69, Name = "Смоленская область" },
				new Region { Id = 70, Name = "Тамбовская область" },
				new Region { Id = 71, Name = "Тверская область" },
				new Region { Id = 72, Name = "Томская область" },
				new Region { Id = 73, Name = "Тульская область" },
				new Region { Id = 74, Name = "Тюменская область" },
				new Region { Id = 75, Name = "Ульяновская область" },
				new Region { Id = 76, Name = "Челябинская область" },
				new Region { Id = 77, Name = "Ярославская область" },
				new Region { Id = 78, Name = "Москва" },
				new Region { Id = 79, Name = "Санкт-Петербург" },
				new Region { Id = 80, Name = "Севастополь" },
				new Region { Id = 81, Name = "Еврейская автономная область" },
				new Region { Id = 82, Name = "Ненецкий автономный округ" },
				new Region { Id = 83, Name = "Ханты-Мансийский автономный округ - Югра" },
				new Region { Id = 84, Name = "Чукотский автономный округ" },
				new Region { Id = 85, Name = "Ямало-Ненецкий автономный округ" },
				new Region { Id = 86, Name = "Донецкая Народная Республика" },
				new Region { Id = 87, Name = "Запорожская область" },
				new Region { Id = 88, Name = "Луганская Народная Республика" },
				new Region { Id = 89, Name = "Херсонская область" }
			};
		}

		//private void InitializeButtons()
		//{
		//	// Создаем кнопки в зависимости от режима
		//	var buttonsPanel = new StackPanel
		//	{
		//		Orientation = Orientation.Horizontal,
		//		HorizontalAlignment = HorizontalAlignment.Right,
		//		Margin = new Thickness(0, 10, 0, 0)
		//	};

		//	var saveButton = new Button
		//	{
		//		Content = _isNewPatient ? "Добавить" : "Сохранить",
		//		Width = 120,
		//		Margin = new Thickness(5),
		//		Style = (Style)FindResource("MaterialDesignRaisedButton")
		//	};
		//	saveButton.Click += SaveButton_Click;

		//	var cancelButton = new Button
		//	{
		//		Content = "Отмена",
		//		Width = 120,
		//		Margin = new Thickness(5),
		//		Style = (Style)FindResource("MaterialDesignRaisedButton")
		//	};
		//	cancelButton.Click += CancelButton_Click;

		//	buttonsPanel.Children.Add(saveButton);
		//	buttonsPanel.Children.Add(cancelButton);

		//	// Добавляем панель кнопок в Grid
		//	var grid = (Grid)Content;
		//	grid.Children.Add(buttonsPanel);
		//	Grid.SetRow(buttonsPanel, 1);
		//}

		private void LoadPatientData()
		{
			try
			{
				DataContext = currentPatient;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка загрузки данных пациента: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
				NavigationService.GoBack();
			}
		}
	

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (_isNewPatient)
				{
					// Добавляем нового пациента
					_db.patient.Add(currentPatient);
				}

				_db.SaveChanges();
				MessageBox.Show("Данные пациента успешно сохранены", "Успех",
					MessageBoxButton.OK, MessageBoxImage.Information);
				NavigationService.GoBack();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка сохранения данных: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.GoBack();
		}
		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!char.IsDigit(e.Text, 0))
				e.Handled = true;
		}
	}
}
