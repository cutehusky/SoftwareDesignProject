using CommonDTO;
using MudBlazor;
using SoftwareDesignProject.Repositories;

namespace SoftwareDesignProject.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<PaginationList<UserDTO>> GetList(int page, int pageSize, string sortBy, SortDirection order, string search)
    {
        var users = await _userRepository.GetAll(page, pageSize, sortBy, order, search);
        return users;
    }

    public async Task UpdateUserRole(UserDTO dto)
    {
        var user = await _userRepository.GetById(dto.UserId);
        if (user == null) throw new KeyNotFoundException("User not found");

        var sucesss = await _userRepository.Update(dto);
        if (!sucesss) throw new InvalidOperationException("Failed to update user role");
    }

    public async Task Delete(Guid id)
    {
        var success = await _userRepository.Remove(id);
        if (!success) throw new KeyNotFoundException("User not found");
    }

    public async Task Add(UserDTO user)
    {
        if (user.Password == null)
        {
            throw new ArgumentNullException(nameof(user.Password), "Password cannot be null");
        }

        var hashedPassword = _passwordHasher.HashPassword(user.Password);

        var newUser = new UserDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            Password = hashedPassword,
            UserRole = user.UserRole
        };

        var success = await _userRepository.Add(newUser);
        if (!success) throw new InvalidOperationException("Failed to create user");
    }

    public async Task Upgrade(Guid id)
    {
        var user = await _userRepository.GetById(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        if (user.UserRole == UserRoles.Premium || user.UserRole == UserRoles.Admin)
        {
            throw new InvalidOperationException("User is already premium or admin");
        }
        var updatedUser = new UserDTO
        {
            UserId = user.UserId,
            UserRole = UserRoles.Premium
        };
        var success = await _userRepository.Update(updatedUser);
        if (!success) throw new InvalidOperationException("Failed to upgrade user");
    }
    public async Task<UserDTO?> GetById(Guid id) => await _userRepository.GetById(id);
}
