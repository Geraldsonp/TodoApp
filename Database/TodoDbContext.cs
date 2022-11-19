using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models.Entity;

namespace TodoApp.Database;

public class TodoDbContext : IdentityDbContext<User>
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {

    }

}