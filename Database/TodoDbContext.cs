using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models.Entity;
using TodoApp.Services.Interfaces;

namespace TodoApp.Database;

public class TodoDbContext : IdentityDbContext<User>
{
    private readonly IUserIdProvider _userIdProvider;

    public TodoDbContext(DbContextOptions<TodoDbContext> options, IUserIdProvider userIdProvider) : base(options)
    {
        _userIdProvider = userIdProvider;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<TodoList>().HasOne<User>(x => x.Owner)
            .WithMany(x => x.TodosLists)
            .HasForeignKey(x => x.OwnerId);

        builder.Entity<Todo>().HasQueryFilter(todo => todo.OwnerId == _userIdProvider.GetCurrentUserId());
        builder.Entity<TodoList>().HasQueryFilter(todo => todo.OwnerId == _userIdProvider.GetCurrentUserId());

    }

    public DbSet<Todo> Todos { get; set; }
    public DbSet<TodoList> TodoLists { get; set; }
}