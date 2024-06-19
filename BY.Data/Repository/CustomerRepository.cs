using BY.Data.Base;
using BY.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BY.Data.Repository
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository()
        {
        }

        public Customer? FindUser(string email, string phone){
            return _context.Customers.Where(x=> x.Email == email && x.Phone == phone).FirstOrDefault();
        }

        public Customer? GetCustomerLatest(){
            return _context.Customers.OrderByDescending(x => x.CustomerId).FirstOrDefault();
        }
    }
}
