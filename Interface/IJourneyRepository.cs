
using PruebaConsole.Dto;
using PruebaConsole.Entity;

namespace PruebaConsole.Interface;
public interface IJourneyRepository : IGenericRepository<Journies>
{
    Task<HashSet<JourneyDto>> GetWithFlights();
    Task<List<JourneyDto>> GetWithFlightsExcept(string[] flights);
}