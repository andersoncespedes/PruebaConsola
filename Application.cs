
using PruebaConsole.Configuration;
using PruebaConsole.Entity;
using PruebaConsole.Interface;
using PruebaConsole.Repository;

namespace PruebaConsole;
public static class Application
{
    private static readonly Conexion _conexion = Conexion.GetInstance();
    public static async void Run()
    {
        IJourneyRepository gen = new JourneyRepository();
        int opcion = 0;
        while (opcion != 5)
        {
            Console.WriteLine("1-5");
            opcion = int.Parse(Console.ReadLine());
            switch (opcion)
            {
                case 1:
                    foreach (Journies journies in await gen.List())
                    {
                        Console.Write(journies.Destination + " ");
                        Console.Write(journies.Price + " ");
                        Console.Write(journies.Id + " ");
                        Console.WriteLine();
                    }
                    break;
                case 2:
                    Console.Write("Inserta la id -> ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        Journies flights = gen.GetOne(id);
                        Console.WriteLine(flights.Destination);
                    }
                    break;
                case 3:
                    Journies jouney = new Journies();
                    Console.Write("Origin -> ");
                    jouney.Origin = Console.ReadLine();
                    Console.Write("Price -> ");
                    if (double.TryParse(Console.ReadLine(), out double price))
                    {
                        jouney.Price = price;
                    }
                    else
                    {
                        jouney.Price = 0;
                    }
                    Console.Write("Destination -> ");
                    jouney.Destination = Console.ReadLine();
                    gen.Add(jouney);
                    break;
                case 4:
                    Console.Write("Inserta la id -> ");
                    if (int.TryParse(Console.ReadLine(), out int idx))
                    {
                        gen.DeleteOne(idx);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
