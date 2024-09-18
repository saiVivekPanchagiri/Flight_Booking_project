using AutoMapper;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

            return booking;
        }
                catch (Exception ex)
                {

                    await transaction.RollbackAsync();
                    throw;
                    
                }
            }

        }

        //public async Task<Seat> AddSeatAsync(Seat seat)
        //{
        //    await _context.Seats.AddAsync(seat);
        //    await _context.SaveChangesAsync();
        //    return seat;
        //}

        public async Task<Passenger> AddPassengerAsync(Passenger passenger)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
            await _context.Passengers.AddAsync(passenger);
            await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
            return passenger;
        }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Passengers)
                .SingleOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task DeleteBookingAsync(int bookingId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    var booking = await _context.Bookings
                        .Include(b => b.Passengers)
                        .SingleOrDefaultAsync(b => b.BookingId == bookingId);

                    if (booking != null)
                    {
                        _context.Bookings.Remove(booking);

                        // Remove associated passengers
                        _context.Passengers.RemoveRange(booking.Passengers);

                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }
        }

        public async Task<IEnumerable<Booking>> GetBookingsByFlightIdAsync(int flightId)
        {
            return await _context.Bookings
                .Where(b => b.FlightId == flightId)
                .Include(b => b.Passengers) // Include passengers if needed for the deletion
                .ToListAsync();
        }


        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Fetch the booking and include passengers
                    var booking = await _context.Bookings
                        .Include(b => b.Passengers)
                        .SingleOrDefaultAsync(b => b.BookingId == bookingId);

                    if (booking == null)
                    {
                        return false; // Booking not found
                    }

                    // Perform any additional cancellation logic or validation if needed.
                    // If the booking cannot be canceled, you can return false here.

                    // Remove the booking and associated passengers
                    _context.Passengers.RemoveRange(booking.Passengers);
                    _context.Bookings.Remove(booking);

                    // Commit the changes
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    // Rollback in case of any failure
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }


        // Fetch the updated booking details after a passenger is deleted

    }
}
