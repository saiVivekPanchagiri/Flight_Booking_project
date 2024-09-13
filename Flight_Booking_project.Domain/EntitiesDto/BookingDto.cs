using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        public bool IsPaid { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        public List<PassengerDto> Passengers { get; set; }
        public int FlightId { get; set; }
        public FlightDto Flight { get; set; }
    }
}
