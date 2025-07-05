using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using WeatherApp;

namespace Weather.Tests;

public class ApiKeyTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> factory;
    private readonly HttpClient httpClient;
    private const string CITY = "Melbourne";
    private const string COUNTRY = "au";


    public ApiKeyTests(CustomWebApplicationFactory factory)
    {
        this.factory = factory;
        this.httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Verify_UnAuthorised_Access_When_Key_Not_Provided()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/weatherforecast?city={CITY}&country={COUNTRY}");
        var response = await httpClient.SendAsync(request);

        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }

    [Fact]
    public async Task Verify_UnAuthorised_Access_When_INVALID_Key_Provided()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/weatherforecast?city={CITY}&country={COUNTRY}");
        request.Headers.Add("x-api-key", "key1234");

        var response = await httpClient.SendAsync(request);

        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task Verify_Authorised_Access_When_VALID_Key_Provided()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/weatherforecast?city={CITY}&country={COUNTRY}");
        var apiKeyList = TestConstants.ApiKeys.Split(","); //get test api keys 
        request.Headers.Add("x-api-key", apiKeyList[0]);

        var response = await httpClient.SendAsync(request);

        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }    
             
}