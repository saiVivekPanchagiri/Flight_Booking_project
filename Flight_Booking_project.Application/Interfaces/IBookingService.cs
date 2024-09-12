using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Application.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResult> ConfirmBookingAsync(BookingRequestDto bookingRequestDto);
    }
}
