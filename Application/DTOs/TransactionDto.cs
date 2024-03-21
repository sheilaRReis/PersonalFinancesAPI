using Domain.Entities;

namespace Application.DTOs
{
    public class TransactionDto
    {
        public required decimal Value { get; set; }
        public required string? Type { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public required int IdUser { get; set; }
        public required int IdCategory { get; set; }
    }
}
