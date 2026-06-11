// Models/CreateCategoryViewModel.cs

using System.ComponentModel.DataAnnotations;

namespace Practice_Project.Models
{
    public class CategoryViewModel
    {
       
        public int CategoryId { get; set; }
        
        [Required][StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int BooksCount { get; set; }
    }
}