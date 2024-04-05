

namespace PruebaConsole.Entity;
public class Journies : BaseEntity
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public double Price { get; set; }
}