using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BY.Data.Models;
using BY.Busniness;
using BY.Business;

namespace BY.RazorWebApp.Pages.BookingPage
{
    public class DetailsModel : PageModel
    {
        private readonly IBookingBusiness _business;
        public DetailsModel()
        {
            _business ??= new BookingBusiness();
        }

        public Booking Booking { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var result = await _business.GetBookingById(id.Value);

            if (result == null || result.Status < 0 || result.Data == null)
            {
                return NotFound();
            }
            else
            {
                Booking = result.Data as Booking;
            }
            return Page();
        }
    }
}
