using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЭМК.Model
{
	// Класс для хранения обоих диагнозов
	public class SelectedDiagnoses
	{
		public MkbDiagnosis PreliminaryDiagnosis { get; set; }
		public MkbDiagnosis MainDiagnosis { get; set; }
	}
}
