using CommonDTO;
using Microsoft.EntityFrameworkCore;
using SoftwareDesignProject.Models.DTOMapper;
using SoftwareDesignProject.Services;


namespace SoftwareDesignProject.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            var res = await _dbContext.Users.Where(u => u.Username == username)
                .Select((user => new UserDTOMapper().ConvertTo(user))).FirstOrDefaultAsync();
            return res;
        }

        public async Task<bool> InsertUserAsync(UserDTO user)
        {
            _dbContext.Add(new UserDTOMapper().ConvertFrom(user));
            var rowsAffected = await _dbContext.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}