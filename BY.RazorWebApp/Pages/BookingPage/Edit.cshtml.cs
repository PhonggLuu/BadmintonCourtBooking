using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BY.Data.Models;
using BY.Business;

namespace BY.RazorWebApp.Pages.BookingPage
{
    public class EditModel : PageModel
    {
        private readonly IBookingBusiness _bookingBusiness;
        private readonly ICustomerBusiness _customerBusiness;
        public EditModel()
        {
            _bookingBusiness ??= new BookingBusiness();
            _customerBusiness ??= new CustomerBusiness();
        }

        [BindProperty]
        public Booking Booking { get; set; } = new Booking();
        [BindProperty]
        public Customer Customer { get; set; } = new Customer();
        public SelectList CustomerNames { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = (await _bookingBusiness.GetBookingById(id.Value)).Data as Booking;
            if (booking == null)
            {
                return NotFound();
            }
            Booking = booking;

            // Ensure Booking.Customer is not null
            if (Booking.Customer == null)
            {
                Booking.Customer = new Customer();
            }

            // Populate CustomerNames with the list of customer names
            var customersResult = await _customerBusiness.GetAll();
            var customers = customersResult.Data as List<Customer>; // Ensure it's a list
            if (customers == null)
            {
                customers = new List<Customer>();
            }
            CustomerNames = new SelectList(customers, "CustomerId", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _bookingBusiness.UpdateBooking(Booking);

            return RedirectToPage("./Index");
        }
    }
}
