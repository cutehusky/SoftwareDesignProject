using System.Data;
using Dapper;
using SoftwareDesignProject.Models;


namespace SoftwareDesignProject.Repositories
{
    public class UserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            string sql = "SELECT * FROM users WHERE username = @username";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { username });
        }

        public async Task<bool> InsertUserAsync(User user)
        {
            string sql = "INSERT INTO users (username, password_hash) VALUES (@username, @password_hash)";
            int rowsAffected = await _dbConnection.ExecuteAsync(sql, user);
            return rowsAffected > 0;
        }
    }
}