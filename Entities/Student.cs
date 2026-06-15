using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_Project.Entities
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        public string RollNumber { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public virtual ICollection<BookIssue> BookIssues { get; set; } = new List<BookIssue>();
        
        public ICollection<Fine> Fines { get; set; } = new List<Fine>();
    }
}