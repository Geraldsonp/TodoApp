using Microsoft.AspNetCore.Identity;

namespace TodoApp.Models.Entity;

public class User : IdentityUser
{
    public string? FullName { get; set; }
    public string? Password { get; set; }
}