using Microsoft.AspNetCore.Identity;
using TodoApp.Models;
using TodoApp.Models.ViewModels;

namespace TodoApp.Services.Interfaces;

public interface IUserBusiness
{
    Task<AuthResult> SingUp(SingUpViewModel viewModel);
    Task<AuthResult> SingIn(LogInViewModel logInViewModel);

    Task SingOut();
}