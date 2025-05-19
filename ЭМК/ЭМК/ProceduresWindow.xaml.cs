using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ЭМК
{
	/// <summary>
	/// Логика взаимодействия для ProceduresWindow.xaml
	/// </summary>
	public partial class ProceduresWindow : Window
	{
		public Procedure SelectedProcedure { get; private set; }
		private List<Procedure> _allProcedures;
		public ProceduresWindow()
		{
			InitializeComponent();
			LoadProcedures();
		}
		private void LoadProcedures()
		{
			try
			{
				string filePath = "procedures.txt";

				if (!File.Exists(filePath))
				{
					MessageBox.Show("Файл procedures.txt не найден!");
					Close();
					return;
				}

				_allProcedures = new List<Procedure>();

				foreach (string line in File.ReadLines(filePath))
				{
					if (string.IsNullOrWhiteSpace(line))
						continue;

					int firstSpaceIndex = line.IndexOf(' ');

					if (firstSpaceIndex > 0)
					{
						string code = line.Substring(0, firstSpaceIndex).Trim();
						string description = line.Substring(firstSpaceIndex + 1).Trim();

						_allProcedures.Add(new Procedure
						{
							Code = code,
							Name = description
						});
					}
				}

				ProceduresListView.ItemsSource = _allProcedures;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
				Close();
			}
		}

		private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var searchText = SearchTextBox.Text.ToLower();

			if (string.IsNullOrWhiteSpace(searchText))
			{
				ProceduresListView.ItemsSource = _allProcedures;
			}
			else
			{
				ProceduresListView.ItemsSource = _allProcedures
					.Where(p => p.Code.ToLower().Contains(searchText) ||
						   p.Name.ToLower().Contains(searchText))
					.ToList();
			}
		}

		private void SelectButton_Click(object sender, RoutedEventArgs e)
		{
			SelectedProcedure = ProceduresListView.SelectedItem as Procedure;

			if (SelectedProcedure != null)
			{
				DialogResult = true;
				Close();
			}
			else
			{
				MessageBox.Show("Выберите процедуру из списка");
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
	public class Procedure
	{
		public string Code { get; set; }
		public string Name { get; set; }

		public string DisplayText => $"{Code} - {Name}";

		public override string ToString() => DisplayText;
	}
}
