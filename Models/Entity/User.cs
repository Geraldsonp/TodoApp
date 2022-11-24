using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace TodoApp.Models.Entity;

public class User : IdentityUser
{
    public string? FullName { get; set; }
    public string? Password { get; set; }
    [JsonIgnore]
    public virtual ICollection<TodoList> TodosLists { get; set; }
}