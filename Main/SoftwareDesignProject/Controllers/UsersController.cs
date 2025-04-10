using Microsoft.AspNetCore.Mvc;
using CommonDTO;
using SoftwareDesignProject.Services;
using MudBlazor;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SoftwareDesignProject.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

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
        if (userRole < CommonDTO.UserRoles.Admin)
        {
            return Forbid();
        }
        if (!await _semaphore.WaitAsync(TimeSpan.FromSeconds(10)))
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
            _semaphore.Release();
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userRole = await GetCurrentUserRoleAsync();
        if (userRole < CommonDTO.UserRoles.Admin)
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
        if (userRole < CommonDTO.UserRoles.Admin)
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
        if (!await _semaphore.WaitAsync(TimeSpan.FromSeconds(10)))
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
            _semaphore.Release();
        }
    }

    private async Task<CommonDTO.UserRoles> GetCurrentUserRoleAsync()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        // Default role if no valid user ID is found
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return CommonDTO.UserRoles.Normal;
        }

        var user = await _userService.GetById(userId);
        if (user == null)
        {
            return CommonDTO.UserRoles.Normal;
        }

        return user.UserRole ?? CommonDTO.UserRoles.Normal;
    }
}