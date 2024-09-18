using AutoMapper;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // DTO to Domain Model
            CreateMap<BookingRequestDto, Booking>()
                .ForMember(dest => dest.Passengers, opt => opt.MapFrom(src => src.Passengers));

            CreateMap<PassengerRequestDto, Passenger>();
            CreateMap<SeatBookingDto, Seat>()
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.SeatNumber));

            // Domain Model to DTO
            CreateMap<Booking, BookingDetailsDto>()
                .ForMember(dest => dest.Passengers, opt => opt.MapFrom(src => src.Passengers));

            CreateMap<Passenger, PassengerDto>();
            CreateMap<Seat, SeatDto>();



        }
    }
}
