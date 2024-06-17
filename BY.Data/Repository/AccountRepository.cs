using BY.Data.Base;
using BY.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BY.Data.Repository
{
    public class AccountRepository : GenericRepository<Account>
    {
        public AccountRepository() { }

        public async Task<Customer?> Login(string username,string password)
        {
            return await _context.Accounts.Where(x => x.UserName == username && x.Password == password)
                .Include(c => c.Customer)
                .Select(s => s.Customer)
                .FirstOrDefaultAsync();
        }
    }
}
 