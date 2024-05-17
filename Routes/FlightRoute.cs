using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PruebaConsole.Controllers;

namespace PruebaConsole.Routes;
public static partial class FlightRoute
{
    public static async Task SelectRoute(HttpListenerContext context, string requestUrl)
    {
        FlightController flightController = new FlightController();
        Console.WriteLine(requestUrl.Split("/").Last());

        if (context.Request.HttpMethod == "GET" && requestUrl.Split("/").Last() == "Flights")
        {
            await flightController.GetAll(context);
        }
        else if (context.Request.HttpMethod == "GET" && Regex.IsMatch(requestUrl, @"^\/Flights\/\d+$"))
        {
            if (int.TryParse(requestUrl.Split("/").Last(), out int id))
            {
                flightController.GetOne(context, id);
            }
        }
        else if (context.Request.HttpMethod == "POST" && !MyRegex().IsMatch(requestUrl))
        {
            await flightController.AddOne(context);
        }
        else if (context.Request.HttpMethod == "DELETE" && Regex.IsMatch(requestUrl, @"^\/Flights\/\d+$"))
        {
            if (int.TryParse(requestUrl.Split("/").Last(), out int id))
            {
                flightController.DeleteOne(context, id);
            }
        }
        else if(context.Request.HttpMethod == "GET" && requestUrl.Split("/").Last().ToUpper() == "getwithoutl".ToUpper())
        {
            flightController.GetOneWithoutL(context);
        }
    }

    [GeneratedRegex(@"^\/Flights\/\d+$")]
    private static partial Regex MyRegex();
}