using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.ResponseDto
{

    public class SeatDetailsDto
    {
        public string SeatClass { get; set; }
        public string SeatPosition { get; set; }
        public decimal Price { get; set; }

    }
}
