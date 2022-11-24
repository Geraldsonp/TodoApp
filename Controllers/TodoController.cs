using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Models.ViewModels.TodoViewModels;
using TodoApp.Services.Interfaces;

namespace TodoApp.Controllers;

[Authorize]
public class TodoController : Controller
{
    private readonly ITodoBusiness _todoBusiness;

    public TodoController(ITodoBusiness todoBusiness)
    {
        _todoBusiness = todoBusiness;
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
    public IActionResult Create([FromBody] string content)
    {
        var todoItem = _todoBusiness.Add(content);
        return Ok(todoItem);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
         _todoBusiness.Remove(id);
        return NoContent();
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