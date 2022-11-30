using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Models.DTOS;
using TodoApp.Models.ViewModels.TodoViewModels;
using TodoApp.Services.Interfaces;

namespace TodoApp.Controllers;

[Authorize]
public class TodoController : Controller
{
    private readonly ITodoBusiness _todoBusiness;
    private readonly ITodoListBusiness _todoListBusiness;

    public TodoController(ITodoBusiness todoBusiness, ITodoListBusiness todoListBusiness)
    {
        _todoBusiness = todoBusiness;
        _todoListBusiness = todoListBusiness;
    }

    // GET
    public IActionResult Index()
    {
        IndexViewModel viewModel = new IndexViewModel()
        {
            Todos = _todoBusiness.GetAll()
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateTodoDTO createTodoDto)
    {
        if (!_todoListBusiness.DoesExist(createTodoDto.ListId))
        {
            return BadRequest("List with specified id does Not Exist");
        }

        if (!_todoListBusiness.HasSpace(createTodoDto.ListId))
        {
            return BadRequest("List has too many items please remove some or create a new list");
        }

        _todoListBusiness.DoesExist(createTodoDto.ListId);
        var todoItem = _todoBusiness.Add(createTodoDto);
        return Ok(todoItem);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
         _todoBusiness.Remove(id);
        return Ok($"Todo item deleted successfully");
    }
    
    [HttpPost]
    public async Task<IActionResult> Complete(int id)
    {
        var doesExits = _todoBusiness.DoesExist(id);

        if (!doesExits)
        {
            return NotFound("Todo with Id Does not Exist");
        }

        var todo = _todoBusiness.Complete(id);
        return Ok(todo);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateContent(int id,[FromBody] string content)
    {
        var doesExits = _todoBusiness.DoesExist(id);

        if (!doesExits)
        {
            return NotFound("Todo with Id Does not Exist");
        }

        var todo = _todoBusiness.Update(id, content);
        return Ok(todo);
    }

}