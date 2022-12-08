using TodoApp.Models;
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
    public Result<Todo> Add(CreateTodoDTO createTodoDto)
    {
        if (!_unitOfWork.TodoListRepo.DoesExist(createTodoDto.ListId))
        {
            return new Result<Todo>(ErrorTypes.EntityNotFound);
        }

        var list = _unitOfWork.TodoListRepo.Get(createTodoDto.ListId);

        if (list.Todos.Count >= 12)
        {
            return new Result<Todo>(ErrorTypes.ListDoesNotHasSpace);
        }

        var todo = new Todo()
        {
            Content = createTodoDto.Content,
            TodoListId = createTodoDto.ListId,
            OwnerId = _userIdProvider.GetCurrentUserId()
        };

        _unitOfWork.TodoRepo.Create(todo);
        _unitOfWork.SaveChanges();

        return new Result<Todo>(todo);
    }

    public Result<Todo> Remove(int id)
    {
        var doesExist = _unitOfWork.TodoRepo.DoesExist(id);

        if (doesExist)
        {
            var todo = _unitOfWork.TodoRepo.Get(id);

            _unitOfWork.TodoRepo.Delete(todo);

            _unitOfWork.SaveChanges();

            return new Result<Todo>(true);
        }

        return new Result<Todo>(ErrorTypes.EntityNotFound);
    }

    public Result<Todo> Update(int id, string newContent)
    {
        var doesExist = _unitOfWork.TodoRepo.DoesExist(id);

        if (doesExist)
        {
            var todo = _unitOfWork.TodoRepo.Get(id);
            todo.Content = newContent;
            _unitOfWork.SaveChanges();

            return new Result<Todo>(todo);
        }


        return new Result<Todo>(ErrorTypes.EntityNotFound);
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