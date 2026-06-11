// Models/CreateAuthorViewModel.cs

using System.ComponentModel.DataAnnotations;
using Practice_Project.Entities;

namespace Practice_Project.Models
{
    public class AuthorViewModel
    {
        public int AuthorId { get; set; }
        [Required][StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Biography { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}