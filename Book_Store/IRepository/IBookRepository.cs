using Book_Store.Models;

namespace Book_Store.IRepository
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAllBooks(int pageNumber, int pageSize);
        Book GetBookById(int id);
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int id);
        void Save();
        string GetCurrentUserId();
        void AddLoan(Loan loan);
        Loan GetActiveLoan(int bookId, string userId);
        int GetTotalBooksCount();
    }
}
