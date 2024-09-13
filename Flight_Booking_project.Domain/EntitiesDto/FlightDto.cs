using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto
{
    public class FlightDto
    {
        public int FlightId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int DepartureAirportId { get; set; }
        public AirportDto DepartureAirport { get; set; }
        public int ArrivalAirportId { get; set; }
        public AirportDto ArrivalAirport { get; set; }
        public int AirlineId { get; set; }
        public AirlineDto Airline { get; set; }
        public List<StopDto> Stops { get; set; }
        public List<SeatDto> Seats { get; set; }
        public List<string> Bookings { get; set; }
    }

}
