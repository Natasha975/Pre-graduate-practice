using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace ЭМК.Model
{
	public static class DirectionViewerHelper
	{
		public static void ShowDirectionDetails(int directionId, string patientName, string patientBirthDate,
				string patientSnils, string doctorInfo, string mainDiagnosis, Window ownerWindow)
		{
			try
			{
				using (var db = new MedCardDBEntities())
				{
					var referral = db.referral.FirstOrDefault(r => r.id_referral == directionId);
					if (referral != null)
					{
						// Создаем FlowDocument с макетом направления
						FlowDocument doc = CreateDirectionDocument(referral, patientName, patientBirthDate,
							patientSnils, doctorInfo, mainDiagnosis);

						// Создаем окно для просмотра документа
						var viewerWindow = new Window
						{
							Title = $"Направление Н-{referral.id_referral}",
							Width = 850,
							Height = 550,
							WindowStartupLocation = WindowStartupLocation.CenterOwner,
							Owner = ownerWindow
						};

						var viewer = new FlowDocumentScrollViewer
						{
							Document = doc,
							VerticalScrollBarVisibility = ScrollBarVisibility.Auto
						};

						// Добавляем кнопки
						var dockPanel = new DockPanel();

						var printButton = new Button
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
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке направления: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private static FlowDocument CreateDirectionDocument(referral referral, string patientName,
			string patientBirthDate, string patientSnils, string doctorInfo, string mainDiagnosis)
		{
			var doc = new FlowDocument
			{
				PageWidth = 800,
				PagePadding = new Thickness(40),
				ColumnWidth = 700,
				FontFamily = new FontFamily("Arial"),
				FontSize = 12
			};

			// Добавляем заголовок
			var header = new Paragraph(new Run("НАПРАВЛЕНИЕ"))
			{
				FontSize = 16,
				FontWeight = FontWeights.Bold,
				TextAlignment = TextAlignment.Center,
				Margin = new Thickness(0, 0, 0, 20)
			};
			doc.Blocks.Add(header);

			// Добавляем номер и дату направления
			var numberParagraph = new Paragraph();
			numberParagraph.Inlines.Add(new Run($"№ Н-{referral.id_referral}"));
			numberParagraph.Inlines.Add(new Run($" от {referral.date_of_creation?.ToString("dd.MM.yyyy")}"));
			doc.Blocks.Add(numberParagraph);

			// Добавляем разделитель
			doc.Blocks.Add(new Paragraph(new Run(new string('_', 100)))
			{
				Margin = new Thickness(0, 10, 0, 20)
			});

			// Добавляем информацию о пациенте
			var patientTable = new Table
			{
				CellSpacing = 0,
				Margin = new Thickness(0, 0, 0, 20)
			};

			// Создаем колонки
			patientTable.Columns.Add(new TableColumn { Width = new GridLength(150) });
			patientTable.Columns.Add(new TableColumn { Width = new GridLength(500) });

			// Создаем строки
			patientTable.RowGroups.Add(new TableRowGroup());

			// Пациент
			var patientRow = new TableRow();
			patientRow.Cells.Add(new TableCell(new Paragraph(new Run("Пациент:")) { FontWeight = FontWeights.Bold }));
			patientRow.Cells.Add(new TableCell(new Paragraph(new Run($"{patientName} ({patientBirthDate})"))));
			patientTable.RowGroups[0].Rows.Add(patientRow);

			// СНИЛС
			var snilsRow = new TableRow();
			snilsRow.Cells.Add(new TableCell(new Paragraph(new Run("СНИЛС:")) { FontWeight = FontWeights.Bold }));
			snilsRow.Cells.Add(new TableCell(new Paragraph(new Run(patientSnils))));
			patientTable.RowGroups[0].Rows.Add(snilsRow);

			doc.Blocks.Add(patientTable);

			// Добавляем информацию о направлении
			var directionTable = new Table
			{
				CellSpacing = 0,
				Margin = new Thickness(0, 0, 0, 20)
			};

			// Создаем колонки
			directionTable.Columns.Add(new TableColumn { Width = new GridLength(150) });
			directionTable.Columns.Add(new TableColumn { Width = new GridLength(500) });

			// Создаем строки
			directionTable.RowGroups.Add(new TableRowGroup());

			// Тип направления
			var typeRow = new TableRow();
			typeRow.Cells.Add(new TableCell(new Paragraph(new Run("Тип направления:")) { FontWeight = FontWeights.Bold }));
			typeRow.Cells.Add(new TableCell(new Paragraph(new Run(referral.type_of_direction))));
			directionTable.RowGroups[0].Rows.Add(typeRow);

			// Услуга
			var serviceRow = new TableRow();
			serviceRow.Cells.Add(new TableCell(new Paragraph(new Run("Услуга:")) { FontWeight = FontWeights.Bold }));
			serviceRow.Cells.Add(new TableCell(new Paragraph(new Run(referral.service))));
			directionTable.RowGroups[0].Rows.Add(serviceRow);

			// Источник оплаты
			var paymentRow = new TableRow();
			paymentRow.Cells.Add(new TableCell(new Paragraph(new Run("Источник оплаты:")) { FontWeight = FontWeights.Bold }));
			paymentRow.Cells.Add(new TableCell(new Paragraph(new Run(referral.payment_source))));
			directionTable.RowGroups[0].Rows.Add(paymentRow);

			// Организация
			var orgRow = new TableRow();
			orgRow.Cells.Add(new TableCell(new Paragraph(new Run("Организация:")) { FontWeight = FontWeights.Bold }));
			orgRow.Cells.Add(new TableCell(new Paragraph(new Run(string.IsNullOrEmpty(referral.organization) ? "-" : referral.organization))));
			directionTable.RowGroups[0].Rows.Add(orgRow);

			// Врач
			var doctorRow = new TableRow();
			doctorRow.Cells.Add(new TableCell(new Paragraph(new Run("Врач:")) { FontWeight = FontWeights.Bold }));
			doctorRow.Cells.Add(new TableCell(new Paragraph(new Run(referral.doctor))));
			directionTable.RowGroups[0].Rows.Add(doctorRow);

			// Дата приема
			var dateRow = new TableRow();
			dateRow.Cells.Add(new TableCell(new Paragraph(new Run("Дата приема:")) { FontWeight = FontWeights.Bold }));
			dateRow.Cells.Add(new TableCell(new Paragraph(new Run(referral.date_of_admission?.ToString("dd.MM.yyyy")))));
			directionTable.RowGroups[0].Rows.Add(dateRow);

			// Диагноз
			var diagnosisRow = new TableRow();
			diagnosisRow.Cells.Add(new TableCell(new Paragraph(new Run("Диагноз:")) { FontWeight = FontWeights.Bold }));
			diagnosisRow.Cells.Add(new TableCell(new Paragraph(new Run(mainDiagnosis))));
			directionTable.RowGroups[0].Rows.Add(diagnosisRow);

			// Основание
			var notesRow = new TableRow();
			notesRow.Cells.Add(new TableCell(new Paragraph(new Run("Основание:")) { FontWeight = FontWeights.Bold }));
			string justificationText = string.IsNullOrWhiteSpace(referral.justification) ? "-" : referral.justification;
			notesRow.Cells.Add(new TableCell(new Paragraph(new Run(justificationText))));

			doc.Blocks.Add(directionTable);

			// Добавляем подпись
			var signParagraph = new Paragraph();
			signParagraph.Inlines.Add(new LineBreak());
			signParagraph.Inlines.Add(new LineBreak());

			// Врач (левое выравнивание)
			signParagraph.Inlines.Add(new Run("Врач, оформивший направление: " + doctorInfo));
			signParagraph.Inlines.Add(new LineBreak());

			// Подпись и М.П. (центрирование через отдельный Span)
			var centeredSpan = new Span();
			centeredSpan.Inlines.Add(new Run("Подпись: _______________________"));
			centeredSpan.Inlines.Add(new LineBreak());
			centeredSpan.Inlines.Add(new Run("М. П."));
			signParagraph.Inlines.Add(centeredSpan);

			// Устанавливаем выравнивание для всего параграфа
			signParagraph.TextAlignment = TextAlignment.Left;
			doc.Blocks.Add(signParagraph);

			return doc;
		}

		private static void PrintDocument(FlowDocument document)
		{
			var printDialog = new PrintDialog();
			if (printDialog.ShowDialog() == true)
			{
				document.PageHeight = printDialog.PrintableAreaHeight;
				document.PageWidth = printDialog.PrintableAreaWidth;
				printDialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, "Направление");
			}
		}
	}
}
