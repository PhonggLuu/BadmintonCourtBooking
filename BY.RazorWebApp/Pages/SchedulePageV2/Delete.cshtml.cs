using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BY.Data.Models;
using BY.Business;

namespace BY.RazorWebApp.Pages.SchedulePageV2
{
    public class DeleteModel : PageModel
    {
        private readonly IScheduleBusiness _scheduleBusiness;

        public string? Message { get; set; }
        public string? Error { get; set; }
        public DeleteModel()
        {
            _scheduleBusiness = new ScheduleBusiness();
        }

        [BindProperty]
        public Schedule Schedule { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {


            var schedule = await _scheduleBusiness.GetScheduleById(id);

            if (schedule == null || schedule.Status < 0 || schedule.Data == null)
            {
                return NotFound();
            }
            else
            {
                Schedule = schedule.Data as Schedule;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var schedule = await _scheduleBusiness.GetScheduleById(id);
            if (schedule != null)
            {
                Schedule = schedule.Data as Schedule;
               var result = await _scheduleBusiness.DeleteSchedule(Schedule);
                if(result.Status < 0)
                {
                    return RedirectToPage("./Delete");
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
