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
    public class DetailsModel : PageModel
    {
        private readonly IScheduleBusiness _scheduleBusiness;

        public DetailsModel()
        {
            _scheduleBusiness ??= new ScheduleBusiness();
        }

        public Schedule Schedule { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var result = await _scheduleBusiness.GetScheduleById(id);

            if (result == null || result.Status < 0 || result.Data == null)
            {
                return NotFound();
            }
            else
            {
                Schedule = result.Data as Schedule;
            }
            return Page();
        }
    }
}
