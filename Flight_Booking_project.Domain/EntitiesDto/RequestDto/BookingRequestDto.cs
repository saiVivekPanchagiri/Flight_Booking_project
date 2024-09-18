﻿namespace Flight_Booking_project.Domain.EntitiesDto.RequestDto
{
    public class BookingRequestDto
    {
        public int UserId { get; set; }
        public int FlightId { get; set; }
        public ICollection<SeatBookingDto> SeatBookings { get; set; } // Seats selected by the user
        public ICollection<PassengerRequestDto> Passengers { get; set; } // Passengers details
        public bool IsPaid { get; set; } = false; // Default to false
    }

}
