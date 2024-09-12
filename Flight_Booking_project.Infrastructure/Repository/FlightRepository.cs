using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Infrastructure.Repository
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlightBookingContext _context;

        public FlightRepository(FlightBookingContext context)
        {
            _context = context;
        }

        public async Task<Airport> GetAirportByNameAsync(string airportName)
        {
            return await _context.Airports
                .FirstOrDefaultAsync(a => a.Name.ToLower() == airportName.ToLower());
        }
        public async Task<Flight> GetFlightByIdAsync(int flightId)
        {
            return await _context.Flights
            .Include(f => f.Stops)
           .ThenInclude(s => s.Airport)  // Ensure that the airport for each stop is included
           .Include(f => f.Seats)
           .Include(f => f.Airline)
           .Include(f => f.DepartureAirport)
           .Include(f => f.ArrivalAirport)
           .FirstOrDefaultAsync(f => f.FlightId == flightId);

        }

        public async Task<List<Flight>> SearchFlightsAsync(int departureAirportId, int arrivalAirportId, FlightBasicSearchRequestDto searchRequest)
        {
            var query = _context.Flights
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.Airline)
                .Include(f => f.Seats)
                .Include(f => f.Stops)
                .AsQueryable();

            query = query.Where(f => f.DepartureAirportId == departureAirportId && f.ArrivalAirportId == arrivalAirportId);

            // Filter by class type and availability of seat
            if (!string.IsNullOrEmpty(searchRequest.ClassType))
            {
                query = query.Where(f => f.Seats.Any(s => s.ClassType == searchRequest.ClassType && s.IsAvailable));
            }

            if (searchRequest.DepartureDate != null)
            {
                query = query.Where(f => f.DepartureTime.Date == searchRequest.DepartureDate.Date);
            }

            return await query.ToListAsync();
        }


        public async Task<bool> CheckSeatAvailabilityAsync(int flightId, string classType, int passengerCount)
        {
            var flight = await _context.Flights
                .Include(f => f.Seats)
                .FirstOrDefaultAsync(f => f.FlightId == flightId);

            var availableSeats = flight.Seats.Count(s => s.ClassType == classType && s.IsAvailable);

            return availableSeats >= passengerCount;
        }

        public async Task<List<Flight>> SearchFlightsByAdvanceFilterAsync(int departureAirportId, int arrivalAirportId, string classType, DateTime departureDate, FlightAdvanceSearchRequestDto AdvsearchRequest)
        {
            var query = _context.Flights
        .Include(f => f.Seats)
        .Include(f => f.Airline)
        .Include(f => f.Stops)
        .AsQueryable();


            query = query.Where(f => f.DepartureAirportId == departureAirportId && f.ArrivalAirportId == arrivalAirportId);

            // Filter by class type and availability of seats
            if (!string.IsNullOrEmpty(classType))
            {
                query = query.Where(f => f.Seats.Any(s => s.ClassType == classType && s.IsAvailable));
            }

          


            // Filter by minimum price
            if (AdvsearchRequest.MinPrice.HasValue)
            {
                query = query.Where(f => f.Seats.Any(s => s.Price >= AdvsearchRequest.MinPrice.Value));
            }

            // Filter by maximum price
            if (AdvsearchRequest.MaxPrice.HasValue)
            {
                query = query.Where(f => f.Seats.Any(s => s.Price <= AdvsearchRequest.MaxPrice.Value));
            }

            // Filter by airline name
            if (!string.IsNullOrEmpty(AdvsearchRequest.AirlineName))
            {
                query = query.Where(f => f.Airline.AirlineName.ToLower() == AdvsearchRequest.AirlineName.ToLower());
            }

            // Filter by number of stops
            if (AdvsearchRequest.NumberOfStops.HasValue)
            {
                query = query.Where(f => f.Stops.Count == AdvsearchRequest.NumberOfStops.Value);
            }
            

  
            return await query.ToListAsync();

        }
    }

}
