using PostalService.Models;
using PostalService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPostalService, PostalService.Services.PostalService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/postcode/{code}", (string code, IPostalService postalService) =>
{
    var info = postalService.GetPostcodeInfo(code);
    if (info is null)
        return Results.NotFound(new { message = "Post code not found." });
    return Results.Ok(info);
});

app.Run();