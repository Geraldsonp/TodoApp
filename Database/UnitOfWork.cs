using TodoApp.Models.Entity;
using TodoApp.Services.Interfaces;

namespace TodoApp.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly TodoDbContext _dbContext;


    public UnitOfWork(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}