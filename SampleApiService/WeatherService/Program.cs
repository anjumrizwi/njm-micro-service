using WeatherService.Models;
using WeatherService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWeatherService, WeatherService.Services.WeatherService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/weather/forecast", (string? city, int days, IWeatherService weatherService) =>
{
    var result = weatherService.GetForecast(city, days);
    return Results.Ok(result);
});

app.Run();