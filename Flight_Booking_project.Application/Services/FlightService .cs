using AutoMapper;
using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;


        /*private int _cachedDepartureAirportId;
        private int _cachedArrivalAirportId;
        private string _cachedClassType;
        private DateTime _cacheddepatureDate;*/
        public FlightService(IFlightRepository flightRepository, IMapper mapper)
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
        }

        public async Task<List<FlightResponseDto>> SearchFlightsAsync(string DepartureAirportName, string ArrivalAirportName, string ClassType, DateTime DepartureDate, int NumberOfPassengers)
        {
            if (string.IsNullOrEmpty(DepartureAirportName) || string.IsNullOrEmpty(ArrivalAirportName))
            {
                throw new Exception("Both Departure and Arrival Airport Names are required.");
            }

            // Get the departure and arrival airports
            var departureAirport = await _flightRepository.GetAirportByNameAsync(DepartureAirportName);
            var arrivalAirport = await _flightRepository.GetAirportByNameAsync(ArrivalAirportName);



            if (departureAirport == null || arrivalAirport == null)
            {
                throw new Exception("Departure or Arrival airport not found");
            }
           /* _cachedArrivalAirportId=arrivalAirport.AirportId;
            _cachedDepartureAirportId = departureAirport.AirportId;
            _cachedClassType=ClassType;
            _cacheddepatureDate = DepartureDate;
*/

            // Fetch flights based on the search criteria
            var flights = await _flightRepository.SearchFlightsAsync(departureAirport.AirportId, arrivalAirport.AirportId,  DepartureAirportName, ArrivalAirportName, ClassType, DepartureDate,  NumberOfPassengers);

            var availableFlights = new List<Flight>();
            foreach (var flight in flights)
            {
                var isAvailable = await _flightRepository.CheckSeatAvailabilityAsync(
                    flight.FlightId, ClassType, NumberOfPassengers);

                if (isAvailable)
                {
                    availableFlights.Add(flight);
                }
            }

            if (availableFlights == null || !availableFlights.Any())
            {
                throw new Exception("No flights found based on the search criteria.");
            }

            // Map the flight data to FlightResponseDto
            var flightDtos = _mapper.Map<List<FlightResponseDto>>(availableFlights);
            return flightDtos;
        }

      /* public async Task<List<FlightResponseDto>> SearchFlightsByAdvanceFilterAsync(decimal? MinPrice, decimal? MaxPrice, string? AirlineName, int? NumberOfStops)
        {

            *//*if (_cachedDepartureAirportId == 0 || _cachedArrivalAirportId == 0 || string.IsNullOrEmpty(_cachedClassType) || _cacheddepatureDate == default(DateTime))
            {
                throw new Exception("Basic search must be completed before applying advanced filters.");
            }*//*
            var flights = await _flightRepository.SearchFlightsByAdvanceFilterAsync( MinPrice,  MaxPrice,  AirlineName, NumberOfStops);
            if (flights == null || !flights.Any())
            {
                throw new Exception("No flights found based on the search criteria.");
            }
            return _mapper.Map<List<FlightResponseDto>>(flights);

        }*/

        public async Task<FlightDetailsResponseDto> GetFlightByIdAsync(int flightId)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(flightId);

            if (flight == null)
            {
                throw new Exception("Flight not found");
            }

            // Using AutoMapper to map the flight entity to FlightDetailsDto
            var flightDetailsDto = _mapper.Map<FlightDetailsResponseDto>(flight);

            return flightDetailsDto;
        }
    }

}
