using CountryService.Models;
using CountryService.Services;

var builder = WebApplication.CreateBuilder(args);

// Fix: Specify the correct implementation type for ICountryService
builder.Services.AddSingleton<ICountryService, CountryService.Services.CountryService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/countries", (ICountryService countryService) =>
    Results.Ok(countryService.GetAllCountries())
);

app.MapPost("/countries", (Country country, ICountryService countryService) =>
{
    var result = countryService.AddCountry(country);
    return result switch
    {
        CountryAddResult.Success => Results.Created($"/countries/{country.Code}", country),
        CountryAddResult.Duplicate => Results.Conflict("Country code already exists."),
        CountryAddResult.Invalid => Results.BadRequest("Code and Name are required."),
        _ => Results.StatusCode(500)
    };
});

app.Run();