namespace Ninja.NET.Core;

public class HttpRequest
{
    public string Path { get; set; }
    public string Method { get; set; }
    public string Body { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();
}