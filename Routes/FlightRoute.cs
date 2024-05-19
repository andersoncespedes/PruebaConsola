using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PruebaConsole.Controllers;

namespace PruebaConsole.Routes;
public static partial class FlightRoute
{
    public static async Task SelectRoute(HttpListenerContext context, string requestUrl)
    {
        FlightController flightController = new FlightController();
        Console.WriteLine(context.Request.HttpMethod);
        if(context.Request.HttpMethod == "OPTIONS"){
            byte[] data = Encoding.UTF8.GetBytes("data");
             context.Response.OutputStream.Write(data, 0, "data".Length);
        }
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
        else if (context.Request.HttpMethod == "DELETE" && MyRegex().IsMatch(requestUrl))
        {
            if (int.TryParse(requestUrl.Split("/").Last(), out int id))
            {
                Console.WriteLine(id);
                flightController.DeleteOne(context, id);
            }
        }
        else if(context.Request.HttpMethod == "GET" && requestUrl.Split("/").Last().ToUpper() == "getwithoutl".ToUpper())
        {
            flightController.GetOneWithoutL(context);
        }
        else if(context.Request.HttpMethod == "PUT" && MyRegex().IsMatch(requestUrl))
        {
            if(int.TryParse(requestUrl.Split("/").Last(), out int id)){
                await flightController.UpdateOne(context, id);
            }
        }
    }

    [GeneratedRegex(@"^\/Flights\/\d+$")]
    private static partial Regex MyRegex();

}