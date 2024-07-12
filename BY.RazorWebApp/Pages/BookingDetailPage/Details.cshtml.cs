using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BY.Data.Models;
using BY.Business;

namespace BY.RazorWebApp.Pages.BookingDetailPage
{
    public class DetailsModel : PageModel
    {
        private readonly IBookingDetailBusiness _bookingDetailBusiness;
        private readonly IBookingBusiness _bookingBusiness;
        private readonly IScheduleBusiness _scheduleBusiness;

        public DetailsModel()
        {
            _bookingDetailBusiness ??= new BookingDetailBusiness();
            _bookingBusiness ??= new BookingBusiness();
            _scheduleBusiness ??= new ScheduleBusiness();
        }

        public BookingDetail BookingDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            BookingDetail = (await _bookingDetailBusiness.GetBookingDetailById(id)).Data as BookingDetail;
            return Page();
        }
    }
}
