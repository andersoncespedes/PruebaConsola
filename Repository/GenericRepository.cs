using System.Data.SqlClient;
using System.Reflection;
using PruebaConsole.Configuration;
using PruebaConsole.Entity;
using PruebaConsole.Interface;

namespace PruebaConsole.Repository;
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    Conexion conexion = Conexion.GetInstance();
    private T _entity;
    private Type _type;
    private PropertyInfo[] _propertyInfos;
    public GenericRepository()
    {
        _entity = Activator.CreateInstance<T>();
        _type = _entity.GetType();
        _propertyInfos = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }
    public virtual async Task<List<T>> List()
    {
        SqlConnection a = conexion.StablishConexion();
        List<T> values = new();
        SqlCommand command = new SqlCommand("SELECT " + String.Join(", ", _propertyInfos.Select(e => e.Name)) + " FROM " + _type.Name, a);

        foreach (PropertyInfo property in _propertyInfos)
        {
            command.Parameters.AddWithValue(property.Name, property.Name);
        }
        using (SqlDataReader read = command.ExecuteReader())
        {
            while (read.Read())
            {
                T Instance = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in _propertyInfos)
                {

                    property.SetValue(Instance, read[property.Name]);
                }
                values.Add(Instance);
            }
        }
        conexion.DisableConecction(a);
        return values;
    }
    public virtual T GetOne(int id)
    {
        SqlConnection connection = conexion.StablishConexion();
        T data = Activator.CreateInstance<T>();
        using (SqlCommand command = new("SELECT " + String.Join(", ", _propertyInfos.Select(e => e.Name)) +
        " FROM " + _type.Name + " WHERE Id = '" + id + "'", connection))
        {
            foreach (PropertyInfo propert in _propertyInfos)
            {
                command.Parameters.AddWithValue(propert.Name, propert.Name);
            }
            using SqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                foreach (PropertyInfo property1 in _propertyInfos)
                {
                    if (read[property1.Name] != DBNull.Value)
                    {
                        property1.SetValue(data, read[property1.Name]);
                    }

                }
            }
        }
        conexion.DisableConecction(connection);

        return data;
    }
    public virtual void Add(T Entity)
    {
        SqlConnection sqlConnection = conexion.StablishConexion();
        List<string> nameProp = _propertyInfos.Where(e => e.Name != "Id").Select(e => e.Name).ToList();
        using (SqlCommand command = new SqlCommand($"INSERT INTO {_type.Name}({String.Join(",", nameProp)}) VALUES ({String.Join(",", nameProp.Select(e => $"@{e}"))})", sqlConnection))
        {
            foreach (PropertyInfo prop in _propertyInfos)
            {
                if (prop.Name != "Id")
                {
                    command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(Entity));
                    Console.WriteLine(String.Join(",", nameProp.Select(e => $"@{e}")));
                }
            }
            command.ExecuteNonQuery();
        }
        conexion.DisableConecction(sqlConnection);
    }
    public virtual void DeleteOne(int id)
    {
        T data = GetOne(id);
        if (data != null)
        {
            SqlConnection sqlConnection = conexion.StablishConexion();
            string Consulta = $"DELETE FROM {_type.Name} WHERE id = {id}";
            using (SqlCommand command = new SqlCommand(Consulta, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
            conexion.DisableConecction(sqlConnection);

        }
    }

    public void Update(T Entity)
    {
        SqlConnection con = conexion.StablishConexion();
        string query = $"UPDATE {_type.Name} SET {String.Join(",", _propertyInfos.Where(e => e.Name != "Id").Select(e => $"{e.Name} = @{e.Name}"))} WHERE Id = {Entity.Id}";
        Console.WriteLine(query);
        using (SqlCommand command = new SqlCommand(query, con))
        {
            foreach (PropertyInfo prop in _propertyInfos)
            {
                if (prop.Name != "Id")
                {
                    command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(Entity));
                }
            }
            command.ExecuteNonQuery();
        }
            conexion.DisableConecction(con);

    }
}
