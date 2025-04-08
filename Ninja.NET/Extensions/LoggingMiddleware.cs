using Ninja.NET.Core;

namespace Ninja.NET.Extensions;

public class LoggingMiddleware : Middleware
{
    public override async Task Invoke(HttpRequest request, HttpResponse response, RequestDelegate next)
    {
        Console.WriteLine($"Request: {request.Method} {request.Path}");
        await next(request, response);
        Console.WriteLine($"Response: {response.Content}");
    }
}