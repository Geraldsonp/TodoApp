using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = _todoBusiness.Add(createTodoDto);

        if (!result.Succeded)
        {
            switch (result.ErrorType)
            {
                case ErrorTypes.EntityNotFound:
                    return NotFound(result.ErrorMessage);
                case ErrorTypes.ListDoesNotHasSpace:
                    return BadRequest(result.ErrorMessage);
                default:
                    return BadRequest();
            }
        }

        return Ok(result.Data);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var result =  _todoBusiness.Remove(id);

        if (result.Succeded)
        {
            return Ok($"Todo item deleted successfully");
        }

        return NotFound(result.ErrorMessage);
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