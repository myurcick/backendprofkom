using System.ComponentModel.DataAnnotations;

namespace ProfkomBackend.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; } // e.g., голова, секретар
        public string content { get; set; }
        public string? ImageUrl { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool isActive { get; set; } = true; // чи є в профкомі
    }
}
