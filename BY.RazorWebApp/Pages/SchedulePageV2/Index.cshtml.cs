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
    public class IndexModel : PageModel
    {
        private readonly IScheduleBusiness _scheduleBusiness;

        public IndexModel()
        {
            _scheduleBusiness ??= new ScheduleBusiness();
        }

        public IList<Schedule> Schedule { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await _scheduleBusiness.GetAllSchedule();
            if (result.Status > 0 && result.Data != null)
            {
                Schedule = result.Data as IList<Schedule>;

            }
        }
    }
}
