using CommonDTO;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using SoftwareDesignProject.Models.DTOMapper;
using SoftwareDesignProject.Models.Entities;
using SoftwareDesignProject.Services;

namespace SoftwareDesignProject.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDTO?> GetUserByUsernameAsync(string username) =>
        await _dbContext.Users
            .Where(u => u.Username == username)
            .Select(u => new UserDTOMapper().ConvertTo(u))
            .FirstOrDefaultAsync();

    public async Task<PaginationList<UserDTO>> GetAll(int page, int pageSize, string sortBy, SortDirection order, string search)
    {
        var totalCount = await _dbContext.Users.CountAsync();
        IOrderedQueryable<User> users = sortBy switch
        {
            "Username" => order == SortDirection.Descending
                ? _dbContext.Users.OrderByDescending(user => user.Username)
                : _dbContext.Users.OrderBy(user => user.Username),
            "UserRole" => order == SortDirection.Descending
                ? _dbContext.Users.OrderByDescending(user => user.UserRole)
                : _dbContext.Users.OrderBy(user => user.UserRole),
            _ => order == SortDirection.Descending
                ? _dbContext.Users.OrderByDescending(user => user.UserId)
                : _dbContext.Users.OrderBy(user => user.UserId)
        };

        var items = await users
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(user => new UserDTOMapper().ConvertTo(user))
            .ToListAsync();

        return new PaginationList<UserDTO>()
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<UserDTO?> GetById(Guid id) =>
        await _dbContext.Users
            .Where(u => u.UserId == id)
            .Select(u => new UserDTOMapper().ConvertTo(u))
            .FirstOrDefaultAsync();

    public async Task<bool> Add(UserDTO dto)
    {
        _dbContext.Users.Add(new UserDTOMapper().ConvertFrom(dto));
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> Remove(Guid id)
    {
        var rows = await _dbContext.Users
            .Where(u => u.UserId == id)
            .ExecuteDeleteAsync();
        return rows > 0;
    }

    public async Task<bool> Update(UserDTO userDTO)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.UserId == userDTO.UserId);
        if (user == null) return false;
        new UserDTOMapper().CopyToEntity(user, userDTO);
        var rowsAffected = await _dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }
}
