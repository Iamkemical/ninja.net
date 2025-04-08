using System.Net;
using System.Text;

namespace Ninja.NET.Core;

public class WebHost
{
    private readonly Router _router;
    private readonly List<Middleware> _middlewares = new();

    public WebHost(Router router)
    {
        _router = router;
    }

    public void Use(Middleware middleware)
    {
        _middlewares.Add(middleware);
    }

    public async Task StartAsync(string url)
    {
        var listener = new HttpListener();
        listener.Prefixes.Add(url);
        listener.Start();
        Console.WriteLine("Server started at " + url);

        while (true)
        {
            var context = await listener.GetContextAsync();
            var request = new HttpRequest
            {
                Path = context.Request.Url.AbsolutePath,
                Method = context.Request.HttpMethod,
                Headers = context.Request.Headers.Cast<string>().ToDictionary(k => k, v => context.Request.Headers[v]),
                Body = new StreamReader(context.Request.InputStream).ReadToEnd()
            };

            var response = new HttpResponse();
            
            var handler = _router.GetRoutes(request.Method, request.Path);

            if (handler != null)
            {
                int index = 0;

                async Task next(HttpRequest req, HttpResponse res)
                {
                    if (index < _middlewares.Count)
                    {
                        var middleware = _middlewares[index];
                        index++;
                        await middleware.Invoke(req, res, next);
                    }
                    else
                    {
                        await handler.Invoke(req, res);
                    }
                }
                await next(request, response);
            }

            if (response.StatusCode == 0)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                context.Response.StatusCode = response.StatusCode;
                foreach (var header in response.Headers)
                {
                    context.Response.AddHeader(header.Key, header.Value);
                }
                
                var responseBytes = Encoding.UTF8.GetBytes(response.Content);
                context.Response.ContentLength64 = responseBytes.Length;
                await context.Response.OutputStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }
            context.Response.OutputStream.Close();
        }
    }
}