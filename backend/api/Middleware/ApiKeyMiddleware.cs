public class ApiKeyMiddleware
{
    public static string API_KEY_HEADER_NAME = "X-API-KEY";
    private readonly RequestDelegate next;
    private readonly IConfiguration configuration;
    private readonly ILogger<ApiKeyMiddleware> logger;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ApiKeyMiddleware> logger)
    {
        this.next = next;
        this.configuration = configuration;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        //get list of valid api keys from configuration
        var apiKeys = configuration.GetValue<string>("ApplicationConfiguration:ApiKeys") ?? throw new ApplicationException("Api keys for this application haven't been configured. Please provide a comma separated list of keys, with each key being 6 characters long. Example: 8ja346b,533ab8c");        
        string[] apiKeyList = apiKeys.Split(",");

        //enforce 6 character length policy
        foreach (var key in apiKeyList)
        {
            if (key.Length != 7) throw new ApplicationException("Please provide a comma separated list of keys, with each key being 6 characters long. Example: 8ja346b,533ab8c");
        }

        //check for a valid api key in the header
            if (!context.Request.Headers.ContainsKey(API_KEY_HEADER_NAME) ||
                !apiKeyList.Contains(context.Request.Headers[API_KEY_HEADER_NAME].ToString()))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Please supply a valid API key.");
                return; //stop further processing of middleware/request
            }

        await next.Invoke(context); //continue pipeline processing
    }
}