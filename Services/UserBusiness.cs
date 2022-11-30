using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Models.Entity;
using TodoApp.Models.ViewModels;
using TodoApp.Services.Interfaces;

namespace TodoApp.Services;

public class UserBusiness : IUserBusiness
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UserBusiness(IUnitOfWork unitOfWork,  UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    //Todo: Send More meaning full errors
    public async Task<AuthResult> SingUp(SingUpViewModel viewModel)
    {
        var user = new User()
        {
            FullName = viewModel.FullName,
            UserName = viewModel.UserName,
            Password = viewModel.Password
        };

        var result = await _userManager.CreateAsync(user, viewModel.Password);

        if (result.Succeeded)
        {
            var singInResult = await _signInManager.PasswordSignInAsync(user, viewModel.Password, true, false);

            return singInResult.Succeeded
                ? new AuthResult(result.Succeeded, null)
                : new AuthResult(result.Succeeded, "Error");
        }
        else
        {
            return new AuthResult(result.Succeeded, result.Errors.FirstOrDefault().Description);
        }
    }


    public async Task<AuthResult> SingIn(LogInViewModel logInViewModel)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == logInViewModel.UserName);

        if (user is null)
        {
            return new AuthResult(false, "User Name or Password error");
        }

        var result = await _signInManager.PasswordSignInAsync(user, logInViewModel.Password, true, false);

        if (result.Succeeded)
        {
            return new AuthResult(result.Succeeded, null);
        }
        else
        {
            return new AuthResult(result.Succeeded, "Error Login in");
        }
    }

    public async Task SingOut()
    {
        await _signInManager.SignOutAsync();
    }

}