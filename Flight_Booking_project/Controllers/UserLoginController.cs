// AuthController.cs
using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto;
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

    [HttpPost("register")]
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
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserDto userDto)
    {
        if (userDto == null)
        {
            return BadRequest("UserDto cannot be null.");
        }
        var token = await _userService.LoginAsync(userDto);

        if (!string.IsNullOrEmpty(token))
        {
            return Ok(new { token });
        }
        return Unauthorized("Invalid username or password.");
    }
}

