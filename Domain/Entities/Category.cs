using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
