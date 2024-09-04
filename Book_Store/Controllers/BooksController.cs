using Book_Store.IRepository;
using Book_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Book_Store.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IWebHostEnvironment _webHostEnvironment; 
        private const int PageSize = 2;
        public BooksController(IBookRepository bookRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository;
            _webHostEnvironment = webHostEnvironment; // Initialize it
        }

        public IActionResult Index(int pageNumber = 1)
        {
            var books = _bookRepository.GetAllBooks(pageNumber, PageSize);
            var totalBooks = _bookRepository.GetTotalBooksCount();
            var totalPages = (int)Math.Ceiling(totalBooks / (double)PageSize);

            var viewModel = new BookListViewModel
            {
                Books = books,
                CurrentPage = pageNumber,
                TotalPages = totalPages
            };

            return View(viewModel);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book, IFormFile Image)
        {
           
            
                if (Image != null)
                {
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }

                    book.ImageUrl = "/images/" + uniqueFileName;
                }

                _bookRepository.AddBook(book);
                _bookRepository.Save();
                return RedirectToAction(nameof(Index));
           

           
        }
        public IActionResult Details(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
                return NotFound();

            return View(book);
        }
        public IActionResult Loan(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
                return NotFound();

            var userId = _bookRepository.GetCurrentUserId(); 

            if (book.Count > 0)
            {
                book.Count--;

                var loan = new Loan
                {
                    BookId = book.Id,
                    UserId = userId,
                    LoanDate = DateTime.Now
                };

                _bookRepository.AddLoan(loan); 
                _bookRepository.Save();

                TempData["SuccessMessage"] = "Book loaned successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "No copies available to loan.";
            }

            return RedirectToAction(nameof(Details), new { id = book.Id });
        }

        public IActionResult Return(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
                return NotFound();

            var userId = _bookRepository.GetCurrentUserId(); 

            var activeLoan = _bookRepository.GetActiveLoan(book.Id, userId);

            if (activeLoan != null)
            {
                activeLoan.ReturnDate = DateTime.Now;

                book.Count++;

                _bookRepository.Save();

                TempData["SuccessMessage"] = "Book returned successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "No active loan found for this book.";
            }

            return RedirectToAction(nameof(Details), new { id = book.Id });
        }

        
    }

}


