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
    public class IndexModel : PageModel
    {
        private readonly IBookingBusiness bookingBusiness;
        public IndexModel()
        {
            bookingBusiness ??= new BookingBusiness();
        }

        public IList<Booking> Booking { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Booking = (await bookingBusiness.GetALlBooking()).Data as IList<Booking>;
        }
    }
}
