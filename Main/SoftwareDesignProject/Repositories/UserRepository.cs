using CommonDTO;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
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

        public async Task<PaginationList<UserDTO>> GetAll( int page, int pageSize,
            string sortBy, SortDirection order, string search)
        {
            var totalCount = _dbContext.Users.Count();
            var users = await _dbContext.Users
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(user => new UserDTOMapper().ConvertTo(user))
                .ToListAsync();

            return new PaginationList<UserDTO>()
            {
                Items = users,
                TotalCount = totalCount
            };
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