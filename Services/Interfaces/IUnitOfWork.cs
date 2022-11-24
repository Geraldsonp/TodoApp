using TodoApp.Models.Entity;

namespace TodoApp.Services.Interfaces;

public interface IUnitOfWork
{
    void SaveChanges();
    public IGenericRepo<Todo>? TodoRepo { get; }
    public IGenericRepo<TodoList> TodoListRepo { get; }
}