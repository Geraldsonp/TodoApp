using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.ViewModels;

public class LogInViewModel
{
    [Required]
    public string UserName { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }

    public AuthResult? Result { get; set; }
    
}