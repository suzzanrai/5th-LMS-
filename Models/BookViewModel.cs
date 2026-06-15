// Models/BookViewModel.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Practice_Project.Models
{
    public class BookViewModel
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(13)]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "ISBN must be 13 digits")]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int TotalQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int QuantityAvailable { get; set; }

        [DataType(DataType.Date)]
        //     [Display(Name = "Publication Date")]
        public DateTime? PublicationDate { get; set; } 

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // Foreign keys (dropdowns)
        [Range(1, int.MaxValue, ErrorMessage = "Please select an author")]
        public int AuthorId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        // For SelectList in views
        public IEnumerable<SelectListItem>? Authors { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }

        // Display names used in index/delete views
        public string? AuthorName { get; set; }
        public string? CategoryName { get; set; }
    }
}