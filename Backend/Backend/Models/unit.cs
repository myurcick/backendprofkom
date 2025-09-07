using System.ComponentModel.DataAnnotations;

namespace ProfkomBackend.Models
{
    public class Unit
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string content { get; set; }
        public string? ImageUrl { get; set; }
        public bool isActive { get; set; } = true; // чи є в профкомі
    }
}

