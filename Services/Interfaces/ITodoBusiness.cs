using TodoApp.Models;
using TodoApp.Models.DTOS;
using TodoApp.Models.Entity;

namespace TodoApp.Services.Interfaces;

public interface ITodoBusiness
{
    Result<Todo> Add(CreateTodoDTO todoItem);
    Result<Todo> Remove(int id);
    Result<Todo> Update(int id, string newContent);
    Todo Get(int id);
    IEnumerable<Todo> GetAll();
    bool DoesExist(int id);
    Todo Complete(int id);
}