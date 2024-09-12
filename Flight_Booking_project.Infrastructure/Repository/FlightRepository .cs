using AutoMapper;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Infrastructure.Data;
using FlightBookingSystem.Application.Repository;
using FlightBookingSystem.Application.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBookingSystem.Infrastructure
{
    public class FlightRepository:IFlightRepository
    {
        private readonly FlightBookingContext _context;
        private readonly IMapper _mapper;
        public FlightRepository(FlightBookingContext context)
        {
            _context = context;
        }

        public async Task<Flight> GetFlightByIdAsync(int flightId)
        {
            return await _context.Flights
                .Include(f => f.Seats)
                .SingleOrDefaultAsync(f => f.FlightId == flightId);
        }
    }


}
