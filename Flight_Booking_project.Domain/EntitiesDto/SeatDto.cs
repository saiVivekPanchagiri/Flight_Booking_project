using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto
{
    public class SeatDto
    {
        public int SeatId { get; set; }
        public string SeatNumber { get; set; }
        public string ClassType { get; set; }
        public bool IsAvailable { get; set; }
        public string Position { get; set; }
        public decimal Price { get; set; }
        public int FlightId { get; set; }
        public string Flight { get; set; }
    }

}
