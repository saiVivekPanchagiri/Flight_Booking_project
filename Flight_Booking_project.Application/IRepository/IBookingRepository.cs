using Flight_Booking_project.Domain.Entities;

namespace FlightBookingSystem.Application.Repository
{
    public interface IBookingRepository
    {
        Task<Booking> AddBookingAsync(Booking booking);
        Task<Passenger> AddPassengerAsync(Passenger passenger);
    }
}
