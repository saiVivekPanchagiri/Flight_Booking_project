using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;

namespace Flight_Booking_project.Application.IRepository
{
    public interface IFlightRepository
    {
        Task<Flight> GetFlightByIdAsync(int flightId);
        Task<Airport> GetAirportByNameAsync(string airportName);

        Task<bool> CheckSeatAvailabilityAsync(int flightId, string classType, int passengerCount);
        Task<List<Flight>> SearchFlightsAsync(int departureAirportId, int arrivalAirportId, FlightBasicSearchRequestDto searchRequest);

        Task<List<Flight>> SearchFlightsByAdvanceFilterAsync(int departureAirportId,int arrivalAirportId,string classType,DateTime departureDate,FlightAdvanceSearchRequestDto searchRequest);


    }

}
