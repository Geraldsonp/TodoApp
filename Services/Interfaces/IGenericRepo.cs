using TodoApp.Models.Entity;

namespace TodoApp.Services.Interfaces;

public interface IGenericRepo<T> where T : BaseEntity
{
    void Create(T entity);
    void Delete(T entity);
    void Update(T entity);
    T Get(int id);
    IEnumerable<T> GetAll();
    bool DoesExist(int id);
}