using AutoMapper;
using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using Flight_Booking_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Infrastructure.Repository
{
    public class PassengerRepository:IPassengerRepository
    {
        private readonly FlightBookingContext _context;
        private readonly IMapper _mapper;


        public PassengerRepository(FlightBookingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool IsSuccess, IEnumerable<PassengerDto> Passengers, string Message)> GetPassengersByBookingIdAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .Where(b => b.BookingId == bookingId)
                .Include(b => b.Passengers)
                .FirstOrDefaultAsync();

            if (booking == null)
            {
                return (false, Enumerable.Empty<PassengerDto>(), $"Booking with ID {bookingId} not found.");
            }

            var passengers = _mapper.Map<IEnumerable<PassengerDto>>(booking.Passengers);
            return (true, passengers, string.Empty);
        }
    }
}
