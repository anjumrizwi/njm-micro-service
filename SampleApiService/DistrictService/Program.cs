using DistrictService.Models;
using DistrictService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDistrictService, DistrictService.Services.DistrictService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/countries/{countryCode}/states/{stateCode}/districts", (string countryCode, string stateCode, IDistrictService districtService) =>
{
    var districts = districtService.GetDistricts(countryCode, stateCode);
    if (!districts.Any())
        return Results.NotFound("Country or State not found.");
    return Results.Ok(districts);
});

app.MapPost("/countries/{countryCode}/states/{stateCode}/districts", (string countryCode, string stateCode, District district, IDistrictService districtService) =>
{
    var result = districtService.AddDistrict(countryCode, stateCode, district);
    return result switch
    {
        DistrictAddResult.Success => Results.Created($"/countries/{countryCode}/states/{stateCode}/districts/{district.Code}", district),
        DistrictAddResult.Duplicate => Results.Conflict("District code already exists for this state."),
        DistrictAddResult.Invalid => Results.BadRequest("Code and Name are required."),
        _ => Results.StatusCode(500)
    };
});

app.Run();