
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
        string sql = @"SELECT Origin, Destination, Price FROM Flights WHERE CHARINDEX(UPPER('a'), UPPER(Destination)) = 0";
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
    public Flights GetOneWithoutL()
    {
        SqlConnection connection = conexion.StablishConexion();
        string sql = "SELECT TOP 1 Origin, Destination, CAST(Price AS FLOAT) AS Price FROM Flights WHERE CHARINDEX(UPPER('L'), UPPER(Destination)) = 0 ";
        Flights flights = new();
        using (SqlCommand command = new SqlCommand(sql, connection)){
            using(SqlDataReader reader = command.ExecuteReader()){
                while (reader.Read()){
                    flights.Origin = reader.GetString(0);
                    flights.Destination = reader.GetString(1);
                    flights.Price = reader.GetDouble(2);
                    
                }
            }
        }
        return flights;
    }

    public double GetAmount()
    {
        SqlConnection connection = conexion.StablishConexion();
        string query = "SELECT SUM(Price) as precioTotal FROM Flights";
        double price;
        using(SqlCommand command = new SqlCommand(query,connection)){
            using(SqlDataReader reader = command.ExecuteReader()){
                while(reader.Read())
                {
                    price = reader.GetDouble(0);
                    return price;
                } 
                
            }
        }
        return 0;
       
    }
}