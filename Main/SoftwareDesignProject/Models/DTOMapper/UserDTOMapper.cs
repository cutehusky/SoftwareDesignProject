using CommonDTO;
using SoftwareDesignProject.Models.Entities;
using UserType = SoftwareDesignProject.Models.Entities.UserType;

namespace SoftwareDesignProject.Models.DTOMapper;

public class UserDTOMapper: IDTOMapper<User, UserDTO>
{

    private CommonDTO.UserType ConvertTo(UserType userType)
    {
        return userType switch
        {
            UserType.Normal => CommonDTO.UserType.Normal,
            UserType.Premium => CommonDTO.UserType.Premium,
            UserType.Admin => CommonDTO.UserType.Admin,
            _ => CommonDTO.UserType.Normal
        };
    }

    private UserType ConvertFrom(CommonDTO.UserType userType)
    {
        return userType switch
        {
            CommonDTO.UserType.Normal => UserType.Normal,
            CommonDTO.UserType.Premium => UserType.Premium,
            CommonDTO.UserType.Admin => UserType.Admin,
            _ => UserType.Normal
        };
    }

    public UserDTO ConvertTo(User from)
    {
        return new UserDTO()
        {
            UserId = from.UserId,
            Username = from.Username,
            HashedPassword = from.HashedPassword,
            UserType = ConvertTo(from.UserType),
            CreatedAt = from.CreatedAt
        };
    }

    public User ConvertFrom(UserDTO from)
    {
        return new User()
        {
            UserId = from.UserId,
            Username = from.Username,
            HashedPassword = from.HashedPassword,
            UserType = ConvertFrom(from.UserType),
            CreatedAt = from.CreatedAt
        };
    }
}