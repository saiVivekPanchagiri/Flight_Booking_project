using AutoMapper;
using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
   
    public UserService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper )
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _mapper = mapper;
    }
    public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            throw new Exception("User already exists");
        }

        var user = _mapper.Map<User>(registerDto);
        

        await _userRepository.AddAsync(user);

        return _mapper.Map<UserDto>(user);
    }

    private async Task<UserDto> AuthenticateUser(UserDto user)
    {
        UserDto _user = null;

        try
        {
            var userVal = await _userRepository.GetByEmailAsync(user.Email);

            if (userVal != null && (user.Email == userVal.Email && user.Password == userVal.Password))
            {
                _user = user;
            }
        }
        catch (Exception ex)
        {
            return _user;
        }

        return _user;
    }

    private async Task<string> GenerateToken(UserDto user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], null,
            expires: DateTime.Now.AddMinutes(1),

  

            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public async Task<string> LoginAsync(UserDto user)
    {
        var token = "";
        try
        {
            var _user = await AuthenticateUser(user);
            if (_user != null)
            {
                token = await GenerateToken(user);
            }
        }
        catch (Exception e)
        {
            return token;
        }
        return token;
    }
}

