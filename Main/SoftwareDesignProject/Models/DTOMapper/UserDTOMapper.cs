using CommonDTO;
using SoftwareDesignProject.Models.Entities;
using UserRoles = SoftwareDesignProject.Models.Entities.UserRoles;

namespace SoftwareDesignProject.Models.DTOMapper;

public class UserDTOMapper : IDTOMapper<User, UserDTO>
{

    private CommonDTO.UserRoles ConvertTo(UserRoles userRoles)
    {
        return userRoles switch
        {
            UserRoles.Normal => CommonDTO.UserRoles.Normal,
            UserRoles.Premium => CommonDTO.UserRoles.Premium,
            UserRoles.Admin => CommonDTO.UserRoles.Admin,
            _ => CommonDTO.UserRoles.Normal
        };
    }

    private UserRoles ConvertFrom(CommonDTO.UserRoles? userRoles)
    {
        return userRoles switch
        {
            CommonDTO.UserRoles.Normal => UserRoles.Normal,
            CommonDTO.UserRoles.Premium => UserRoles.Premium,
            CommonDTO.UserRoles.Admin => UserRoles.Admin,
            _ => UserRoles.Normal
        };
    }

    public UserDTO ConvertTo(User from)
    {
        return new UserDTO()
        {
            UserId = from.UserId,
            Username = from.Username,
            Password = from.HashedPassword,
            UserRole = ConvertTo(from.UserRole)
        };
    }

    public User ConvertFrom(UserDTO from)
    {
        return new User()
        {
            UserId = from.UserId,
            Username = string.IsNullOrEmpty(from.Username) ? from.UserId.ToString() : from.Username,
            HashedPassword = from.Password!,
            UserRole = ConvertFrom(from.UserRole)
        };
    }

    public void CopyToEntity(User target, UserDTO source)
    {
        if (source.Username != null)
            target.Username = source.Username;
        if (source.Password != null)
            target.HashedPassword = source.Password;
        if (source.UserRole != null)
            target.UserRole = ConvertFrom(source.UserRole);
    }
}