using TodoApp.Database;
using TodoApp.Models.DTOS;
using TodoApp.Models.Entity;

namespace TodoApp.Services.Interfaces;

public class TodoListBusiness : ITodoListBusiness
{
    private readonly IUnitOfWork _unitOfWork;

    public TodoListBusiness(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public TodoListDto GetTodosList(int listId)
    {
        var list = _unitOfWork.TodoListRepo.Get(listId);

        return new TodoListDto()
        {
            Id = list.Id,
            ListName = list.ListName,
            Todos = list.Todos
        };
    }

    public void DeleteList(int listId)
    {
        var list = _unitOfWork.TodoListRepo.Get(listId);
        _unitOfWork.TodoListRepo.Delete(list);
        _unitOfWork.SaveChanges();
    }

    public TodoList RenameList(int listId, string newName)
    {
        var list = _unitOfWork.TodoListRepo.Get(listId);
        list.ListName = newName;
        _unitOfWork.SaveChanges();
        return list;
    }

    public bool DoesExist(int listId)
    {
        return _unitOfWork.TodoListRepo.DoesExist(listId);
    }

    public IEnumerable<TodoListDto> GetTodosList()
    {
        var lists = _unitOfWork.TodoListRepo.GetAll()
            .SelectMany<TodoList, TodoListDto>(x =>
            {
                return new[]
                {
                    new TodoListDto()
                    {
                        Id = x.Id,
                        ListName = x.ListName,
                        Todos = x.Todos
                    }
                };
            });
        return lists;
    }
}