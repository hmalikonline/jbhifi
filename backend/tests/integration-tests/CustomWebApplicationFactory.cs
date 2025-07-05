using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

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
                { "ApplicationConfiguration:RateLimit:MaxRequests", TestConstants.RateLimit_MaxRequests.ToString() },
                { "ApplicationConfiguration:RateLimit:TimeWindowInSeconds", TestConstants.RateLimit_TimeWindowInSeconds.ToString() },
            };
            configBuilder.AddInMemoryCollection(overrideSettingsForTest);
        });
    }    
}