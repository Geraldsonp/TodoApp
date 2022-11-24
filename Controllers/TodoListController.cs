using Microsoft.AspNetCore.Mvc;
using TodoApp.Services.Interfaces;

namespace TodoApp.Controllers;

public class TodoListController : Controller
{
    private readonly ITodoListBusiness _todoListBusiness;

    public TodoListController(ITodoListBusiness todoListBusiness)
    {
        _todoListBusiness = todoListBusiness;
    }
    // GET
    [HttpGet]
    public IActionResult GetTodosList([FromRoute]int id)
    {
        var doesListExist = _todoListBusiness.DoesExist(id);

        if (!doesListExist)
        {
            return NotFound("List with that Id does not exist");
        }

        return Ok(_todoListBusiness.GetTodosList(id));
    }

    [HttpGet]
    public IActionResult GetAllTodoList()
    {
        return Ok(_todoListBusiness.GetTodosList());
    }
}