using Application.Contract;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementation
{
    public class UserRepository : IUser
    {
        private readonly FinanceAppDbContext _financeAppDbContext;

        public UserRepository(FinanceAppDbContext financeAppDbContext)
        {
            _financeAppDbContext = financeAppDbContext;
        }

        public async Task<ServiceResponse> AddAsync(User user)
        {
            var existingUserWithEmail = await GetUserByEmailAsync(user.Email);
            if (existingUserWithEmail != null)
                return new ServiceResponse(false, $"The email {user.Email} is already registered");

            var existingUserWithUsername = await GetUserByUsernameAsync(user.Username);
            if (existingUserWithUsername != null)
                return new ServiceResponse(false, $"The username {user.Username} has already been registered");

            _financeAppDbContext.User.Add(user);
            await SaveChangesAsync();
            return new ServiceResponse(true, "User account created.");
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var user = await _financeAppDbContext.User.FindAsync(id);
            if (user != null)
            {
                _financeAppDbContext.User.Remove(user);
                await SaveChangesAsync();
                return new ServiceResponse(true, "User account deleted");
            }
            return new ServiceResponse(false, $"User id {id} does not exist.");
        }

        public async Task<List<User>> GetAsync() => await _financeAppDbContext.User.AsNoTracking().ToListAsync();

        public async Task<User> GetByIdAsync(int id) => await _financeAppDbContext.User.FindAsync(id);

        public async Task<ServiceResponse> UpdateAsync(User user)
        {
            var existingUser = await GetUser(user.Id);
            if (existingUser != null)
            {
                UpdateUserFields(user, existingUser);
                await SaveChangesAsync();
                return new ServiceResponse(true, "User account updated");
            }
            return new ServiceResponse(false, $"User id {user.Id} does not exist.");
        }

        #region Helper Methods
        private async Task SaveChangesAsync()
        {
            await _financeAppDbContext.SaveChangesAsync();
            _financeAppDbContext.ChangeTracker.Clear();
        }

        private async Task<User?> GetUser(int id)
        {
            return await _financeAppDbContext.User.FindAsync(id);
        }

        private async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _financeAppDbContext.User.FirstOrDefaultAsync(u => string.Equals(u.Email.ToLower(), email));
        }

        private async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _financeAppDbContext.User.FirstOrDefaultAsync(u => string.Equals(u.Username.ToLower(), username.ToLower()));
        }
        private static void UpdateUserFields(User user, User existingUser)
        {
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Username = user.Username;
            existingUser.Password = user.Password;
        }
        #endregion
    }
}
