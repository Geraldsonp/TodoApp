using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.ViewModels;

public class SingUpViewModel
{
    [Required]
    public string? FullName { get; set; }
    [Required]
    public string? UserName { get; set; }
    [Required, MinLength(3, ErrorMessage = "Password must be at least 3")]
    public string? Password { get; set; }

    [Required, DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password Does not match")]
    public string? ConfirmPassword { get; set; }

    public AuthResult? SingUpResult { get; set; }
}