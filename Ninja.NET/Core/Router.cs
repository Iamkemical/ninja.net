namespace Ninja.NET.Core;

public class Router
{
    private readonly Dictionary<string, Func<HttpRequest, HttpResponse, Task>> _routes = new();

    public void Map(string method, string path, Func<HttpRequest, HttpResponse, Task> handler)
    {
        var key = $"{method.ToUpper()} {path}";
        _routes.Add(key, handler);
    }

    public Func<HttpRequest, HttpResponse, Task> GetRoutes(string method, string path)
    {
        var key = $"{method.ToUpper()} {path}";
        return _routes.ContainsKey(key) ? _routes[key] : null;
    }
}