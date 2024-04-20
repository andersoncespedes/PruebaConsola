
using System.Net;
using System.Text;
using Newtonsoft.Json;
using PruebaConsole.Entity;

namespace PruebaConsole.Controllers;
public class BaseController<T> where T : BaseEntity
{
    public BaseController()
    {

    }
    protected async Task<T> GetBody(HttpListenerContext context)
    {
        string RequestBody;
        using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
        {
            RequestBody = await reader.ReadToEndAsync();
        }
        T Entity = JsonConvert.DeserializeObject<T>(RequestBody);
        return Entity;
    }
    protected void OutputStream(HttpListenerContext context, string json, int status)
    {
        var response = context.Response;
        byte[] data;
        switch (status)
        {
            case 204:
                response.ContentLength64 = 0;
                data = new byte[0];
                break;
            case 400:
                data = Encoding.UTF8.GetBytes("BAD REQUEST");
                response.ContentLength64 = data.Length;
                break;
            case 404:
                data = Encoding.UTF8.GetBytes("NOT FOUND");
                response.ContentLength64 = data.Length;
                break;
            case 500:
                data = Encoding.UTF8.GetBytes("INTERNAL SERVER ERROR");
                response.ContentLength64 = data.Length;
                break;

            default:
                data = Encoding.UTF8.GetBytes(json);
                response.ContentLength64 = data.Length;
                break;
        }
        response.ContentType = "application/json";
        response.StatusCode = status;
        response.OutputStream.Write(data, 0, data.Length);

    }
}