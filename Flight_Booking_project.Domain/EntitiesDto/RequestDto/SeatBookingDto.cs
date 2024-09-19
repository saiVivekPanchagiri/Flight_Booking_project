using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.RequestDto
{
    public class SeatBookingDto
    {

        public string SeatNumber { get; set; }
        public string ClassType { get; set; }
        public string Position {  get; set; }
        
    }

}
