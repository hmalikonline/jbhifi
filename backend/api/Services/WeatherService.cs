public interface IWeatherService
{
    public Task<Weather?> GetWeatherAsync(Location location);
}

public class WeatherService : IWeatherService
{
    private readonly IWeatherProvider weatherProvider;
    private readonly ILogger<WeatherService> logger;

    public WeatherService(IWeatherProvider weatherProvider, ILogger<WeatherService> logger)
    {
        this.weatherProvider = weatherProvider;
        this.logger = logger;
    }
    public async Task<Weather?> GetWeatherAsync(Location location)
    {
        return await weatherProvider.GetWeatherAsync(location);
    }
}