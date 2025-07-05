using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

//Add dependencies
builder.Services.AddScoped<IWeatherProvider, WeatherProvider>();
builder.Services.AddScoped<IWeatherService, WeatherService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add rate limiting scheme for api key
builder.Services.AddRateLimitingSchemeByApiKey(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//inject custom middleware to check for api keys
app.UseMiddleware<ApiKeyMiddleware>();
//inject rate limiter to check for rate limits
app.UseRateLimiter();

app.MapGet("/weatherforecast", async (string city, string country, IWeatherService service) =>
{
    var location = new Location
    {
        City = city,
        Country = country
    };
   // var weather = await service.GetWeatherAsync(location);

    var weather = new Weather
    {
        Description = "test"
    };

    if (weather == null)
        return Results.NotFound();

    return Results.Ok(weather.Description); //return only the weather description

})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

namespace WeatherApp
{
    public partial class Program
    { }
}
