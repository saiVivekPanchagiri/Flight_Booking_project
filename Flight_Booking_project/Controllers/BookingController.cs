using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using FlightBookingSystem.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flight_Booking_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IPassengerService _passengerService;
        private readonly IBookingService _bookingService;

        public BookingController(IPassengerService passengerService, IBookingService bookingService)
        {
            _passengerService = passengerService;
            _bookingService = bookingService;
        }

        [HttpGet("Passengers/{bookingId}")]
        public async Task<IActionResult> GetPassengersByBookingId(int bookingId)
        {
            var (isSuccess, passengers, message) = await _passengerService.GetPassengersByBookingIdAsync(bookingId);

            if (!isSuccess)
            {
                // Return a 404 Not Found response with a custom message
                return NotFound(new { Message = message });
            }
            return Ok(passengers);
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmBooking([FromBody] BookingRequestDto bookingRequestDto)
        {
            var bookingResult = await _bookingService.ConfirmBookingAsync(bookingRequestDto);

            if (!bookingResult.IsSuccess)
            {
                return BadRequest(new BookingResponseDto
                {
                    IsSuccess = false,
                    Message = bookingResult.Message
                });
            }

            var response = new BookingResponseDto
            {
                IsSuccess = bookingResult.IsSuccess,
                Message = bookingResult.Message,
                BookingDetails = new BookingDetailsDto
                {
                    BookingId = bookingResult.Booking.BookingId,
                    FlightId = bookingResult.Booking.FlightId,
                    Passengers = bookingResult.Booking.Passengers.Select(p => new PassengerDto
                    {
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        Gender = p.Gender,
                        Age = p.Age,
                        PhoneNumber = p.PhoneNumber,
                        Address = p.Address,
                        AlternativeContactNumber = p.AlternativeContactNumber
                    }).ToList(),
                    TotalPrice = bookingResult.TotalPrice,
                    IsPaid = bookingResult.IsPaid
                }
            };

            return Ok(response);
        }
    }
}
