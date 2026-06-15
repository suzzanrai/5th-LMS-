using System.ComponentModel.DataAnnotations;

namespace Practice_Project.Entities
{
    public class Book
    {
        // CHANGED: BookId → Id (this is the only fix needed)
        [Key]
        public int Id { get; set; }

        [Required][StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required][StringLength(13)]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int TotalQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int QuantityAvailable { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PublicationDate { get; set; } 

        public bool IsActive { get; set; } = true;

        // Foreign Keys
        [Required] 
        public int AuthorId { get; set; }

        [Required] 
        public int CategoryId { get; set; }

        // Navigation properties (already perfect)
        public virtual Author Author { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<BookIssue> BookIssues { get; set; } = new List<BookIssue>();
    }
}