using System.ComponentModel.DataAnnotations;
namespace ProfkomBackend.Models
{
    public class Prof
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string Head { get; set; }
        public string? ImageUrl { get; set; }
        public string summary { get; set; } = string.Empty;
        public string? adress { get; set; }
        public string? email { get; set; }
        public string? schedule { get; set; }
        public string? facultyURL { get; set; }
        public bool isActive { get; set; } = true; 
    }
}
