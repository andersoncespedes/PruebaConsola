using Newtonsoft.Json;
using System.Net;

using PruebaConsole.Interface;
using PruebaConsole.Repository;
using System.Text;
using PruebaConsole.Entity;

namespace PruebaConsole.Controllers;
public class JourneyController : BaseController<Journies>
{
    private readonly IJourneyRepository _journeyRepository;
    public JourneyController()
    {
        _journeyRepository = new JourneyRepository();
    }
    public async Task ListAll(HttpListenerContext context)
    {
        var response = context.Response;
        var json = JsonConvert.SerializeObject(await _journeyRepository.List());
        OutputStream(context, json);

    }
    public async Task GetOne(HttpListenerContext context, int id)
    {
        var response = context.Response;
        Journies journies = _journeyRepository.GetOne(id);
        if (journies.Origin == null)
        {
            throw new Exception("Not Found");
        }
        string json = JsonConvert.SerializeObject(_journeyRepository.GetOne(id));
        OutputStream(context, json);
    }
    public async Task AddOne(HttpListenerContext context)
    {
        var response = context.Response;
        Journies journies = await GetBody(context);
        if (journies == null)
        {
            throw new Exception($"Could not find {nameof(journies)}");
        }
        _journeyRepository.Add(journies);
        var json = JsonConvert.SerializeObject(journies);
        OutputStream(context, json);

    }
}
