using CommonDTO;
using Microsoft.EntityFrameworkCore;
using SoftwareDesignProject.Models.DTOMapper;
using SoftwareDesignProject.Services;


namespace SoftwareDesignProject.Repositories
{
    public class UserRepository: IUserRepository
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

        public Task<List<UserDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO?> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Add(UserDTO pluginDto)
        {
            _dbContext.Add(new UserDTOMapper().ConvertFrom(pluginDto));
            var rowsAffected = await _dbContext.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public Task<bool> Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(UserDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}