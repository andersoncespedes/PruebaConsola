
using PruebaConsole.Entity;

namespace PruebaConsole.Interface;
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<List<T>> List();
    T GetOne(int id);
    void Add(T Entity);
}