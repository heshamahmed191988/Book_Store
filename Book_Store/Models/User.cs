using Microsoft.AspNetCore.Identity;

namespace Book_Store.Models
{
    public class User: IdentityUser
    {
  
        public ICollection<Loan> Loans { get; set; }
    }
}
