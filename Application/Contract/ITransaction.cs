using Application.DTOs;
using Domain.Entities;

namespace Application.Contract
{
    public interface ITransaction
    {
        public Task<ServiceResponse> UpdateAsync(Transaction transaction);
        public Task<Transaction> GetByIdAsync(int id);
        public Task<List<Transaction>> GetAsync();
        public Task<ServiceResponse> DeleteAsync(int id);
        public Task<ServiceResponse> AddAsync(Transaction transaction);
    }
}
