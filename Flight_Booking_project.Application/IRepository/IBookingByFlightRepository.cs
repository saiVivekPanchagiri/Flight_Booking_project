using Flight_Booking_project.Domain.Entities;

namespace FlightBookingSystem.Application.Repository
{
    public interface IBookingByFlightRepository 
    {
        Task<Flight> GetFlightByIdAsync(int flightId);
    }
}
