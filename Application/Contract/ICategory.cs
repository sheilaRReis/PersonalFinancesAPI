using Application.DTOs;
using Domain.Entities;

namespace Application.Contract
{
    public interface ICategory
    {
        public Task<ServiceResponse> UpdateAsync(Category category);
        public Task<Category> GetByIdAsync(int id);
        public Task<List<Category>> GetAsync();
        public Task<ServiceResponse> DeleteAsync(int id);
        public Task<ServiceResponse> AddAsync(Category category);
    }
}
