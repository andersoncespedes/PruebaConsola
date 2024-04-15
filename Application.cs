using System.Net;
using System.Text.RegularExpressions;
using PruebaConsole.Configuration;
using PruebaConsole.Controllers;

namespace PruebaConsole;

public static class Application
{
    private static readonly Conexion _conexion = Conexion.GetInstance();
    private static async Task MapControllers(HttpListener httpListener)
    {
        JourneyController journeyController = new JourneyController();
        HttpListenerContext context = httpListener.GetContext();
        HandleRequest(context);
        var requestUrl = context.Request.Url.AbsolutePath;
        if (requestUrl.StartsWith("/Journey"))
        {
            if (context.Request.HttpMethod == "GET" && !Regex.IsMatch(requestUrl, @"^\/Journey\/\d+$"))
            {
                await journeyController.ListAll(context);
            }
            else if (context.Request.HttpMethod == "GET" && Regex.IsMatch(requestUrl, @"^\/Journey\/\d+$"))
            {
                if (int.TryParse(requestUrl.Split("/").Last(), out int index))
                {
                    journeyController.GetOne(context, index);
                }

            }
            else if (context.Request.HttpMethod == "POST" && !Regex.IsMatch(requestUrl, @"^\/Journey\/\d+$"))
            {
                await journeyController.AddOne(context);
            }
            else if(context.Request.HttpMethod == "DELETE"){
                if(int.TryParse(requestUrl.Split("/").Last(), out int index)){
                     journeyController.DeleteOne(context, index);
                }
            }
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
    public static async Task Run()
    {
        HttpListener httpListener = new HttpListener();
        httpListener.Prefixes.Add("http://localhost:3002/");
        Console.WriteLine("Listening in the port 3002");
        httpListener.Start();
        while (true)
        {
            await MapControllers(httpListener);
        }
    }
}
