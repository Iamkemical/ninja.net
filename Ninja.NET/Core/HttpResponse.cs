using System.Text.Json;

namespace Ninja.NET.Core;

public class HttpResponse
{
    public int StatusCode { get; set; }
    public string Content { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();

    public void SetJsonResponse(object obj)
    {
        Content = JsonSerializer.Serialize(obj);
        StatusCode = 200;
        Headers.Add("Content-Type", "application/json");
    }
}