
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using PruebaConsole.Controllers;

namespace PruebaConsole.Routes;
public static class JourneyRoute
{
    public static async Task SelectRoute(HttpListenerContext context, string requestUrl)
    {
         JourneyController journeyController = new JourneyController();
        if (context.Request.HttpMethod == "GET" && requestUrl.Split("/").Last() == "Journey" || requestUrl.Split("/").Last() == string.Empty)
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
        else if (context.Request.HttpMethod == "DELETE")
        {
            if (int.TryParse(requestUrl.Split("/").Last(), out int index))
            {
                journeyController.DeleteOne(context, index);
            }
        }
        else if (context.Request.HttpMethod == "GET" && requestUrl.Split("/").Last() == "flight")
        {
            journeyController.GetOnlyOrigin(context);
        }
        else if(context.Request.HttpMethod == "GET" && requestUrl.Split("/").Last().ToUpper() == "COUNT" )
        {
            journeyController.GetCounts(context);

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
}
