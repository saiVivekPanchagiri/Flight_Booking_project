using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Application.Services;
using Flight_Booking_project.Domain;
using Flight_Booking_project.Infrastructure.Data;
using Flight_Booking_project.Infrastructure.Repository;
using FlightBookingSystem.Application.Repository;
using FlightBookingSystem.Application.Services;
using FlightBookingSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<FlightBookingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("EFCoreDBConnection"),b=>b.MigrationsAssembly("Flight_Booking_project")));
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();
builder.Services.AddScoped<IPassengerService, PassengerService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
