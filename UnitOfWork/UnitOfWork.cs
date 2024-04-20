using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PruebaConsole.Interface;
using PruebaConsole.Repository;

namespace PruebaConsole.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private static readonly object _lock = new object();
    private static IUnitOfWork _unitOfWork;
    private IJourneyRepository _journey;
    private IFlightRepository _flight;
    public UnitOfWork(){}
    public static IUnitOfWork GetInstance(){
        lock (_lock){
            _unitOfWork ??= new UnitOfWork();
        }
        return _unitOfWork;
    }
    public IJourneyRepository journeyRepository {
        get{
            _journey ??= new JourneyRepository();
            return _journey;
        }
    }

    public IFlightRepository flightRepository {
        get{
            _flight ??= new FlightRepository();
            return _flight;
        }
    }
}