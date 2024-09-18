using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.ResponseDto
{
    public class BookingResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } // Additional message or error information

        public BookingDetailsDto BookingDetails { get; set; } // Booking details if successful
    }
}


