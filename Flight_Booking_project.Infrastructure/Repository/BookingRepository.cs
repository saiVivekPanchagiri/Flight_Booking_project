using AutoMapper;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Infrastructure.Data;
using FlightBookingSystem.Application.Repository;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingSystem.Infrastructure
{
    public class BookingRepository:IBookingRepository
    {

        private readonly FlightBookingContext _context;
        private readonly IMapper _mapper;

        public BookingRepository(FlightBookingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Flight> GetFlightByIdAsync(int flightId)
        {
            return await _context.Flights
                .Include(f => f.Seats)
                .SingleOrDefaultAsync(f => f.FlightId == flightId);
        }

        public async Task<Booking> AddBookingAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Seat> AddSeatAsync(Seat seat)
        {
            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();
            return seat;
        }

        public async Task<Passenger> AddPassengerAsync(Passenger passenger)
        {
            await _context.Passengers.AddAsync(passenger);
            await _context.SaveChangesAsync();
            return passenger;
        }


    }
}
