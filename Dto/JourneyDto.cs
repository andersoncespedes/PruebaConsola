

namespace PruebaConsole.Dto;
public class JourneyDto
{
    public int Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public double Price { get; set; }
    public ICollection<FlightDto> Flight {get; set;} = new List<FlightDto>();
}