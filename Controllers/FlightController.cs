using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PruebaConsole.Entity;
using PruebaConsole.Interface;
using PruebaConsole.UnitOfWork;

namespace PruebaConsole.Controllers;
public class FlightController : BaseController<Flights>
{
    private readonly IUnitOfWork _unitOfWork;
    public FlightController(){
        this._unitOfWork = UnitOfWork.UnitOfWork.GetInstance();
    }
    public async Task GetAll(HttpListenerContext listener)
    {
        string json = JsonConvert.SerializeObject(await _unitOfWork.flightRepository.List());
        OutputStream(listener, json, 200);
    }
    public  void GetOne(HttpListenerContext context, int id ){
        Flights finded =  _unitOfWork.flightRepository.GetOne(id);
        if(finded.Origin == null){
            OutputStream(context, "No Se Encontro el Flight", 404);
        }
        else{
            OutputStream(context,JsonConvert.SerializeObject(  finded), 200);
        }
    }
}