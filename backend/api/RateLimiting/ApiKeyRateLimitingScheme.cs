using System.Threading.RateLimiting;

public static class ApiKeyRateLimitingScheme
{
    public static IServiceCollection AddRateLimitingSchemeByApiKey(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            //applying global rate limiting policy for all endpoints
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Request.Headers[ApiKeyMiddleware.API_KEY_HEADER_NAME].ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 5,
                        QueueLimit = 0,
                        Window = TimeSpan.FromSeconds(10)
                    }));
        });

        return services;
    }
}