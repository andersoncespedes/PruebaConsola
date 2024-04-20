
using PruebaConsole.Entity;
using PruebaConsole.Interface;

namespace PruebaConsole.Repository;
public class JourneyRepository : GenericRepository<Journies>, IJourneyRepository
{
    public JourneyRepository(){

    }
    public override Journies GetOne(int id)
    {
        Console.WriteLine("Epales panase");
        return base.GetOne(id);
    }
}