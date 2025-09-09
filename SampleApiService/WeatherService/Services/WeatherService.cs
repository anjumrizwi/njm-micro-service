using WeatherService.Models;

namespace WeatherService.Services;

public class WeatherService : IWeatherService
{
    public IEnumerable<WeatherForecast> GetForecast(string? city, int days)
    {
        var now = DateTime.UtcNow;
        var rng = new Random(city?.GetHashCode() ?? now.Millisecond);
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        return Enumerable.Range(1, Math.Clamp(days, 1, 10)).Select(i => new WeatherForecast(
            now.AddDays(i).ToString("yyyy-MM-dd"),
            rng.Next(-10, 42),
            summaries[rng.Next(summaries.Length)],
            city ?? "Unknown"
        ));
    }
}