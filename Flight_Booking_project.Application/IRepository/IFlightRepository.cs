using Flight_Booking_project.Domain.Entities;

namespace FlightBookingSystem.Application.Repository
{
    public interface IFlightRepository { 
        //Task<List<Flight>> SearchFlightsAsync(FlightSearchRequestDto searchRequest); 
        Task<Flight> GetFlightByIdAsync(int flightId);
    }
}
