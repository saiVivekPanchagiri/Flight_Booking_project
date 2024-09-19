using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;

namespace FlightBookingSystem.Application.Repository
{
    public interface IBookingRepository
    {

        Task<Booking> AddBookingAsync(Booking booking);
        Task<Passenger> AddPassengerAsync(Passenger passenger);
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task DeleteBookingAsync(int bookingId);

        Task<IEnumerable<Booking>> GetBookingsByFlightIdAsync(int flightId);

        Task<bool> CancelBookingAsync(int bookingId);

        




    }
}
