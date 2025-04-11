using CommonDTO;
using SoftwareDesignProject.Models.Entities;
using UserRoles = SoftwareDesignProject.Models.Entities.UserRoles;

namespace SoftwareDesignProject.Models.DTOMapper;

public class UserDTOMapper : IDTOMapper<User, UserDTO>
{
    public UserDTO ConvertTo(User from)
    {
        return new UserDTO()
        {
            UserId = from.UserId,
            Username = from.Username,
            Password = from.HashedPassword,
            UserRole = (CommonDTO.UserRoles) from.UserRole
        };
    }

    public User ConvertFrom(UserDTO from)
    {
        return new User()
        {
            UserId = from.UserId,
            Username = string.IsNullOrEmpty(from.Username) ? from.UserId.ToString() : from.Username,
            HashedPassword = from.Password!,
            UserRole = from.UserRole.HasValue ? (UserRoles) from.UserRole.Value : UserRoles.Normal
        };
    }

    public void CopyToEntity(User target, UserDTO source)
    {
        if (source.Username != null)
            target.Username = source.Username;
        if (source.Password != null)
            target.HashedPassword = source.Password;
        if (source.UserRole != null)
            target.UserRole = (UserRoles) source.UserRole;
    }
}