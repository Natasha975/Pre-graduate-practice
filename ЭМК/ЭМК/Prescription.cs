using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЭМК
{
	public class Prescription
	{
		public int number { get; set; }
		public string type { get; set; } // Тип рецепта (107-1/у, 148-1/у-88 и т.д.)
		public int id_inspection { get; set; }
		public System.DateTime date { get; set; }
		public string name_of_the_drug { get; set; }
		public string dosage { get; set; }
		public string method_of_administration { get; set; }
		public string method_of_administration_details { get; set; }
		public string dosage_regimen { get; set; }
		public string dosage_regimen_optional { get; set; }
		public string duration_of_treatment_number { get; set; }
		public string duration_of_treatment_duration { get; set; }
		public string duration_of_treatment_comments { get; set; }
		public string justification_of_appointment { get; set; }
	}
}
