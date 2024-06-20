using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BY.Data.Models;

namespace BY.RazorWebApp.Pages.BookingDetailPage
{
    public class CreateModel : PageModel
    {
        private readonly BY.Data.Models.Net1704_221_2_BYContext _context;

        public CreateModel(BY.Data.Models.Net1704_221_2_BYContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId");
        ViewData["ScheduleId"] = new SelectList(_context.Schedules, "ScheduleId", "ScheduleId");
            return Page();
        }

        [BindProperty]
        public BookingDetail BookingDetail { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BookingDetails.Add(BookingDetail);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
