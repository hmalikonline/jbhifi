using System.Threading.RateLimiting;

public class RateLimitConfiguration
{
        public int MaxRequests { get; set; }
        public int TimeWindowInSeconds { get; set; }
}

public static class ApiKeyRateLimitingScheme
{
    public static IServiceCollection AddRateLimitingSchemeByApiKey(this IServiceCollection services, IConfiguration configuration)
    {
        //fetching rate limiting options from configuration
        RateLimitConfiguration? rateLimitConfig = configuration
                                                        .GetSection("ApplicationConfiguration:RateLimit")
                                                        .Get<RateLimitConfiguration>() ?? throw new ApplicationException("Rate limiting options haven't been configured");

        var rateLimitTimeWindowInSeconds = TimeSpan.FromSeconds(rateLimitConfig.TimeWindowInSeconds);

        services.AddRateLimiter(options =>
        {
            //applying global rate limiting policy for all endpoints
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Request.Headers[ApiKeyMiddleware.API_KEY_HEADER_NAME].ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = rateLimitConfig.MaxRequests, //from config
                        QueueLimit = 0,
                        Window = rateLimitTimeWindowInSeconds //from config
                    }));

            options.OnRejected = async (context, cancellationToken) =>
            {
                // Custom rejection handling logic
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.Headers["Retry-After"] = rateLimitTimeWindowInSeconds.TotalSeconds.ToString();

                //for now using basic time conversion - this can be improved
                await context.HttpContext.Response.WriteAsync($"Looks like you've used your quota of {rateLimitConfig.MaxRequests} weather reports {(rateLimitConfig.TimeWindowInSeconds == 60 ? "an hour" : "per" + rateLimitConfig.TimeWindowInSeconds + " seconds")}. Please check again later.", cancellationToken);
            };                    
        });
        

        return services;
    }
}