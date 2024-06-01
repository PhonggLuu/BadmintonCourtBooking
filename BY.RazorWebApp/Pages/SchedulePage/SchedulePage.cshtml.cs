using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BY.RazorWebApp.Pages.SchedulePage
{
    public class SchedulePageModel : PageModel
    {
        private readonly IScheduleBusiness _scheduleBusiness = new ScheduleBusiness();
        public string? Error { get; set; }

        public List<Schedule>? Schedule { get; private set; }
        public async Task OnGetAsync()
        {
                var result = await _scheduleBusiness.GetAllSchedule();
                if (result != null || result?.Status == 1)
                {
                    Schedule = result.Data as List<Schedule>;
                }
                else
                {
                    Error = result?.Message;
                }
        }
    }
}
