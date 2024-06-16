using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BY.Data.Models;
using BY.Business;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BY.RazorWebApp.Pages.SchedulePageV2
{
    public class CreateModel : PageModel
    {
        private readonly IScheduleBusiness _scheduleBusiness;
        private readonly ICourtBusiness _courtBusiness;

        public string? Message { get; set; }
        public string? Error { get; set; }
        public CreateModel()
        {
            _scheduleBusiness = new ScheduleBusiness();
            _courtBusiness = new CourtBusiness();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var courtsResult = await _courtBusiness.GetAllCourt();
            if (courtsResult.Status > 0 && courtsResult.Data != null)
            {
                var courts = courtsResult.Data as List<Court>;
                ViewData["CourtId"] = new SelectList(courts, "CourtId", "Name");
            }
            return Page();
        }

        [BindProperty]
        public Schedule Schedule { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Schedule.From > Schedule.To)
            {
                Error = "Time From not great than or equal time To";
                await OnGetAsync();
                return Page();
            }
            if(Schedule.Date <  DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(1))))
            {
                Error = "Date not less than today";
                await OnGetAsync();
                return Page();
            }
            var resultCreate = await _scheduleBusiness.CreateSchedule(Schedule);
            if (resultCreate != null && resultCreate?.Status == 1)
            {
                Message = resultCreate?.Message;
            }
            else
            {
                Error = resultCreate?.Message;
            }
            return RedirectToPage("./Index");
        }
    }
}
