using BY.Busniness;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BY.RazorWebApp.Pages
{
    public class CustomerModel : PageModel
    {
        private readonly ICustomerBusiness _business = new CustomerBusiness();
        public Customer Customer { get; set; }
        public List<Customer> Customers { get; set; }
        public void OnGet()
        {
             Customers = _business.GetAll().Result.Data as List<Customer>;
        }
    }
}
