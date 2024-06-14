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
        public CreateModel()
        {
           _bookingBusiness ??= new BookingBusiness();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Booking Booking { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result = _bookingBusiness.CreateBooking(Booking);
            if (result == null)
            {
                return Page();
            }
            else
            {
                return RedirectToPage("./Index");
            }
        }
    }
}
