using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Precision(18, 2)]
        public required decimal Value { get; set; }
        public required string? Type { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public required int IdUser { get; set; }
        public User? User { get; set; }
        public required int IdCategory { get; set; }
        public Category? Category { get; set; }
    }
}
