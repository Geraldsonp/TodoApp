using Microsoft.AspNetCore.Mvc;
using TodoApp.Models.ViewModels;
using TodoApp.Services.Interfaces;

namespace TodoApp.Controllers;

public class AuthController : Controller
{
    private readonly IUserBusiness _userBusiness;

    public AuthController(IUserBusiness userBusiness)
    {
        _userBusiness = userBusiness;
    }
    // GET
    [HttpGet]
    public IActionResult LogIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(LogInViewModel logInViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(logInViewModel);
        }

        var result = await _userBusiness.SingIn(logInViewModel);

        if (!result.Succeded)
        {
             logInViewModel.Result = result;
            return View(logInViewModel);
        }

        return RedirectToAction("index", "Todo");
    }

    [HttpGet]
    public async Task<IActionResult> DemoLogIn()
    {
        var viewModel = new LogInViewModel()
        {
            UserName = "John_Doe",
            Password = "jRx3kVh7W3HNkJ8"
        };
        var result = await _userBusiness.SingIn(viewModel);

        return RedirectToAction("index", "Todo");
    }

    [HttpGet]
    public IActionResult SingUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SingUp(SingUpViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var authResult = await _userBusiness.SingUp(viewModel);

        if (!authResult.Succeded)
        {
            viewModel.SingUpResult = authResult;
            return View(viewModel);
        }

        return RedirectToAction("Index", "Todo");
    }

    [HttpGet]
    public async Task<IActionResult> SingOut()
    {
        await _userBusiness.SingOut();
        return RedirectToAction("index", "Todo");
    }
}