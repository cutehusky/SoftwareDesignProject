using System.Security.Claims;
using CommonDTO;
using Microsoft.AspNetCore.Mvc.Filters;
using SoftwareDesignProject.Services;

namespace SoftwareDesignProject.Controllers;

public class GetUserInfoActionFilter: IAsyncActionFilter
{
    private readonly IUserService _userService;
    public GetUserInfoActionFilter(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var user = context.HttpContext.User;
        var userId = GetCurrentUserId(user);
        var userRole = await GetCurrentUserRole(userId);
        context.HttpContext.Items["UserId"] = userId;
        context.HttpContext.Items["UserRole"] = userRole;
        await next();
    }
    
    private async Task<UserRoles?> GetCurrentUserRole(Guid? userId)
    {
        if (userId == null)
            return null;
        var user = await _userService.GetById(userId.Value);
        return user?.UserRole;
    }
      
    private static Guid? GetCurrentUserId(ClaimsPrincipal userClaim)
    {
        if (userClaim.Identity is not { IsAuthenticated: true })
            return null;
        var userIdClaim = userClaim.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Upn);
        return Guid.TryParse(userIdClaim?.Value ?? string.Empty, out var res) ? res : null;
    }
}