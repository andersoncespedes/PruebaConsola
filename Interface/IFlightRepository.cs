
using PruebaConsole.Entity;

namespace PruebaConsole.Interface;
public interface IFlightRepository : IGenericRepository<Flights>
{
    IEnumerable<Flights> GetWithoutG();
    Flights GetOneWithoutL();
    
}