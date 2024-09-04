using Book_Store.Hubs;
using Book_Store.IRepository;
using Book_Store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Book_Store.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<BookCountHub> _hubContext; 

        public BookRepository(BookStoreContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IHubContext<BookCountHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext; 
        }

        public IEnumerable<Book> GetAllBooks(int pageNumber, int pageSize)
        {
            return _context.Books
                .Include(b => b.Loans)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        public Book GetBookById(int id)
        {
            return _context.Books.Include(b => b.Loans).FirstOrDefault(b => b.Id == id);
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
        }

        public void UpdateBook(Book book)
        {
            _context.Books.Update(book);
        }

        public void DeleteBook(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
        }

        public void Save()
        {
            _context.SaveChanges();

            _hubContext.Clients.All.SendAsync("ReceiveBookCountUpdate");
        }

        public string GetCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user != null ? _userManager.GetUserId(user) : null;
        }

        public void AddLoan(Loan loan)
        {
            _context.Loans.Add(loan);
        }

        public Loan GetActiveLoan(int bookId, string userId)
        {
            return _context.Loans
                .Where(l => l.BookId == bookId && l.UserId == userId && l.ReturnDate == null)
                .FirstOrDefault();
        }

        public int GetTotalBooksCount()
        {
            return _context.Books.Count();
        }
    }
}
