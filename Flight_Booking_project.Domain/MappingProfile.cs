using AutoMapper;
using Flight_Booking_project.Domain.Entities;
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
            CreateMap<Passenger, PassengerDto>();
        }
    }
}
