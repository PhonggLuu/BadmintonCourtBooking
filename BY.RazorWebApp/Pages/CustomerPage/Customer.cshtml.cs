using BY.Busniness;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BY.RazorWebApp.Pages
{
    public class CustomerModel : PageModel
    {
        private readonly ICustomerBusiness _business = new CustomerBusiness();
        public string Message { get; set; } = default;
        [BindProperty]
        public Customer Customer { get; set; }
        public List<Customer> Customers { get; set; }
        public void OnGet()
        {
            Customers = GetCustomers();
        }
        public IActionResult OnPost()
        {
            AddCustomer();
            return RedirectToPage();
        }

        private List<Customer> GetCustomers()
        {
            var customerResult = _business.GetAll().Result;
            if(customerResult.Status > 0 && customerResult.Data != null)
            {
                var customers = customerResult.Data as List<Customer>;
                return customers;
            }
            return new List<Customer>();
        }

        private void AddCustomer()
        {
            var result = _business.Save(Customer);
            if(result != null)
            {
                Message = result.Result.Message;
            }
            else
            {
                Message = "Error System";
            }
        }
    }
}
