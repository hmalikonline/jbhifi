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
    private readonly IConfiguration configuration;

    public WeatherProvider(IHttpClientFactory httpClientFactory, ILogger<WeatherProvider> logger, IConfiguration configuration)
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;
        this.configuration = configuration;
    }
    public async Task<Weather?> GetWeatherAsync(Location location)
    {
        var weatherProviderAPIBaseURL = configuration.GetSection("OpenWeatherMap").GetValue<string>("BaseAPIURL") ?? throw new ApplicationException("Open weather map provider API url isn't configured");       

        var apiKeys = configuration.GetSection("Openweathermap:APIKeys").Get<string>() ?? throw new ApplicationException("Open weather map provider API keys aren't configured. Please provide one or more keys separated with comma."); ;
        string[] weatherProviderAPIKeys = apiKeys.Split(",");

        if (!weatherProviderAPIKeys.Any()) throw new ApplicationException("Open weather map provider API keys aren't configured"); ;

        //for now spread the usage of keys randomly, this can be improved by implementing a more sophisticated mechanism such as round robin for even spread of key usage
        int keyIndexToBeConsumed = Random.Shared.Next(weatherProviderAPIKeys.Length);

        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(weatherProviderAPIBaseURL);
        
        //call api
        var response = await httpClient.GetAsync($"?q={location.City},{location.Country}&appid={weatherProviderAPIKeys[keyIndexToBeConsumed]}");

        if (response.IsSuccessStatusCode)
        {
            //parse api data
            var weatherData = await response.Content.ReadFromJsonAsync<OpenWeatherData>();
            if (weatherData != null && weatherData.Weather != null && weatherData.Weather.Any())
            {
                return new Weather
                {
                    Description = weatherData.Weather.First().Description
                };
            }
        }

        return null; //return null if weather couldn't be obtained from the provider
    }

    public class OpenWeatherData
    {
        public Weather[]? Weather { get; set; } //weather property in API response
    }
}