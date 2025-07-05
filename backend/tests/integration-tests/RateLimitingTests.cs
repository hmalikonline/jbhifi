using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using WeatherApp;

namespace Weather.Tests;

public class RateLimitingTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> factory;
    private readonly HttpClient httpClient;
    private const string CITY = "Melbourne";
    private const string COUNTRY = "au";


    public RateLimitingTests(CustomWebApplicationFactory factory)
    {
        this.factory = factory;
        this.httpClient = factory.CreateClient();
    }

    private async Task<HttpStatusCode> Call_API_After_A_Delay(int delayInSeconds)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/weatherforecast?city={CITY}&country={COUNTRY}");
        var apiKeyList = TestConstants.ApiKeys.Split(","); //get test api keys 
        request.Headers.Add("x-api-key", apiKeyList[0]);

        //if delay is specified, add delay
        if (delayInSeconds > 0)
            Thread.Sleep(delayInSeconds * 1000);

        var response = await httpClient.SendAsync(request);

        return response.StatusCode;        
    }

    [Fact]
    public async Task Verify_Requests_Are_Allowed_Within_Rate_Limit()
    {
        var singleRequestInterval = (int)Math.Ceiling((double)TestConstants.RateLimit_TimeWindowInSeconds / TestConstants.RateLimit_MaxRequests);

        //making 2 requests with a delay that's within acceptable rate limit
        //first request will be instant, subsequent requests will be delayed
        var responseStatus = await Call_API_After_A_Delay(0); 
        Assert.True(responseStatus == HttpStatusCode.OK); 

        //making 2nd request in twice the acceptable interval, to stay within the rate limit
        responseStatus = await Call_API_After_A_Delay(singleRequestInterval*2);
        Assert.True(responseStatus == HttpStatusCode.OK); 
    }

    [Fact]
    public async Task Verify_Requests_Are_Blocked_Beyond_Rate_Limit()
    {
        var singleRequestInterval = (int)Math.Ceiling((double)TestConstants.RateLimit_TimeWindowInSeconds / TestConstants.RateLimit_MaxRequests);

        //making 2 requests 
        //1st request will be instant, subsequent requests will be delayed
        var responseStatus = await Call_API_After_A_Delay(0); 
        Assert.True(responseStatus == HttpStatusCode.OK); 

        //making 2nd request in half the acceptable interval, to cause the request rate go above rate limit
        responseStatus = await Call_API_After_A_Delay((int)singleRequestInterval/2); 
        Assert.True(responseStatus == HttpStatusCode.TooManyRequests); //the second request should receive "too many requests" status
       
    }
    
  
             
}