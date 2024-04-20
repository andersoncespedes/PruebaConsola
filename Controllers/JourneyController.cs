using Newtonsoft.Json;
using System.Net;

using PruebaConsole.Interface;
using PruebaConsole.Entity;
namespace PruebaConsole.Controllers;
public class JourneyController : BaseController<Journies>
{
    private readonly IUnitOfWork _unitOfWork;
    public JourneyController()
    {
        _unitOfWork = PruebaConsole.UnitOfWork.UnitOfWork.GetInstance();
    }
    public async Task ListAll(HttpListenerContext context)
    {
        var json = JsonConvert.SerializeObject(await _unitOfWork.journeyRepository.List());
        OutputStream(context, json, 200);

    }
    public void GetOne(HttpListenerContext context, int id)
    {
        Journies journies = _unitOfWork.journeyRepository.GetOne(id);
        if (journies.Origin == null)
        {
            OutputStream(context, "", 500);
        }
        else
        {
            string json = JsonConvert.SerializeObject(_unitOfWork.journeyRepository.GetOne(id));
            OutputStream(context, json, 200);
        }

    }
    public async Task AddOne(HttpListenerContext context)
    {
        Journies journies = await GetBody(context);
        if (journies.Origin == null)
        {
            OutputStream(context, "", 404);
        }
        else
        {
            _unitOfWork.journeyRepository.Add(journies);
            var json = JsonConvert.SerializeObject(journies);
            OutputStream(context, json, 201);
        }

    }
    public void DeleteOne(HttpListenerContext context, int index)
    {
        Journies jounerney = _unitOfWork.journeyRepository.GetOne(index);
        Console.WriteLine(jounerney);
        if (jounerney.Destination == null) OutputStream(context, "", 404);
        else
        {
            _unitOfWork.journeyRepository.DeleteOne(index);
            string json = JsonConvert.SerializeObject(jounerney);
            OutputStream(context, json, 204);
        }

    }
}
