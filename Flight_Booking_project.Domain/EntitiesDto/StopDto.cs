using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto
{
    public class StopDto
    {
        public int StopId { get; set; }
        public DateTime StopTime { get; set; }
        public int FlightId { get; set; }
        public string Flight { get; set; }
        public int AirportId { get; set; }
        public AirportDto Airport { get; set; }
    }

}
