using Application.Contract;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementation
{
    public class TransactionRepository : ITransaction
    {
        private readonly FinanceAppDbContext _financeAppDbContext;

        public TransactionRepository(FinanceAppDbContext financeAppDbContext)
        {
            _financeAppDbContext = financeAppDbContext;
        }

        public async Task<ServiceResponse> AddAsync(Transaction transaction)
        {
            var existingCategory = await CategoryExistsById(transaction.IdCategory);
            if (existingCategory is null)
                return new ServiceResponse(false, $"There are no categories with the id {transaction.IdCategory}");
            
            var existingUser = await UserExistsById(transaction.IdUser);
            if (existingUser is null)
                return new ServiceResponse(false, $"There are no users with the id {transaction.IdUser}");
            
            transaction.User = existingUser;
            transaction.Category = existingCategory;
            _financeAppDbContext.Transaction.Add(transaction);
            await SaveChangesAsync();
            return new ServiceResponse(true, "Transaction created");
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var transaction = await GetTransaction(id);
            if (transaction != null)
            {
                _financeAppDbContext.Transaction.Remove(transaction);
                await SaveChangesAsync();
                return new ServiceResponse(true, "Transaction deleted");
            }
            return new ServiceResponse(false, $"Couldn't find a transaction with the id {id}");
        }

        public async Task<List<Transaction>> GetAsync()
        {
            return await _financeAppDbContext.Transaction.AsNoTracking()
                                                         .Include(t => t.Category)
                                                         .Include(t => t.User)
                                                         .ToListAsync();
        }

        public async Task<Transaction> GetByIdAsync(int id)
        {
            return await GetTransaction(id) ?? throw new Exception($"Transaction with id {id} not found");
        }

        public async Task<ServiceResponse> UpdateAsync(Transaction transaction)
        {
            var existingTransaction = await GetTransaction(transaction.Id);
            if (existingTransaction != null)
            {
                UpdateTransactionFields(transaction, existingTransaction);
                await SaveChangesAsync();
                return new ServiceResponse(true, "Transaction updated");
            }
            return new ServiceResponse(false, $"Couldn't find a transaction with the id {transaction.Id}");
        }

        
        #region Métodos Auxiliares

        private async Task SaveChangesAsync()
        {
            await _financeAppDbContext.SaveChangesAsync();
        }

        private async Task<Transaction?> GetTransaction(int id)
        {
            return await _financeAppDbContext.Transaction.AsNoTracking()
                                                         .Include(t => t.Category)
                                                         .Include(t => t.User)
                                                         .Where(t => t.Id.Equals(id))
                                                         .FirstOrDefaultAsync();
        }
        private static void UpdateTransactionFields(Transaction transaction, Transaction existingTransaction)
        {
            existingTransaction.Category = transaction.Category;
            existingTransaction.Description = transaction.Description;
            existingTransaction.Date = transaction.Date;
            existingTransaction.Type = transaction.Type;
            existingTransaction.Value = transaction.Value;
            existingTransaction.Category = transaction.Category;
            existingTransaction.User = transaction.User;
        }

        private async Task<Category> CategoryExistsById(int id)
        {
            return await _financeAppDbContext.Category.FindAsync(id);
        }
        private async Task<User> UserExistsById(int id)
        {
            return await _financeAppDbContext.User.FindAsync(id);
        }
        #endregion
    }
}
