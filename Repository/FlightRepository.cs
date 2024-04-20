
using PruebaConsole.Entity;
using PruebaConsole.Interface;

namespace PruebaConsole.Repository;
public class FlightRepository : GenericRepository<Flights>, IFlightRepository
{
    public FlightRepository(){
        
    }
}