using AutoMapper;
//using FlightBookingSystem.Application.Interfaces;
using FlightBookingSystem.Application.Repository;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Application.IRepository;

namespace FlightBookingSystem.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingByFlightRepository _flightRepository;
        private readonly IMapper _mapper;
        private readonly IFlightRepository _flightRepository;

        decimal totalSeatPrice = 0;
        decimal totalPassengerPrice = 0;
        decimal refundAmount = 0;

        public BookingService(IBookingRepository bookingRepository, IBookingByFlightRepository flightRepository, IMapper mapper)
        public BookingService(IBookingRepository bookingRepository, IFlightRepository flightRepository)
        {
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
            _mapper = mapper;
        }






        /*
        public async Task<BookingResult> ConfirmBookingAsync(BookingRequestDto bookingRequestDto)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(bookingRequestDto.FlightId);

            if (flight == null)
            {
                return new BookingResult { IsSuccess = false, Message = "Flight not found" };
            }

            var seats = flight.Seats.Where(s => bookingRequestDto.SeatBookings.Any(sb => sb.SeatNumber == s.SeatNumber)).ToList();
            if (seats.Any(s => !s.IsAvailable))
            {
                return new BookingResult { IsSuccess = false, Message = "One or more seats are not available" };
            }

            var seatPrices = seats.ToDictionary(s => s.SeatNumber, s => s.Price);

            
            decimal totalSeatPrice = 0;
            decimal totalPassengerPrice = 0;
            foreach (var seatBooking in bookingRequestDto.SeatBookings)
            {
                if (seatPrices.TryGetValue(seatBooking.SeatNumber, out var seatPrice))
                {
                    
                    totalSeatPrice += CalculatePrice(seatPrice, seatBooking.ClassType, seatBooking.Position);
                    foreach (var passenger in bookingRequestDto.Passengers)
                    {
                       
                        totalPassengerPrice += CalculatePassengerPrice(seatPrice,passenger);
                    }
                }
            }

           
           


            var booking = new Booking
            {
                FlightId = flight.FlightId,
                UserId = bookingRequestDto.UserId,
                IsPaid=bookingRequestDto.IsPaid,
                Passengers = bookingRequestDto.Passengers.Select(p => new Passenger
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Gender = p.Gender,
                    Age = p.Age,
                    PhoneNumber = p.PhoneNumber,
                    Address = p.Address,
                    AlternativeContactNumber = p.AlternativeContactNumber
                }).ToList()
            };

            var savedBooking = await _bookingRepository.AddBookingAsync(booking);

            return new BookingResult
            {
                IsSuccess = true,
                Message = "Booking confirmed successfully",
                Booking = savedBooking,
                TotalPrice = totalSeatPrice + totalPassengerPrice,
                // TotalPrice = seats.Sum(s => s.Price),
                IsPaid = savedBooking.IsPaid
            };
        }
        */

        public async Task<BookingResult> ConfirmBookingAsync(BookingRequestDto bookingRequestDto)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(bookingRequestDto.FlightId);

            if (flight == null)
            {
                return new BookingResult { IsSuccess = false, Message = "Flight not found" };
            }

            var seats = flight.Seats.Where(s => bookingRequestDto.SeatBookings.Any(sb => sb.SeatNumber == s.SeatNumber)).ToList();
            if (seats.Any(s => !s.IsAvailable))
            {
                return new BookingResult { IsSuccess = false, Message = "One or more seats are not available" };
            }

            var seatPrices = seats.ToDictionary(s => s.SeatNumber, s => s.Price);

            decimal totalSeatPrice = 0;
            decimal totalPassengerPrice = 0;

            foreach (var seatBooking in bookingRequestDto.SeatBookings)
            {
                if (seatPrices.TryGetValue(seatBooking.SeatNumber, out var seatPrice))
                {
                    totalSeatPrice += CalculatePrice(seatPrice, seatBooking.ClassType, seatBooking.Position);
                    foreach (var passenger in bookingRequestDto.Passengers)
                    {
                        totalPassengerPrice += CalculatePassengerPrice(totalSeatPrice, passenger);
                    }
                }
            }

            //var refundAmount = (totalSeatPrice+totalPassengerPrice) * 0.95m;

            var booking = _mapper.Map<Booking>(bookingRequestDto);
            booking.FlightId = flight.FlightId;
            booking.IsPaid = bookingRequestDto.IsPaid;

            var savedBooking = await _bookingRepository.AddBookingAsync(booking);

            return new BookingResult
            {
                IsSuccess = true,
                Message = "Booking confirmed successfully",
                Booking = savedBooking,
                TotalPrice = totalSeatPrice + totalPassengerPrice,
                IsPaid = savedBooking.IsPaid
            };


        }



        public async Task<BookingResponseDto> CancelBookingAsync(int bookingId)
        {
            var isCancelled = await _bookingRepository.CancelBookingAsync(bookingId);

            if (!isCancelled)
            {
                return new BookingResponseDto
                {
                    IsSuccess = false,
                    Message = "Booking not found or cancellation failed."
                };
            }

            return new BookingResponseDto
            {
                IsSuccess = true,
                Message = "Booking canceled successfully and records deleted."
            };
        }







        /*
        public async Task<BookingResponseDto> CancelBookingAsync(int flightId)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(flightId);

            if (flight == null)
            {
                return new BookingResponseDto { IsSuccess = false, Message = "Flight not found" };
            }

            var bookings = await _bookingRepository.GetBookingsByFlightIdAsync(flightId);

            if (bookings == null || !bookings.Any())
            {
                return new BookingResponseDto { IsSuccess = false, Message = "No bookings found for this flight" };
            }

            foreach (var booking in bookings)
            {
                await _bookingRepository.DeleteBookingAsync(booking.BookingId);
            }

            return new Flight_Booking_project.Domain.EntitiesDto.ResponseDto.BookingResponseDto
            {
                IsSuccess = true,
                Message = "Bookings and associated passengers successfully canceled",
                RefundAmount = (totalSeatPrice + totalPassengerPrice) - ((0.05m) * (totalSeatPrice + totalPassengerPrice))
            };
        }
        */






        private decimal CalculatePrice(decimal basePrice, string classType, string position)
        {
            decimal priceMultiplier = 1;

            
            switch (classType)
            {
                case "Economy":
                    priceMultiplier = 100;
                    break;
                case "Business":
                    priceMultiplier = 200;
                    break;
                case "First":
                    priceMultiplier = 300;
                    break;
            }
            if (position == "Front")
            {
                priceMultiplier += 100; 
            }
            else if (position == "Middle")
            {
                priceMultiplier += 100; 
            }
            else if (position == "Back")
            {
                priceMultiplier += 100; 
            }
            */

        }

        {



            if (passenger.Age < 12)
            {
            }
            else if (passenger.Age >= 65)
            {
            }
            else
            {
                return basePrice;
            }
        }
    }
}



