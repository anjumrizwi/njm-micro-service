var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger"));

// Fake lookup with a few examples
var data = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
{
    ["812006"] = new { country="IN", state="BR", district="Bhagalpur", area="Sajour", code="812006" },
    ["560083"] = new { country="IN", state="KA", district="Bengaluru Urban", area="Bannerghatta Road", code="560083" },
    ["94105"]  = new { country="US", state="CA", district="San Francisco", area="SOMA", code="94105" }
};

app.MapGet("/postcode/{code}", (string code) =>
{
    if (data.TryGetValue(code, out var item)) return Results.Ok(item);
    return Results.NotFound(new { message = "Post code not found." });
});

app.Run();