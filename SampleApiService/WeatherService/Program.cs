using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/weather/forecast", (string? city, int days) =>
{
    var now = DateTime.UtcNow;
    var rng = new Random(city?.GetHashCode() ?? now.Millisecond);
    var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
    var result = Enumerable.Range(1, Math.Clamp(days,1,10)).Select(i => new
    {
        date = now.AddDays(i).ToString("yyyy-MM-dd"),
        temperatureC = rng.Next(-10, 42),
        summary = summaries[rng.Next(summaries.Length)],
        city = city ?? "Unknown"
    });
    return Results.Ok(result);
});

app.Run();