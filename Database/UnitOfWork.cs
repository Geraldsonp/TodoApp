using TodoApp.Models.Entity;
using TodoApp.Services.Interfaces;

namespace TodoApp.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly TodoDbContext _dbContext;
    private readonly IGenericRepo<Todo>? _todoRepo;
    private IGenericRepo<TodoList>? _todoListRepo;

    public UnitOfWork(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IGenericRepo<Todo> TodoRepo => _todoRepo ?? new GenericRepo<Todo>(_dbContext);

    public IGenericRepo<TodoList> TodoListRepo => _todoListRepo ?? new GenericRepo<TodoList>(_dbContext);

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}