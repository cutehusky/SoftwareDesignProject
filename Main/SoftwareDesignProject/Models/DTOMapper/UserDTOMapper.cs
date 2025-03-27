using CommonDTO;
using SoftwareDesignProject.Models.Entities;
using UserRoles = SoftwareDesignProject.Models.Entities.UserRoles;

namespace SoftwareDesignProject.Models.DTOMapper;

public class UserDTOMapper: IDTOMapper<User, UserDTO>
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
            HashedPassword = from.HashedPassword,
            UserRole = ConvertTo(from.UserRole)
        };
    }

    public User ConvertFrom(UserDTO from)
    {
        return new User()
        {
            UserId = from.UserId,
            Username = from.Username,
            HashedPassword = from.HashedPassword,
            UserRole = ConvertFrom(from.UserRole)
        };
    }
}