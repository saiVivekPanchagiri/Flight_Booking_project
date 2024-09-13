using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto
{
    public class UserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    //public string Name { get; set; }
    //public long PhoneNumber { get; set; }
    //public string Gender { get; set; }
    //public string Address { get; set; }
    //public long? AlternativeContactNumber { get; set; }
    //public List<BookingDto> Bookings { get; set; }
}
