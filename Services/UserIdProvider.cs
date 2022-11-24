using System.Security.Claims;
using TodoApp.Services.Interfaces;

namespace TodoApp.Services;

public class UserIdProvider : IUserIdProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

    }

    public string GetCurrentUserId()
    {
        if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
           return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        else
        {
            return "";
        }
    }
}