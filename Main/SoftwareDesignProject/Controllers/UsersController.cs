using CommonDTO;
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

    [ServiceFilter(typeof(AdminRequestAuthFilter))]
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

    [ServiceFilter(typeof(AdminRequestAuthFilter))]
    [HttpPut("role")]
    public async Task<IActionResult> UpdateRole([FromBody] UserDTO dto)
    {
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

    [ServiceFilter(typeof(AdminRequestAuthFilter))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
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

    [ServiceFilter(typeof(AdminRequestAuthFilter))]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserDTO user)
    {
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
        // TODO: not authorized
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid Plugin ID.");
        }
        var user = await _userService.GetById(id);
        return user != null ? Ok(user) : NotFound();
    }

    [HttpPut("premium")]
    public async Task<IActionResult> Upgrade([FromBody] Guid id)
    {
        // TODO: not authorized
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
}