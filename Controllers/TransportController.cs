
using System.Net;
using Newtonsoft.Json;
using PruebaConsole.Entity;
using PruebaConsole.Interface;

namespace PruebaConsole.Controllers;
public class TransportController : BaseController<Transports>
{
    private readonly IUnitOfWork _unitOfWork;
    public TransportController()
    {
        _unitOfWork = PruebaConsole.UnitOfWork.UnitOfWork.GetInstance();
    }
    public async Task ListAll(HttpListenerContext context)
    {
        var json = JsonConvert.SerializeObject(await _unitOfWork.transportRepository.List());
        OutputStream(context, json, 200);

    }
}