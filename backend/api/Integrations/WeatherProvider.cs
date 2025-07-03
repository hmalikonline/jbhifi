public interface IWeatherProvider
{
    public Task<Weather?> GetWeatherAsync(Location location);
}

/// <summary>
/// Fetches weather data from a provider such as OpenWeatherMap
/// </summary>
public class WeatherProvider : IWeatherProvider
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<WeatherProvider> logger;

    public WeatherProvider(IHttpClientFactory httpClientFactory, ILogger<WeatherProvider> logger)
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;
    }
    public async Task<Weather?> GetWeatherAsync(Location location)
    {
        throw new NotImplementedException();
    }
}