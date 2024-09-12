using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Application.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository _repository;

        public PassengerService(IPassengerRepository repository)
        {
            _repository = repository;
        }

        public async Task<(bool IsSuccess, IEnumerable<PassengerDto> Passengers, string Message)> GetPassengersByBookingIdAsync(int bookingId)
        {
            return await _repository.GetPassengersByBookingIdAsync(bookingId);
        }

        
    }
}
