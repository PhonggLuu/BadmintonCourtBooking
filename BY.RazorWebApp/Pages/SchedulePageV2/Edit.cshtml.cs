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

namespace BY.RazorWebApp.Pages.SchedulePageV2
{
    public class EditModel : PageModel
    {
        private readonly IScheduleBusiness _scheduleBusiness;
        private readonly ICourtBusiness _courtBusiness;

        public string? Message { get; set; }
        public string? Error { get; set; }
        public EditModel()
        {
            _scheduleBusiness = new ScheduleBusiness();
            _courtBusiness = new CourtBusiness();
        }

        [BindProperty]
        public Schedule Schedule { get; set; } = default!;
        private async void GetALlCcourt() {
            var courtsResult = await _courtBusiness.GetAllCourt();
            if (courtsResult.Status > 0 && courtsResult.Data != null)
            {
                var courts = courtsResult.Data as List<Court>;
                ViewData["CourtId"] = new SelectList(courts, "CourtId", "Name");
            }
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var schedule = await _scheduleBusiness.GetScheduleById(id);
            if (schedule == null || schedule.Status < 0 || schedule.Data == null)
            {
                return NotFound();
            }
            Schedule = schedule.Data as Schedule;
            GetALlCcourt();
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
            if (Schedule.From > Schedule.To)
            {
                Error = "Time From not great than or equal time To";
                 GetALlCcourt();
                return Page();
            }
            if (Schedule.Date < DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(1))))
            {
                Error = "Date not less than today";
                GetALlCcourt();
                return Page();
            }
            var result = await _scheduleBusiness.UpdateSchedule(Schedule);
            if (result != null && result?.Status == 1)
            {
                Message = result?.Message;
            }
            else
            {
                Error = result?.Message;
            }

            return RedirectToPage("./Index");
        }

    }
}
