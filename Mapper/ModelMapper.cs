using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaConsole.Mapper;
public class ModelMapper<T> where T : class
{
    private List<T> _model = new List<T>();
    private string _name;
    public ModelMapper(string entity)
    {
        _name = entity;
    }
    public List<T> SetMapper (SqlDataReader data)
    {
        throw new NotImplementedException();
    }

}
