using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BY.RazorWebApp.Pages.SchedulePage
{
    public class SchedulePageModel : PageModel
    {
        private readonly IScheduleBusiness _scheduleBusiness = new ScheduleBusiness();

        List<Schedule> schedules {  get; set; }
        public void OnGet()
        {
            
        }
        public async void  GetAllSchedulesAsync() { 
            var result = await _scheduleBusiness.GetALlSchedule();
        }
    }
}
