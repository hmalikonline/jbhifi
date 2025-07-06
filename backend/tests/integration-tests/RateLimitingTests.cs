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
    private string[] apiKeyList = TestConstants.ApiKeys.Split(","); //get test api keys 


    public RateLimitingTests(CustomWebApplicationFactory factory)
    {
        this.factory = factory;
        this.httpClient = factory.CreateClient();
    }

    private async Task<HttpStatusCode> Call_API_After_A_Delay(int delayInSeconds, string apiKeyToUse)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/weatherforecast?city={CITY}&country={COUNTRY}");
        
        request.Headers.Add("x-api-key", apiKeyToUse);

        //if delay is specified, add delay
        if (delayInSeconds > 0)
            Thread.Sleep(delayInSeconds * 1000);

        var response = await httpClient.SendAsync(request);

        var status = response.StatusCode;

        response.Dispose();

        return status;
    }

    [Fact]
    public async Task Verify_Requests_Are_Allowed_Within_Rate_Limit()
    {
        var singleRequestInterval = (int)Math.Ceiling((double)TestConstants.RateLimit_TimeWindowInSeconds / TestConstants.RateLimit_MaxRequests);

        var apiKeyToUse = apiKeyList[0]; //use the first key in the list

        //making 2 requests

        //first request will be instant, subsequent requests will be delayed
        var responseStatus = await Call_API_After_A_Delay(0, apiKeyToUse);
        Assert.True(responseStatus == HttpStatusCode.OK);

        //making 2nd request in twice the acceptable interval, to stay within the rate limit
        responseStatus = await Call_API_After_A_Delay(singleRequestInterval * 2, apiKeyToUse);
        Assert.True(responseStatus == HttpStatusCode.OK);
    }

    [Fact]
    public async Task Verify_Requests_Are_Blocked_When_Rate_Limit_Is_Exceeded()
    {
        var singleRequestInterval = (int)Math.Ceiling((double)TestConstants.RateLimit_TimeWindowInSeconds / TestConstants.RateLimit_MaxRequests);

        var apiKeyToUse = apiKeyList[1]; //use the second key in the list, so it doesn't interfere with other tests

        //making 2 requests

        //1st request will be instant, subsequent requests will be delayed
        var responseStatus = await Call_API_After_A_Delay(0, apiKeyToUse);
        Assert.True(responseStatus == HttpStatusCode.OK);

        //making 2nd request in half the acceptable interval, to cause the request rate go above rate limit
        responseStatus = await Call_API_After_A_Delay((int)(singleRequestInterval / 2), apiKeyToUse);
        Assert.True(responseStatus == HttpStatusCode.TooManyRequests); //the second request should receive "too many requests" status

    }

    /// <summary>
    /// Making requests that will exceed rate limit, however because the requests are made with different api keys, they get processed
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Verify_Rate_Limit_Is_Paritioned_By_Keys()
    {
        var singleRequestInterval = (int)Math.Ceiling((double)TestConstants.RateLimit_TimeWindowInSeconds / TestConstants.RateLimit_MaxRequests);

        var apiKeyToUse = apiKeyList[2]; //use a key in the list
        
        //making 2 requests

        //first request will be instant, subsequent requests will be delayed
        var responseStatus = await Call_API_After_A_Delay(0, apiKeyToUse);
        Assert.True(responseStatus == HttpStatusCode.OK);

        //making 2nd request in half the acceptable interval, to cause the request rate go above rate limit
        //however, since 2nd request is made with a different api key, it goes through
        apiKeyToUse = apiKeyList[3]; //use another key in the list
        responseStatus = await Call_API_After_A_Delay((int)(singleRequestInterval / 2), apiKeyToUse);
        Assert.True(responseStatus == HttpStatusCode.OK);
    }

}