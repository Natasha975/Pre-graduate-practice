using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЭМК
{
	public class Direction
	{
		public int Id { get; set; }
		public string DirectionNumber { get; set; }
		public string DirectionType { get; set; }
		public string Service { get; set; }
		public DateTime AppointmentDate { get; set; }
		public string Organization { get; set; }
		public string Doctor { get; set; }
	}
}
