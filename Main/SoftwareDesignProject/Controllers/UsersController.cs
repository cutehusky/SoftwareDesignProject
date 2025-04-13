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
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
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

        _logger.LogTrace("Getting user list with page: " + page + " and pageSize: " + pageSize +
                          " and sortBy: " + sortBy + " and order: " + sortDirection);
        var users = await _userService.GetList(page, pageSize, sortBy, sortDirection, search);
        return Ok(users);
    }

    [ServiceFilter(typeof(AdminRequestAuthFilter))]
    [HttpPut("role")]
    public async Task<IActionResult> UpdateRole([FromBody] UserDTO dto)
    {
        if (!await UserSemaphore.WaitAsync(TimeSpan.FromSeconds(10)))
        {
            _logger.LogError("UserSemaphore is not available");
            return StatusCode(503, "Service unavailable");
        }

        try
        {
            _logger.LogInformation("Updating user role with id: " + dto.UserId);
            await _userService.UpdateUserRole(dto);
            _logger.LogInformation($"User {dto.UserId} role updated successfully");
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to update user role: " + ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid User ID.");
        }
        
        try
        {
            _logger.LogInformation("Deleting user with ID: " + id);
            await _userService.Delete(id);
            _logger.LogInformation($"User {id} deleted successfully");
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to delete user: " + ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [ServiceFilter(typeof(AdminRequestAuthFilter))]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserDTO user)
    {
        try
        {
            _logger.LogInformation("Creating new user");
            await _userService.Add(user);
            _logger.LogInformation($"User {user.UserId} created successfully");
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
        }
        catch (InvalidDataException ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to create user: " + ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        // TODO: not authorized
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid User ID.");
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
            _logger.LogInformation("Upgrading premium for user with ID: " + id);
            await _userService.Upgrade(id);
            _logger.LogInformation($"User {id} upgraded to premium successfully");
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to upgrade user to premium: " + ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        finally
        {
            UserSemaphore.Release();
        }
    }
}