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

        [HttpPost("Basicsearch")]
        public async Task<IActionResult> SearchFlights([FromBody] FlightBasicSearchRequestDto searchRequest)
        {
            try
            {
                var flights = await _flightService.SearchFlightsAsync(searchRequest);
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPost("AdvanceFilterSearch")]
        public async Task<IActionResult> SearchFlightsByAdvanceFilter([FromBody] FlightAdvanceSearchRequestDto searchRequest)
        {
            try
            {
                var flights = await _flightService.SearchFlightsByAdvanceFilterAsync(searchRequest);
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


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
