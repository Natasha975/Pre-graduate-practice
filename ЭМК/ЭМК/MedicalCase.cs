using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ЭМК.Model;

namespace ЭМК
{
	public class MedicalCase
	{
		//Данные пациента
		public int IdPatient { get; set; }
		public string PatientName { get; set; }
		public string PatientBirthDate { get; set; }
		public string PatientEnp { get; set; }
		public string PatientSnils { get; set; }
		public string PatientAge { get; set; }

		// Данные осмотра
		public int IdInspection { get; set; }
		public string Date { get; set; }
		public string Time { get; set; }
		public int IdDoctor { get; set; }
		public string DoctorName { get; set; }

		public string PlaceOfService { get; set; }
		public string TypeOfServiceCase { get; set; }
		public string TypeOfPayment { get; set; }
		public string PurposeOfTheService { get; set; }

		public Nullable<int> Height { get; set; }
		public Nullable<int> Weight { get; set; }
		public Nullable<int> BloodPressureUpper { get; set; }
		public Nullable<int> BloodPressureLower { get; set; }
		public Nullable<double> Temperature { get; set; }
		public Nullable<int> HeartRate { get; set; }
		public Nullable<int> RespiratoryRate { get; set; }
		public Nullable<int> OxygenSaturation { get; set; }

		public string PatientCondition { get; set; }
		public string PreliminaryDiagnosis { get; set; }
		public string TheMainDiagnosis { get; set; }

		public string Diagnosis { get; set; }
		public string Complaints { get; set; }
		public string Anamnesis { get; set; }
		public string SuspicionOfHeat { get; set; }
		public string Treatment { get; set; }
		public string Recommendations { get; set; }

		public List<Direction> Directions { get; set; } = new List<Direction>();
	}
}
