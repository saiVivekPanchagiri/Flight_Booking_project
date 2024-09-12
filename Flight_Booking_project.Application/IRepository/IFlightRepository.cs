using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;

namespace FlightBookingSystem.Application.Repository
{
    public interface IFlightRepository { 
        //Task<List<Flight>> SearchFlightsAsync(FlightSearchRequestDto searchRequest); 
        Task<Flight> GetFlightByIdAsync(int flightId);
        Task<Airport> GetAirportByNameAsync(string airportName);

        Task<bool> CheckSeatAvailabilityAsync(int flightId, string classType, int passengerCount);
        Task<List<Flight>> SearchFlightsAsync(int departureAirportId, int arrivalAirportId, FlightBasicSearchRequestDto searchRequest);

        Task<List<Flight>> SearchFlightsByAdvanceFilterAsync(int departureAirportId,int arrivalAirportId,string classType,DateTime departureDate,FlightAdvanceSearchRequestDto searchRequest);


    }

}
