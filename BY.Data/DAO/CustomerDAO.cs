using BY.Data.Base;
using BY.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BY.Data.Currency
{
    public class CustomerDAO : BaseDAO<Customer>
    {
        public CustomerDAO() : base()
        {
            
        }
    }
}
