using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger"));

var countries = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
// Seed
countries.TryAdd("IN", "India");
countries.TryAdd("US", "United States");
countries.TryAdd("GB", "United Kingdom");

app.MapGet("/countries", () =>
{
    var list = countries.Select(kv => new { code = kv.Key.ToUpperInvariant(), name = kv.Value }).OrderBy(x => x.name);
    return Results.Ok(list);
});

app.MapPost("/countries", (Country country) =>
{
    if (string.IsNullOrWhiteSpace(country.Code) || string.IsNullOrWhiteSpace(country.Name))
        return Results.BadRequest("Code and Name are required.");
    if (!countries.TryAdd(country.Code.Trim(), country.Name.Trim()))
        return Results.Conflict("Country code already exists.");
    return Results.Created($"/countries/{country.Code}", country);
});

app.Run();

record Country(string Code, string Name);