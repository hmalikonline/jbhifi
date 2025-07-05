using Microsoft.AspNetCore.Mvc.Testing;
using WeatherApp;

namespace Weather.Tests;

public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<WeatherApp.Program>>
{
    private readonly WebApplicationFactory<Program> factory;
    private readonly HttpClient httpClient;

    public ApiIntegrationTests(WebApplicationFactory<WeatherApp.Program> factory)
    {
        this.factory = factory;
        this.httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Test1()
    {
        string city = "Melbourne", country = "au";

        var response = await httpClient.GetAsync($"/weatherforecast?city={city}&country={country}");

        Assert.True(response.IsSuccessStatusCode);
    }
}