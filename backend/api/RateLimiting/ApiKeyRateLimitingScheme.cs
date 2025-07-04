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
                        Window = TimeSpan.FromSeconds(rateLimitConfig.TimeWindowInSeconds) //from config
                    }));
        });

        return services;
    }
}