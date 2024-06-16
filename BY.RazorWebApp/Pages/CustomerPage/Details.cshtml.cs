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
    public class DetailsModel : PageModel
    {
        private readonly ICustomerBusiness _business;

        public DetailsModel()
        {
            _business ??= new CustomerBusiness();
        }

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
    }
}
