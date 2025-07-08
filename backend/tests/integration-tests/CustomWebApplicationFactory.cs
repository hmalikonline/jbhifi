using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Used for overriding API keys configuration.
/// Unfortunately, rate limiting configuration override isn't working and will require more work as rate limit scheme 
/// is built in the pipeline at the time of builder services, rather than app services and that means 
/// the configuration from app settings has been added and override settings here won't take affect.
/// For now, resorting to setting environment variables that align with controlled rate limiting tests.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<WeatherApp.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            // configuration overrides
            var overrideSettingsForTest = new Dictionary<string, string?>
            {
                { "ApplicationConfiguration:ApiKeys", TestConstants.ApiKeys },
                // { "ApplicationConfiguration:RateLimit:MaxRequests", TestConstants.RateLimit_MaxRequests.ToString() },
                // { "ApplicationConfiguration:RateLimit:TimeWindowInSeconds", TestConstants.RateLimit_TimeWindowInSeconds.ToString() },
            };
            configBuilder.AddInMemoryCollection(overrideSettingsForTest);
        });
    }
}

/// <summary>
/// Using simple mechanism to bypass rate limits by allowing a large number of requests per second. 
/// This will ensure tests that aren't checking rate limits such as data validation, api keys tests, can run in parallel without ever hiting rate limits
/// </summary>
public class CustomWebApplicationFactory_WithOut_RateLimits : WebApplicationFactory<WeatherApp.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            // configuration overrides
            var overrideSettingsForTest = new Dictionary<string, string?>
            {
                { "ApplicationConfiguration:ApiKeys", TestConstants.ApiKeys },
                { "ApplicationConfiguration:RateLimit:MaxRequests", "-1" }, //this will bypass rate limiting
                { "ApplicationConfiguration:RateLimit:TimeWindowInSeconds", "1" },
            };
            configBuilder.AddInMemoryCollection(overrideSettingsForTest);
        });
    }
}