namespace Ninja.NET.Core;

public delegate Task RequestDelegate(HttpRequest request, HttpResponse response);
public abstract class Middleware
{
    public abstract Task Invoke(HttpRequest request, HttpResponse response, RequestDelegate next);
}