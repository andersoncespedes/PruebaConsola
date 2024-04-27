using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using PruebaConsole.Configuration;
using PruebaConsole.Controllers;
using PruebaConsole.Routes;

namespace PruebaConsole;

public static class Application
{
    private static readonly Conexion _conexion = Conexion.GetInstance();
    private static readonly RateLimiter rateLimiter = new RateLimiter();
    private static async void MapControllers(HttpListener httpListener)
    {
        HttpListenerContext context = httpListener.GetContext();
        HandleRequest(context);
        string ipEntry = context.Request.RemoteEndPoint.Address.ToString();
        string requestUrl = context.Request.Url.AbsolutePath;
        var responsable = context.Response;
        Timer timer = new(state =>
        {
            responsable.Abort();
        }, null, 3000, Timeout.Infinite);
        if (rateLimiter.AllowRequest(ipEntry))
        {
            if (requestUrl.StartsWith("/Journey"))
            {
                await JourneyRoute.SelectRoute(context, requestUrl);
            }
            else if (requestUrl.StartsWith("/Flights"))
            {
                await FlightRoute.SelectRoute(context, requestUrl);
            }
            else
            {
                HttpListenerResponse response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = 404;
                byte[] data = Encoding.UTF8.GetBytes("NOT FOUND");
                response.ContentLength64 = data.Length;
                response.OutputStream.Write(data, 0, data.Length);
            }
        }
        else
        {
            HttpListenerResponse response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = 429;
            byte[] data = Encoding.UTF8.GetBytes("TOO MANY REQUEST");
            response.ContentLength64 = data.Length;
            response.OutputStream.Write(data, 0, data.Length);
        }

    }
    public static void HandleRequest(HttpListenerContext context)
    {
        // Permitir solicitudes desde cualquier origen
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        // Permitir los métodos GET, POST, PUT y DELETE
        context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
        // Permitir los encabezados Content-Type y Authorization
        context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Authorization");

        // Tu lógica de manejo de solicitud aquí
    }
    public static void Run()
    {
        HttpListener httpListener = new HttpListener();
        httpListener.Prefixes.Add("http://localhost:3002/");
        Console.WriteLine("Listening in the port 3002");
        httpListener.Start();
        while (true)
        {
            MapControllers(httpListener);
        }
    }
}
