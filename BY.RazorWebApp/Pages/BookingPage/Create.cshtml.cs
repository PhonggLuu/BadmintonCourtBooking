using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BY.Data.Models;
using BY.Business;

namespace BY.RazorWebApp.Pages.BookingPage
{
    public class CreateModel : PageModel
    {
        private readonly IBookingBusiness _bookingBusiness;
        private readonly ICustomerBusiness _customerBusiness;

        public CreateModel()
        {
            _bookingBusiness ??= new BookingBusiness();
            _customerBusiness ??= new CustomerBusiness();
        }

        [BindProperty]
        public Booking Booking { get; set; } = new Booking();

        public SelectList CustomerNames { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var customersResult = await _customerBusiness.GetAll();
            var customers = customersResult.Data as List<Customer>; // Ensure it's a list
            if (customers == null)
            {
                customers = new List<Customer>();
            }
            CustomerNames = new SelectList(customers, "Name", "Name");
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _bookingBusiness.CreateBooking(Booking);
            if (result == null)
            {
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}
