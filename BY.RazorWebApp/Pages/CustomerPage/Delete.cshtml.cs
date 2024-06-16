using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BY.Data.Models;
using BY.Business;

namespace BY.RazorWebApp.Pages.CustomerPage
{
    public class DeleteModel : PageModel
    {
        private readonly ICustomerBusiness _business;

        public DeleteModel()
        {
            _business ??= new CustomerBusiness();
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = (await _business.GetById(id.Value)).Data as Customer;

            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                Customer = customer;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var customer = await _context.Customers.FindAsync(id);
            var customer = (await _business.GetById(id.Value)).Data as Customer;
            if (customer != null)
            {
                var result = _business.Delete(Customer.CustomerId);
                if(result == null)
                {
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
