using Application.Contract;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementation
{
    public class CategoryRepository : ICategory
    {
        private readonly FinanceAppDbContext _financeAppDbContext;

        public CategoryRepository(FinanceAppDbContext financeAppDbContext)
        {
            _financeAppDbContext = financeAppDbContext;
        }

        public async Task<ServiceResponse> AddAsync(Category category)
        {
            if (await CategoryExistsByName(category.Name))
                return new ServiceResponse(false, $"There's already a category with the name {category.Name}");

            _financeAppDbContext.Category.Add(category);
            await SaveChangesAsync();
            return new ServiceResponse(true, "Category created");
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var category = await _financeAppDbContext.Category.FindAsync(id);
            if (category != null)
            {
                _financeAppDbContext.Category.Remove(category);
                await SaveChangesAsync();
                return new ServiceResponse(true, "Category deleted");
            }
            return new ServiceResponse(false, $"Couldn't find a category with the id {id}");
        }

        public async Task<List<Category>> GetAsync()
        {
            return await _financeAppDbContext.Category.AsNoTracking().ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _financeAppDbContext.Category.FindAsync(id) ?? throw new Exception($"Category with id {id} not found");
        }

        public async Task<ServiceResponse> UpdateAsync(Category category)
        {
            var existingCategory = await CategoryExistsById(category.Id);
            if (existingCategory!=null)
            {
                UpdateCategoryFields(category, existingCategory); 
                await SaveChangesAsync();
                return new ServiceResponse(true, "Category updated");
            }
            return new ServiceResponse(false, $"Couldn't find a category with the id {category.Id}");
        }

        private static void UpdateCategoryFields(Category category, Category existingCategory)
        {
            existingCategory.Description = category.Description;
            existingCategory.Name = category.Name;
        }

        private async Task SaveChangesAsync()
        {
            await _financeAppDbContext.SaveChangesAsync();
        }

        private async Task<bool> CategoryExistsByName(string name)
        {
            return await _financeAppDbContext.Category.AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        private async Task<Category> CategoryExistsById(int id)
        {
            return await _financeAppDbContext.Category.FindAsync(id);
        }
       
    }
}
