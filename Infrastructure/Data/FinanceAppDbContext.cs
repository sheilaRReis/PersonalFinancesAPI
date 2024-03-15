using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class FinanceAppDbContext : DbContext
    {
        public FinanceAppDbContext(DbContextOptions<FinanceAppDbContext> options) : base(options) { } 

        public DbSet<User> User { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

    }
}
