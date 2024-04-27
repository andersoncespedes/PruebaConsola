
using System.Data.SqlClient;
using PruebaConsole.Dto;
using PruebaConsole.Entity;
using PruebaConsole.Interface;

namespace PruebaConsole.Repository;
public class JourneyRepository : GenericRepository<Journies>, IJourneyRepository
{
    public JourneyRepository()
    {

    }
    public override Journies GetOne(int id)
    {
        return base.GetOne(id);
    }
    public async Task<HashSet<JourneyDto>> GetWithFlights()
    {
        SqlConnection connection = this.conexion.StablishConexion();
        HashSet<JourneyDto> journies = new HashSet<JourneyDto>();
        string query =
        @"SELECT j.Id, j.Origin, j.Destination, f.Id as FlightId, f.Origin as FlightOrgin, f.Destination as FlightDestination
            FROM Journies j INNER JOIN JourneyFlight jf 
                ON j.Id = jf.IdjourneyFK 
            INNER JOIN Flights f 
                ON f.Id = jf.IdFlightFK";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    FlightDto flight = new();
                    JourneyDto journy = new();
                    flight.Id = reader.GetInt32(3);
                    flight.Destination = reader.GetString(5);
                    flight.Origin = reader.GetString(4);
                    if (journies.Any(e => e.Id == reader.GetInt32(0)))
                    {
                        journies.Where(e => e.Id == reader.GetInt32(0)).First().Flight.Add(flight);
                        continue;
                    }

                    journy.Id = reader.GetInt32(0);
                    journy.Origin = reader.GetString(1);
                    journy.Destination = reader.GetString(2);
                    journy.Flight.Add(flight);
                    journies.Add(journy);

                }
            }
        }
        conexion.DisableConecction(connection);
        return journies;
    }

}