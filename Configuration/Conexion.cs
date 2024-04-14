
using System.Data.SqlClient;

namespace PruebaConsole.Configuration;
public class Conexion
{
    private readonly static object _lock = new object();
    private static Conexion _conexion ;
    private readonly string Server = "Anderson-pc\\ANDERSON";
    private readonly string Database = "ColombiaTravel";
    private readonly string User = "Anderson\\sa";
    private readonly string Password = "123456";
    private readonly string TrustConection = "True";
    private readonly string ConexionString = "Server=Anderson-pc\\ANDERSON;Database=ColombiaTravel;User Id=Anderson\\sa;Password=123456;Trusted_Connection=True;";
    public static Conexion GetInstance(){
        lock(_lock){
            if(_conexion == null){
                _conexion = new Conexion();
            }
        }
        return _conexion;
    }
    private Conexion()
    {   
        ConexionString = $"Server={Server};Database={Database};User Id={User};Password={Password};Trusted_Connection={TrustConection};";
    }
    public SqlConnection StablishConexion()
    {
        try
        {
            SqlConnection conexion = new SqlConnection(ConexionString);
            conexion.Open();
            return conexion;
        }catch(Exception err){
            Console.WriteLine($"error {err.Message}");
            return null;
        }
    }
    public void DisableConecction(SqlConnection sql){
         sql.Close();
    }
}