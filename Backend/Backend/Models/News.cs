using System.ComponentModel.DataAnnotations;
namespace ProfkomBackend.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Summary { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    }
}