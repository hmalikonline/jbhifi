using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using WeatherApp;

namespace Weather.Tests;

public class ValidationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> factory;
    private readonly HttpClient httpClient;

    public ValidationTests(CustomWebApplicationFactory factory)
    {
        this.factory = factory;
        this.httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Verify_City_Is_Being_Validated()
    {
        var apiKeyList = TestConstants.ApiKeys.Split(","); //get test api keys         
        var country = "au";
        var cities = new (bool isValid, string cityName)[] { (false, String.Empty), (false,"a$9s!sg"), (true, "Melbourne") }; 

        for (int i=0; i < cities.Length; i++)
        {
            //calling api with different keys to ensure rate limit isn't hit during tests
            var result = await CallAPI(apiKeyList[i], cities[i].cityName, country);

            if (cities[i].isValid)
            {
                //verify api returns Ok, if valid city was provided
                Assert.Equal(StatusCodes.Status200OK, (int)result);                
            }
            else
            {
                //verify api returns bad request, if an invalid city was provided
                Assert.Equal(StatusCodes.Status400BadRequest, (int)result);
            }
        }
    }

    [Fact]
    public async Task Verify_Country_Is_Being_Validated()
    {
        var apiKeyList = TestConstants.ApiKeys.Split(","); //get test api keys         
        var city = "Melbourne";
        var countries = new (bool isValid, string countryCode)[]
                        { (false, String.Empty), (false,"a$9s!sg"), (false, "aus"), (true, "au") }; 

        for (int i=0; i < countries.Length; i++)
        {
            //calling api with different keys to ensure rate limit isn't hit during tests
            var result = await CallAPI(apiKeyList[i], city, countries[i].countryCode);

            if (countries[i].isValid)
            {
                //verify api returns Ok, if valid country was provided
                Assert.Equal(StatusCodes.Status200OK, (int)result);                
            }
            else
            {
                //verify api returns bad request, if an invalid country was provided
                Assert.Equal(StatusCodes.Status400BadRequest, (int)result);
            }
        }
    }

    private async Task<HttpStatusCode> CallAPI(string apiKeyToUse, string city, string country)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/weatherforecast?city={city}&country={country}");

        request.Headers.Add("x-api-key", apiKeyToUse);

        var response = await httpClient.SendAsync(request);

        var status = response.StatusCode;

        response.Dispose();

        return status;
    }            

}
