using Ninja.NET.Core;
using Ninja.NET.Extensions;

class Program
{
    static async Task Main(string[] args)
    {
        var router = new Router();
        var webHost = new WebHost(router);
        
        //Define routes
        router.Map("GET", "/api", async (req, res) =>
        {
            var response = new { message = "Hello, and welcome to Ninja Web Framework", status = "success" };
            res.SetJsonResponse(response);
        });
        
        //Add Middlewares
        webHost.Use(new LoggingMiddleware());
        webHost.Use(new JsonResponseMiddleware());
        
        //Start the Server
        await webHost.StartAsync("http://localhost:5000/");
    }
}