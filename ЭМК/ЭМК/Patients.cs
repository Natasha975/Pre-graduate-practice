using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЭМК
{
	public class Patients
	{
		public int Id { get; set; }
		public string FullName => $"{lastname} {name} {surname ?? ""}";
		public string BirthDate => birthday.ToString("dd.MM.yyyy");
		public string Age => age.ToString();
		public string Pol => gender == "Мужской" ? "Мужской" : "Женский";
		public string Snils { get; set; }
		public string Enp => number_policy_OMS;
		public string Phone { get; set; }
		public string Policy => number_policy_OMS;
		public string passport_number_and_series { get; set; }
		public string name_insurance_company { get; set; }

		public string place_of_work { get; set; }

		// Свойства, соответствующие полям таблицы patient
		public string lastname { get; set; }
		public string name { get; set; }
		public string surname { get; set; }
		public DateTime birthday { get; set; }
		public string MiddleName { get; set; }
		public DateTime BirthDateValue { get; set; }
		public int age { get; set; }
		public string gender { get; set; }
		public string number_policy_OMS { get; set; }

		public string Address { get; set; }
		public string HospitalName { get; set; }

		//private int CalculateAge(DateTime birthDate)
		//{
		//	var today = DateTime.Today;
		//	var age = today.Year - birthDate.Year;
		//	if (birthDate.Date > today.AddYears(-age)) age--;
		//	return age;
		//}

	}
}
