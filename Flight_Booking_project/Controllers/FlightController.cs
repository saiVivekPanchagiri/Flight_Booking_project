using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Microsoft.AspNetCore.Mvc;

namespace Flight_Booking_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet("Basicsearch")]
        public async Task<IActionResult> SearchFlights([FromQuery] string DepartureAirportName,[FromQuery] string ArrivalAirportName, [FromQuery] string ClassType, [FromQuery] DateTime DepartureDate, [FromQuery] int NumberOfPassengers)
        {
            try
            {
                var flights = await _flightService.SearchFlightsAsync(DepartureAirportName,ArrivalAirportName,ClassType,DepartureDate,NumberOfPassengers);
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
       /* [HttpGet("AdvanceFilterSearch")]
        public async Task<IActionResult> SearchFlightsByAdvanceFilter( [FromQuery]decimal? MinPrice, [FromQuery] decimal? MaxPrice, [FromQuery] string? AirlineName, [FromQuery] int? NumberOfStops)
        {
            try
            {
                var flights = await _flightService.SearchFlightsByAdvanceFilterAsync( MinPrice,  MaxPrice, AirlineName, NumberOfStops);
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }*/


        [HttpGet("{flightId}")]
        public async Task<IActionResult> GetFlightDetails(int flightId)
        {
            try
            {
                var flightDetails = await _flightService.GetFlightByIdAsync(flightId);
                if (flightDetails == null)
                {
                    return NotFound();
                }
                return Ok(flightDetails);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }

}
