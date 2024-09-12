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

namespace FlightBookingSystem.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IFlightRepository _flightRepository;

        public BookingService(IBookingRepository bookingRepository, IFlightRepository flightRepository)
        {
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
        }

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

            return basePrice * priceMultiplier;
        }

        private decimal CalculatePassengerPrice(decimal basePrice,PassengerRequestDto passenger)
        {



            if (passenger.Age < 12)
            {
                return basePrice * 0.5m;
            }
            else if (passenger.Age >= 65)
            {
                return basePrice * 0.7m;
            }
            else
            {
                return basePrice;
            }
        }


    }
}

