using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.ViewModels;

public class SingUpViewModel
{
    [Required, DataType(DataType.Text), RegularExpression("[a-zA-Z][a-zA-Z0-9-_]{3,32}", ErrorMessage = "Must start with an alphabetic character. Can contain the following characters: a-z A-Z 0-9 - and _")]
    public string? FullName { get; set; }
    [Required, DataType(DataType.Text), RegularExpression("[a-zA-Z][a-zA-Z0-9-_]{3,32}", ErrorMessage = "Must start with an alphabetic character. Can contain the following characters: a-z A-Z 0-9 - and _")]
    public string? UserName { get; set; }
    [Required, DataType(DataType.Text), MinLength(3, ErrorMessage = "Password must be at least 3")]
    public string? Password { get; set; }

    [Required, DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password Does not match")]
    public string? ConfirmPassword { get; set; }

    public AuthResult? SingUpResult { get; set; }
}