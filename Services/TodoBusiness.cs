using TodoApp.Database;
using TodoApp.Models.DTOS;
using TodoApp.Models.Entity;
using TodoApp.Services.Interfaces;

namespace TodoApp.Services;

public class TodoBusiness : ITodoBusiness
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdProvider _userIdProvider;

    public TodoBusiness(IUnitOfWork unitOfWork, IUserIdProvider userIdProvider)
    {
        _unitOfWork = unitOfWork;
        _userIdProvider = userIdProvider;
    }
    public Todo Add(CreateTodoDTO createTodoDto)
    {
        var todo = new Todo()
        {
            Content = createTodoDto.Content,
            TodoListId = createTodoDto.ListId,
            OwnerId = _userIdProvider.GetCurrentUserId()
        };
        _unitOfWork.TodoRepo.Create(todo);
        _unitOfWork.SaveChanges();

        return todo;
    }

    public void Remove(int id)
    {
        var todo = _unitOfWork.TodoRepo.Get(id);

        _unitOfWork.TodoRepo.Delete(todo);

        _unitOfWork.SaveChanges();
    }

    public Todo Update(int id, string newContent)
    {
        var todo = _unitOfWork.TodoRepo.Get(id);
        todo.Content = newContent;
        _unitOfWork.SaveChanges();
        return todo;
    }

    public Todo Get(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Todo> GetAll()
    {
        return _unitOfWork.TodoRepo.GetAll();
    }

    public bool DoesExist(int id)
    {
        return _unitOfWork.TodoRepo.DoesExist(id);
    }

    public Todo Complete(int id)
    {

        var todo = _unitOfWork.TodoRepo.Get(id);

        todo.IsCompleted = !todo.IsCompleted;

        _unitOfWork.TodoRepo.Update(todo);

        _unitOfWork.SaveChanges();

        return todo;
    }
}