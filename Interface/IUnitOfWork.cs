using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaConsole.Interface;
public interface IUnitOfWork
{
    
    public IJourneyRepository journeyRepository { get; }
    public IFlightRepository flightRepository { get; }
    static IUnitOfWork GetInstance() => null;
}