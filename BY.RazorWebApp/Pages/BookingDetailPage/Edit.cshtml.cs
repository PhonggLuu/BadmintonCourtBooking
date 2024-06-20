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

namespace BY.RazorWebApp.Pages.BookingDetailPage
{
    public class EditModel : PageModel
    {
        private readonly IBookingDetailBusiness _bookingDetailBusiness;
        private readonly IBookingBusiness _bookingBusiness;
        private readonly IScheduleBusiness _scheduleBusiness;

        public EditModel()
        {
            _bookingBusiness = new BookingBusiness();
            _scheduleBusiness = new ScheduleBusiness();
            _bookingDetailBusiness = new BookingDetailBusiness();
        }

        [BindProperty]
        public BookingDetail BookingDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var bookingdetail =  await _bookingDetailBusiness.GetBookingDetailById(id);
            if (bookingdetail == null)
            {
                return NotFound();
            }
            BookingDetail = bookingdetail.Data as BookingDetail;
            var bookings = (await _bookingBusiness.GetALlBooking()).Data as List<Booking>;
            if(bookings != null)
            {
                ViewData["BookingId"] = new SelectList(bookings, "BookingId", "BookingId");
            }    
            var schedules = (await _scheduleBusiness.GetAllSchedule()).Data as List<Schedule>;
            if(schedules != null)
            {
                ViewData["ScheduleId"] = new SelectList(schedules, "ScheduleId", "ScheduleId");
            }    
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _bookingDetailBusiness.UpdateBookingDetail(BookingDetail);
            if (result.Status == 1)
                return RedirectToPage("./BookingDetail");

            return Page();
        }
    }
}
