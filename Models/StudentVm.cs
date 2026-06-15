using System.ComponentModel.DataAnnotations;

namespace Practice_Project.Models;

public class StudentVm
{
    [Key]
    public int StudentId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
    public string Phone { get; set; } = string.Empty;

    public string RollNumber { get; set; } = string.Empty;

    public string RollNumberDisplay => string.IsNullOrEmpty(RollNumber) ? "Auto-generated" : RollNumber;

    public bool IsActive { get; set; } = true;
    
    public virtual ICollection<BookIssueViewModel> BookIssues { get; set; } = new List<BookIssueViewModel>();
}