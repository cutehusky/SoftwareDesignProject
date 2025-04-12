namespace SoftwareDesignProject.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogTrace("Handling request: {Method} {Url}", context.Request.Method, context.Request.Path);
        
        await _next(context);

        _logger.LogTrace("Finished handling request. Response Status Code: {StatusCode}", context.Response.StatusCode);
    }
}
