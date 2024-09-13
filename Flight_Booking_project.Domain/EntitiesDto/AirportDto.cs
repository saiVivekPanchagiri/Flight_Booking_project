using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto
{
    public class AirportDto
    {
        public int AirportId { get; set; }
        public string AirportCode { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<string> DepartureFlights { get; set; }
        public List<string> ArrivalFlights { get; set; }
        public List<AirlineDto> Airlines { get; set; }
    }

}
