using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BY.RazorWebApp.Pages.SchedulePage
{
    public class EditModel : PageModel
    {
        private readonly IScheduleBusiness _scheduleBusiness = new ScheduleBusiness();
        private readonly CourtBusiness _courtBusiness = new CourtBusiness();

        public string? Error { get; set; }
        [BindProperty]
        public Schedule Schedule { get; set; } = new Schedule();
        [BindProperty]
        public List<Court>? Courts { get; set; } = new List<Court>();
        public async Task<IActionResult> OnGetAsync(int id)
        {
            await OnGetCourtAsync();
            await OnGetScheduleByIdAsync(id);
            return Page();
        }

        public async Task OnGetCourtAsync()
        {
            var courtResult = await _courtBusiness.GetAllCourt();
            if (courtResult != null && courtResult.Status == 1)
            {
                Courts = courtResult.Data as List<Court>;
            }
            else
            {
                Error = courtResult?.Message;
            }
        }

        public async Task OnGetScheduleByIdAsync(int idSchedule)
        {
            var scheduleResult = await _scheduleBusiness.GetScheduleById(idSchedule);
            if (scheduleResult != null && scheduleResult.Status == 1)
            {
                Schedule = scheduleResult.Data as Schedule;
            }
            else
            {
                Error = scheduleResult?.Message;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Schedule.From > Schedule.To)
            {
                Error = "Time From not great than or equal time To";
                await OnGetCourtAsync();
                return Page();
            }
            if (Schedule.Date < DateTime.Now.Subtract(TimeSpan.FromDays(1)))
            {
                Error = "Date not less than today";
                await OnGetCourtAsync();
                return Page();
            }
            var resultCreate = await _scheduleBusiness.UpdateSchedule(Schedule);
            if (resultCreate != null && resultCreate?.Status == 1)
            {
                return RedirectToPage("./SchedulePage");
            }
            else
            {
                Error = resultCreate?.Message;
            }
            return Page();
        }
    }
}
