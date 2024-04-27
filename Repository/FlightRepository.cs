
using System.Data.SqlClient;
using PruebaConsole.Configuration;
using PruebaConsole.Entity;
using PruebaConsole.Interface;

namespace PruebaConsole.Repository;
public class FlightRepository : GenericRepository<Flights>, IFlightRepository
{
    public FlightRepository()
    {

    }

    public IEnumerable<Flights> GetWithoutG()
    {
        SqlConnection sqlConnection = this.conexion.StablishConexion();
        string sql = @"SELECT Origin, Destination, Price FROM Flights WHERE CHARINDEX(UPPER('a'), UPPER( Destination)) < 0 ";
        List<Flights> list = new List<Flights>();
        using (SqlCommand command = new SqlCommand(sql, sqlConnection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Flights flight = new();
                    flight.Origin = reader.GetString(0);
                    flight.Destination = reader.GetString(1);
                    flight.Price = reader.GetDouble(2);
                    list.Add(flight);
                }
            }
        }
        this.conexion.DisableConecction(sqlConnection);
        return list;
    }
}