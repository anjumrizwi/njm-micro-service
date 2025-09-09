using StateService.Models;
using StateService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IStateService, StateService.Services.StateService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/countries/{countryCode}/states", (string countryCode, IStateService stateService) =>
{
    var states = stateService.GetStates(countryCode);
    if (!states.Any())
        return Results.NotFound("Country not found.");
    return Results.Ok(states);
});

app.MapPost("/countries/{countryCode}/states", (string countryCode, State state, IStateService stateService) =>
{
    var result = stateService.AddState(countryCode, state);
    return result switch
    {
        StateAddResult.Success => Results.Created($"/countries/{countryCode}/states/{state.Code}", state),
        StateAddResult.Duplicate => Results.Conflict("State code already exists for this country."),
        StateAddResult.Invalid => Results.BadRequest("Code and Name are required."),
        _ => Results.StatusCode(500)
    };
});

app.Run();