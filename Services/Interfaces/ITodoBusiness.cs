using TodoApp.Models.Entity;

namespace TodoApp.Services.Interfaces;

public interface ITodoBusiness
{
    Todo Add(string todoItem);
    void Remove(int id);
    Todo Update(int id, string newContent);
    Todo Get(int id);
    IEnumerable<Todo> GetAll();
    bool DoesExist(int id);
    Todo Complete(int id);
}