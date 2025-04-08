using Ninja.NET.Core;

namespace Ninja.NET.Extensions;

public class JsonResponseMiddleware : Middleware
{
    public override async Task Invoke(HttpRequest request, HttpResponse response, RequestDelegate next)
    {
        if (response.Content != null && !response.Headers.ContainsKey("Content-Type"))
        {
            response.SetJsonResponse(new { message = "Custom respone", data = response.Content });
        }

        await next(request, response);
    }
}