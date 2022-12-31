using Microsoft.EntityFrameworkCore;
using TodoApp.Models.Entity;
using TodoApp.Services.Interfaces;

namespace TodoApp.Database;

public class GenericRepo<T> : IGenericRepo<T> where T : BaseEntity
{
    //Test
    private readonly TodoDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepo(TodoDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public void Create(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public T Get(int id)
    {
        return _dbSet.Find(id);
    }

    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public bool DoesExist(int id)
    {
        return _dbSet.Any(x => x.Id == id);
    }
}