using Microsoft.AspNetCore.Mvc;
using TodoApp.Models.DTOS;
using TodoApp.Models.Entity;

namespace TodoApp.Services.Interfaces;

public interface ITodoListBusiness
{
    TodoListDto GetTodosList(int listId);
    void DeleteList(int listId);
    TodoList RenameList(int listId, string newName);
    bool DoesExist(int listId);
    IEnumerable<TodoListDto> GetTodosList();
    bool HasSpace(int listId);
}