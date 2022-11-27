using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoApp.Database;
using TodoApp.Helpers;
using TodoApp.Models;
using TodoApp.Models.Entity;
using TodoApp.Services;
using TodoApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<TodoDbContext>(x =>
    x.UseNpgsql(PostgresHelper.GetConnectionString(builder.Configuration))
        .UseLazyLoadingProxies());

builder.Services.Configure<JwtSettings>(builder.Configuration);

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;

    // User settings.
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
}).AddEntityFrameworkStores<TodoDbContext>();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/auth/LogIn";
    opt.LogoutPath = "/Auth/Logout";
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();
builder.Services.AddScoped<ITodoBusiness, TodoBusiness>();
builder.Services.AddScoped<IUserIdProvider, UserIdProvider>();
builder.Services.AddScoped<ITodoListBusiness, TodoListBusiness>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Todo}/{action=Index}/{id?}");

app.Run();