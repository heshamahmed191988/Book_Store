using System.ComponentModel.DataAnnotations;

namespace Book_Store.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        [StringLength(100, ErrorMessage = "Author cannot be longer than 100 characters.")]
        public string Author { get; set; }

        [StringLength(400, ErrorMessage = "Image URL cannot be longer than 400 characters.")]
        public string? ImageUrl { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Count must be a non-negative number.")]
        public int? Count { get; set; }

        public ICollection<Loan>? Loans { get; set; }
    }
}
