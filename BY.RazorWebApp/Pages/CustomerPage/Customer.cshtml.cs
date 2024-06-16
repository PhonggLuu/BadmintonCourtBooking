using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
        public async Task<IActionResult> OnPostDelete(int id)
        {
            var item = await _business.GetById(id);
            if (item != null)
            {
                await _business.Delete(id);
            }
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
