using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Book_Store.Models
{
    public class BookStoreContext: IdentityDbContext<User>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }


        public BookStoreContext(DbContextOptions<BookStoreContext> dbContextOptions) : base(dbContextOptions) { }


    }
}
