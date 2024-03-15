using Application.DTOs;
using Domain.Entities;

namespace Application.Contract
{
    public interface IUser
    {
        Task<ServiceResponse> AddAsync(User user);
        Task<ServiceResponse> UpdateAsync(User user);
        Task<ServiceResponse> DeleteAsync(int id);
        Task<List<User>>? GetAsync();
        Task<User>? GetByIdAsync(int id);
    }
}
