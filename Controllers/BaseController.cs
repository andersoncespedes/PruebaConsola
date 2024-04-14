
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
    protected void OutputStream(HttpListenerContext context, string json)
    {
        var response = context.Response;
         byte[] data = Encoding.UTF8.GetBytes(json);
        response.ContentType = "application/json";
        response.ContentLength64 = data.Length;
        response.OutputStream.Write(data,0, data.Length);

    }
}