using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
	public class Flight
	{
		public int FlightId { get; set; }

		[Required]
		public string AircraftType { get; set; }

		[Required]
		public string FromLocation { get; set; }

		[Required]
		public string ToLocation { get; set; }

		[Required]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd/MM/yyyy tt/mm")]
		public DateTime DepartureTime { get; set; }

		[Required]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd/MM/yyyy tt/mm")]
		[Range(typeof(DateTime), "01/01/1900 00:00", "06/06/2079 00:00")]
		public DateTime ArrivalTime { get; set; }
	}
}
