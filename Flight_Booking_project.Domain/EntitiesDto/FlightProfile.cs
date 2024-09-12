using AutoMapper;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Flight_Booking_project.Domain.EntitiesDto
{

    public class FlightProfile : Profile
    {
        public FlightProfile()
        {
            CreateMap<Flight, FlightResponseDto>()
                .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.FlightId))
                .ForMember(dest => dest.Airline, opt => opt.MapFrom(src => src.Airline.AirlineName))
                .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.DepartureAirport.Name))
                .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.ArrivalAirport.Name))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.DepartureTime))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
                .ForMember(dest => dest.NumberOfStops, opt => opt.MapFrom(src => src.Stops.Count))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Seats.FirstOrDefault().Price))
                .ForMember(dest => dest.SeatClass, opt => opt.MapFrom(src => src.Seats.FirstOrDefault().ClassType));

            CreateMap<Flight, FlightDetailsResponseDto>()
                .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.FlightId))
                .ForMember(dest => dest.Airline, opt => opt.MapFrom(src => src.Airline.AirlineName))
                .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.DepartureAirport.Name))
                .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.ArrivalAirport.Name))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.DepartureTime))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
                .ForMember(dest => dest.StopCount, opt => opt.MapFrom(src => src.Stops.Count))
                .ForMember(dest => dest.BaggageAllowance, opt => opt.MapFrom(src => src.Airline.BaggageAllowance))
                .ForMember(dest => dest.Stops, opt => opt.MapFrom(src => src.Stops.Select(s => new StopDetailsDto
                {
                    AirportName = s.Airport.Name,
                    StopDuration = s.StopTime
                })))
                .ForMember(dest => dest.Seats, opt => opt.MapFrom(src => src.Seats.Select(seat => new SeatDetailsDto
                {
                    SeatClass = seat.ClassType,
                    SeatPosition = seat.Position,
                    Price = seat.Price
                })));




            // Mapping from FlightSearchRequestDto to Flight entity
            CreateMap<FlightBasicSearchRequestDto, Flight>()
                // We don't directly map these string fields to the Flight entity
                // because they're used for searching, not setting.
                .ForMember(dest => dest.DepartureAirport, opt => opt.Ignore()) // Airport is mapped in query, not directly
                .ForMember(dest => dest.ArrivalAirport, opt => opt.Ignore()) // Same as above
                .ForMember(dest => dest.Seats, opt => opt.Ignore()) // Seats are not mapped directly in search
                .ForMember(dest => dest.Airline, opt => opt.Ignore()); // Airline is handled similarly in the service
        }
    }
}
