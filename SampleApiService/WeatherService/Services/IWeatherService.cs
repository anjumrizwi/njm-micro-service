using WeatherService.Models;

namespace WeatherService.Services;

public interface IWeatherService
{
    IEnumerable<WeatherForecast> GetForecast(string? city, int days);
}