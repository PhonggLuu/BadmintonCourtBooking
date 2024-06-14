using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BY.Data.Models;
using BY.Business;

namespace BY.RazorWebApp.Pages.BookingPage
{
    public class DeleteModel : PageModel
    {
        private readonly IBookingBusiness bookingBusiness;
        public DeleteModel()
        {
            bookingBusiness ??= new BookingBusiness();    
        }

        [BindProperty]
        public Booking Booking { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = (await bookingBusiness.GetBookingById(id.Value)).Data as Booking; 

            if (booking == null)
            {
                return NotFound();
            }
            else
            {
                Booking = booking;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = (await bookingBusiness.GetBookingById(id.Value)).Data as Booking;
            if (booking != null)
            {
                var result = bookingBusiness.DeleteBooking(booking);
                if (result == null)
                {
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
