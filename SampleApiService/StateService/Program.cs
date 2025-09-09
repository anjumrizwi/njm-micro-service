using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger"));

// store: countryCode -> dictionary of states (stateCode -> stateName)
var store = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

// seed
store.TryAdd("IN", new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase));
store["IN"].TryAdd("KA", "Karnataka");
store["IN"].TryAdd("BR", "Bihar");
store.TryAdd("US", new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase));
store["US"].TryAdd("CA", "California");
store["US"].TryAdd("WA", "Washington");

app.MapGet("/countries/{countryCode}/states", (string countryCode) =>
{
    if (!store.TryGetValue(countryCode, out var states)) return Results.NotFound("Country not found.");
    var list = states.Select(kv => new { code = kv.Key.ToUpperInvariant(), name = kv.Value }).OrderBy(x => x.name);
    return Results.Ok(list);
});

app.MapPost("/countries/{countryCode}/states", (string countryCode, State state) =>
{
    if (string.IsNullOrWhiteSpace(state.Code) || string.IsNullOrWhiteSpace(state.Name))
        return Results.BadRequest("Code and Name are required.");
    var states = store.GetOrAdd(countryCode, _ => new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase));
    if (!states.TryAdd(state.Code.Trim(), state.Name.Trim()))
        return Results.Conflict("State code already exists for this country.");
    return Results.Created($"/countries/{countryCode}/states/{state.Code}", state);
});

app.Run();

record State(string Code, string Name);