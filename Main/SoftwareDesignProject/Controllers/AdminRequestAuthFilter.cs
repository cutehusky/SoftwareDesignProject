using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SoftwareDesignProject.Services;
using UserRoles = CommonDTO.UserRoles;

namespace SoftwareDesignProject.Controllers;

public class AdminRequestAuthFilter: IAsyncActionFilter
{
    private readonly IUserService _userService;
    public AdminRequestAuthFilter(IUserService userService)
    {
        _userService = userService;
    }
    
    private async Task<UserRoles?> GetCurrentUserRole(ClaimsPrincipal userClaim)
    {
        var userId = GetCurrentUserId(userClaim);
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

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var user = context.HttpContext.User;
        var userRole = await GetCurrentUserRole(user);
        if (userRole is null or < UserRoles.Admin)
        {
            context.Result = new ForbidResult();
            return;
        }
        await next();
    }
}