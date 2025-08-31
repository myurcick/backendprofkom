using System.ComponentModel.DataAnnotations;

namespace ProfkomBackend.Models
{
    public class ProfileMember
    {
        [Key]
        public int Id { get; set; }
        public string Position { get; set; } // e.g., голова, секретар
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
