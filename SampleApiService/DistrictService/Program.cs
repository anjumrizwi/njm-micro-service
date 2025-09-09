using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger"));

// store: country -> state -> district list
var store = new ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, string>>>(StringComparer.OrdinalIgnoreCase);

// seed
store.TryAdd("IN", new(StringComparer.OrdinalIgnoreCase));
store["IN"].TryAdd("BR", new(StringComparer.OrdinalIgnoreCase));
store["IN"]["BR"].TryAdd("BH", "Bhagalpur");
store["IN"]["BR"].TryAdd("PT", "Patna");

app.MapGet("/countries/{countryCode}/states/{stateCode}/districts", (string countryCode, string stateCode) =>
{
    if (!store.TryGetValue(countryCode, out var states)) return Results.NotFound("Country not found.");
    if (!states.TryGetValue(stateCode, out var districts)) return Results.NotFound("State not found.");
    var list = districts.Select(kv => new { code = kv.Key.ToUpperInvariant(), name = kv.Value }).OrderBy(x => x.name);
    return Results.Ok(list);
});

app.MapPost("/countries/{countryCode}/states/{stateCode}/districts", (string countryCode, string stateCode, District district) =>
{
    if (string.IsNullOrWhiteSpace(district.Code) || string.IsNullOrWhiteSpace(district.Name))
        return Results.BadRequest("Code and Name are required.");
    var states = store.GetOrAdd(countryCode, _ => new(StringComparer.OrdinalIgnoreCase));
    var districts = states.GetOrAdd(stateCode, _ => new(StringComparer.OrdinalIgnoreCase));
    if (!districts.TryAdd(district.Code.Trim(), district.Name.Trim()))
        return Results.Conflict("District code already exists for this state.");
    return Results.Created($"/countries/{countryCode}/states/{stateCode}/districts/{district.Code}", district);
});

app.Run();

record District(string Code, string Name);