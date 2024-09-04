namespace Book_Store.Models
{
    public class BookListViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
