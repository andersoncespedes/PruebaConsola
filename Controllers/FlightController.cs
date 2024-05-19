using System.Net;
using Newtonsoft.Json;
using PruebaConsole.Entity;
using PruebaConsole.Interface;

namespace PruebaConsole.Controllers;
public class FlightController : BaseController<Flights>
{
    private readonly IUnitOfWork _unitOfWork;
    public FlightController()
    {
        this._unitOfWork = UnitOfWork.UnitOfWork.GetInstance();
    }
    public async Task GetAll(HttpListenerContext listener)
    {
        string json = JsonConvert.SerializeObject(await _unitOfWork.flightRepository.List());
        OutputStream(listener, json, 200);
    }
    public void GetOne(HttpListenerContext context, int id)
    {
        Flights finded = _unitOfWork.flightRepository.GetOne(id);
        if (finded.Origin == null)
        {
            OutputStream(context, "No Se Encontro el Flight", 404);
        }
        else
        {
            OutputStream(context, JsonConvert.SerializeObject(finded), 200);
        }
    }
    public async Task AddOne(HttpListenerContext context)
    {
        Flights flights = await GetBody(context);
        if (flights == null)
        {
            OutputStream(context, "No Se Pudo obtener los datos", 400);
        }
        else
        {
            _unitOfWork.flightRepository.Add(flights);
            string json = JsonConvert.SerializeObject(flights);
            OutputStream(context, json, 201);
        }
    }
    public async Task UpdateOne(HttpListenerContext context, int id)
    {
        Flights flights = _unitOfWork.flightRepository.GetOne(id);
        if (flights == null)
        {
            OutputStream(context, "NOT FOUND", 404);
        }
        else{
            Flights flightsBody = await GetBody(context);
            if(flightsBody == null){
               OutputStream(context, "No Se Pudo obtener los datos", 400);
            }
            else{
                flightsBody.Id = flights.Id;
                _unitOfWork.flightRepository.Update(flightsBody);
                string json = JsonConvert.SerializeObject(flightsBody);
                OutputStream(context, json, 201);
            }
        }
    }
    public void DeleteOne(HttpListenerContext context, int id)
    {
        Flights flights = _unitOfWork.flightRepository.GetOne(id);
        if (flights.Origin == null)
        {
            OutputStream(context, "NOT FOUND", 404);
        }
        else
        {
            _unitOfWork.flightRepository.DeleteOne(id);
            OutputStream(context, "", 202);
        }
    }
    public void GetOneWithoutL(HttpListenerContext context)
    {
        Flights flights = _unitOfWork.flightRepository.GetOneWithoutL();
        if (flights.Origin == null)
        {
            OutputStream(context, "NOT FOUND", 404);
        }
        string json = JsonConvert.SerializeObject(flights);
        OutputStream(context, json, 200);
    }
}