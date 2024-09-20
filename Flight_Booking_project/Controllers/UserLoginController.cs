// AuthController.cs
using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UserLoginController : ControllerBase
{
    private readonly IUserService _userService;
    public UserLoginController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            await _userService.RegisterAsync(registerDto);
            return Ok("User registered successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<LoginResultDto>> Login([FromBody] UserDto userDto)
    {
        if (userDto == null)
        {
            return BadRequest("UserDto cannot be null.");
        }

        var loginResult = await _userService.LoginAsync(userDto);

        if (loginResult != null)
        {
            return Ok(loginResult); // Return LoginResultDto in the response
        }

        return Unauthorized("Invalid username or password.");
    }

    [HttpGet("UserByEmail")]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email is required."); // Return 400 Bad Request if the email is not provided
        }

        var userDto = new RegisterDto { Email = email }; // Create UserDto with the email
        var user = await _userService.GetUserByEmail(userDto);

        if (user == null)
        {
            return NotFound(); // Return 404 Not Found if the user is not found
        }
        return Ok(user); // Return 200 OK with the user data
    }
}

