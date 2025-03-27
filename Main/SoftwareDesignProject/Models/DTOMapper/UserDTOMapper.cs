using DTO;
using SoftwareDesignProject.Models.Entities;
using UserType = SoftwareDesignProject.Models.Entities.UserType;

namespace SoftwareDesignProject.Models.DTOMapper;

public class UserDTOMapper: IDTOMapper<User, UserDTO>
{

    private DTO.UserType ConvertTo(UserType userType)
    {
        return userType switch
        {
            UserType.Normal => DTO.UserType.Normal,
            UserType.Premium => DTO.UserType.Premium,
            UserType.Admin => DTO.UserType.Admin,
            _ => DTO.UserType.Normal
        };
    }

    private UserType ConvertFrom(DTO.UserType userType)
    {
        return userType switch
        {
            DTO.UserType.Normal => UserType.Normal,
            DTO.UserType.Premium => UserType.Premium,
            DTO.UserType.Admin => UserType.Admin,
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