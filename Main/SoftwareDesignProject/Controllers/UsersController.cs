using System.Security.Claims;
using CommonDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using SoftwareDesignProject.Services;

namespace SoftwareDesignProject.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private static readonly SemaphoreSlim UserSemaphore = new(1, 1);

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "",
        [FromQuery] string order = "",
        [FromQuery] string search = "")
    {
        var userRole = await GetCurrentUserRoleAsync();
        if (userRole is null or < UserRoles.Admin)
        {
            return Forbid();
        }
        
        page = Math.Max(0, page);
        pageSize = Math.Max(1, pageSize);

        var sortDirection = SortDirection.None;
        if (order.Equals("Ascending", StringComparison.OrdinalIgnoreCase))
        {
            sortDirection = SortDirection.Ascending;
        }
        else if (order.Equals("Descending", StringComparison.OrdinalIgnoreCase))
        {
            sortDirection = SortDirection.Descending;
        }

        Console.WriteLine("Getting user list with page: " + page + " and pageSize: " + pageSize +
                          " and sortBy: " + sortBy + " and order: " + sortDirection);
        var users = await _userService.GetList(page, pageSize, sortBy, sortDirection, search);
        return Ok(users);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("role")]
    public async Task<IActionResult> UpdateRole([FromBody] UserDTO dto)
    {
        var userRole = await GetCurrentUserRoleAsync();
        if (userRole is null or < UserRoles.Admin)
        {
            return Forbid();
        }
        if (!await UserSemaphore.WaitAsync(TimeSpan.FromSeconds(10)))
            return StatusCode(503, "Service unavailable");

        try
        {
            await _userService.UpdateUserRole(dto);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        finally
        {
            UserSemaphore.Release();
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userRole = await GetCurrentUserRoleAsync();
        if (userRole is null or < UserRoles.Admin)
        {
            return Forbid();
        }
        try
        {
            await _userService.Delete(id);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserDTO user)
    {
        var userRole = await GetCurrentUserRoleAsync();
        if (userRole is null or < UserRoles.Admin)
        {
            return Forbid();
        }
        try
        {
            await _userService.Add(user);
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetById(id);
        return user != null ? Ok(user) : NotFound();
    }

    [HttpPut("upgrade")]
    public async Task<IActionResult> Upgrade([FromBody] Guid id)
    {
        if (!await UserSemaphore.WaitAsync(TimeSpan.FromSeconds(10)))
            return StatusCode(503, "Service unavailable");
        try
        {
            await _userService.Upgrade(id);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        finally
        {
            UserSemaphore.Release();
        }
    }

    private async Task<UserRoles?> GetCurrentUserRoleAsync()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return null;
        }
        var user = await _userService.GetById(userId.Value);
        return user?.UserRole;
    }
       
    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Upn);
        return Guid.TryParse(userIdClaim?.Value ?? string.Empty, out var res) ? res : null;
    }
}