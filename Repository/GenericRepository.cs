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
    public GenericRepository()
    {
        _entity = Activator.CreateInstance<T>();
    }
    public virtual async Task<List<T>> List()
    {
        SqlConnection a = conexion.StablishConexion();
        List<T> values = new();
        Type type = _entity.GetType();
        PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        SqlCommand command = new SqlCommand("SELECT " + String.Join(", ", propertyInfos.Select(e => e.Name)) + " FROM " + type.Name, a);

        foreach (PropertyInfo property in propertyInfos)
        {
            command.Parameters.AddWithValue(property.Name, property.Name);
        }
        using (SqlDataReader read = command.ExecuteReader())
        {
            while (read.Read())
            {
                T Instance = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in propertyInfos)
                {

                    property.SetValue(Instance, read[property.Name]);
                }
                values.Add(Instance);
            }
        }
        await conexion.DisableConecction(a);
        return values;
    }
    public virtual T GetOne(int id)
    {
        SqlConnection connection = conexion.StablishConexion();
        Type type = _entity.GetType();
        PropertyInfo[] property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        T data = Activator.CreateInstance<T>();
        using (SqlCommand command = new("SELECT " + String.Join(", ", property.Select(e => e.Name)) +
        " FROM " + type.Name + " WHERE Id = '" + id + "'", connection))
        {
            foreach (PropertyInfo propert in property)
            {
                command.Parameters.AddWithValue(propert.Name, propert.Name);
            }
            using SqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                foreach (PropertyInfo property1 in property)
                {
                    if (read[property1.Name] != DBNull.Value)
                    {
                        property1.SetValue(data, read[property1.Name]);
                    }

                }
            }
        }
        return data;
    }
    public virtual void Add(T Entity)
    {
        SqlConnection sqlConnection = conexion.StablishConexion();
        Type entityType = Entity.GetType();
        PropertyInfo[] properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        List<string> nameProp = properties.Where(e => e.Name != "Id").Select(e => e.Name).ToList();
        using (SqlCommand command = new SqlCommand($"INSERT INTO {entityType.Name}({String.Join(",", nameProp)}) VALUES ({String.Join(",", nameProp.Select(e => $"@{e}"))})", sqlConnection))
        {
            foreach (PropertyInfo prop in properties)
            {
                if (prop.Name != "Id")
                {
                    command.Parameters.AddWithValue("@"+prop.Name, prop.GetValue(Entity));
                    Console.WriteLine(String.Join(",",nameProp.Select(e => $"@{e}")));
                }
            }
            command.ExecuteNonQuery();
        }

    }
}
