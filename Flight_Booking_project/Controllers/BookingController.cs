using AutoMapper;
using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using FlightBookingSystem.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flight_Booking_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IPassengerService _passengerService;
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingController(IPassengerService passengerService, IBookingService bookingService,IMapper mapper)
        {
            _passengerService = passengerService;
            _bookingService = bookingService;
            _mapper = mapper;
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

        [HttpDelete("DeletePassengerList/{bookingId}")]
        public async Task<IActionResult> DeletePassengersByBookingId(int bookingId)
        {
            var result = await _passengerService.DeletePassengersByBookingIdAsync(bookingId);

            if (!result.IsSuccess)
            {
                return NotFound(new { Message = "No passengers found for the given booking ID." });
            }

            return Ok(new { Message = "Passengers deleted successfully." });
        }




        [HttpDelete("DeletePassengers/{passengerId}")]
        public async Task<IActionResult> DeletePassenger(int passengerId)
        {
            var result = await _passengerService.DeletePassengerAsync(passengerId);

            if (!result.IsSuccess)
            {
                return NotFound(new { Message = result.Message });
            }

            return Ok(new { Message = result.Message });
        }


        /*
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

        */



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

            var bookingDetailsDto = _mapper.Map<BookingDetailsDto>(bookingResult.Booking);

            // Set the TotalPrice manually
            bookingDetailsDto.TotalPrice = bookingResult.TotalPrice;

            var response = new BookingResponseDto
            {
                IsSuccess = bookingResult.IsSuccess,
                Message = bookingResult.Message,
                
                BookingDetails = bookingDetailsDto
            };

            return Ok(response);
        }


        [HttpDelete("cancel/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var bookingResult = await _bookingService.CancelBookingAsync(bookingId);

            if (!bookingResult.IsSuccess)
            {
                return BadRequest(new BookingResponseDto
                {
                    IsSuccess = false,
                    Message = bookingResult.Message
                });
            }

            return Ok(new BookingResponseDto
            {
                IsSuccess = true,
                Message = bookingResult.Message
            });
        }


        /*
        [HttpDelete("cancel/{flightId}")]
        public async Task<IActionResult> CancelBooking(int flightId)
        {
            var bookingResult = await _bookingService.CancelBookingAsync(flightId);

            if (!bookingResult.IsSuccess)
            {
                return BadRequest(new BookingResponseDto
                {
                    IsSuccess = false,
                    Message = bookingResult.Message
                });
            }

           // var bookingDetailsDto = _mapper.Map<BookingDetailsDto>(bookingResult.Booking);
           // bookingDetailsDto.RefundAmount = bookingResult.RefundAmount;

            var response = new BookingResponseDto
            {
                IsSuccess = bookingResult.IsSuccess,
                Message = bookingResult.Message,
                BookingDetails= bookingDetailsDto // Set refund amount manually
            };

            return Ok(response);
        }
        */

    }
}
