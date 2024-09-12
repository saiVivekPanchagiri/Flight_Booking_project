using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto
{
    public class AirlineDto
    {
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }
        public int BaggageAllowance { get; set; }
        public List<string> Flights { get; set; }
        public List<string> Airports { get; set; }
    }

}
