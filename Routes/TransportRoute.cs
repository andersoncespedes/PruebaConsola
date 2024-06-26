using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using PruebaConsole.Controllers;

namespace PruebaConsole.Routes;
public static class TransportRoute
{
    public static async Task SelectRoute(HttpListenerContext context, string requestUrl)
    {
        TransportController transportController = new TransportController();
        
        if (context.Request.HttpMethod == "GET" && requestUrl.Split("/").Last() == "transports" || requestUrl.Split("/").Last() == string.Empty && context.Request.HttpMethod == "GET")
        {
            Console.WriteLine(requestUrl);
            await transportController.ListAll(context);
        }

    }
}