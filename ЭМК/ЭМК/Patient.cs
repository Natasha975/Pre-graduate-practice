using System;

namespace ЭМК
{
	public class Patient
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


		// Свойства, соответствующие полям таблицы patient
		public string lastname { get; set; }
		public string name { get; set; }
		public string surname { get; set; }
		public DateTime birthday { get; set; }
		public int age { get; set; }
		public string gender { get; set; }
		public string number_policy_OMS { get; set; }

		public string Address { get; set; }
		public string HospitalName { get; set; }
		
		// ID больницы
		public int? HospitalId { get; set; } 


		public DateTime? BirthDateForSearch => birthday;
		public string SnilsForSearch => Snils;
		public string PolicyForSearch => Policy;
		public string PhoneForSearch => Phone;
	}
}
