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
        private readonly ICourtBusiness _courtBusiness = new CourtBusiness();

        public string? Message { get; set; }
        public string? Error { get; set; }

        public List<Schedule>? Schedules { get; private set; } = default;
        [BindProperty]
        public Schedule Schedule { get; set; } = new Schedule();
        [BindProperty]
        public List<Court>? Courts { get; set; } = default;
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

        public async Task OnGetScheduleAsync()
        {
            var scheduleResult = await _scheduleBusiness.GetAllSchedule();
            if (scheduleResult != null && scheduleResult.Status == 1)
            {
                Schedules = scheduleResult.Data as List<Schedule>;
            }
            else
            {
                Error = scheduleResult?.Message;
            }
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await OnGetCourtAsync();
                await OnGetScheduleAsync();
            }
            catch
            {
                return RedirectToPage("./Error");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
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
            return RedirectToPage("./SchedulePage");
        }
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            int.TryParse(Request.Form["idSchedule"], out int idSchedule);
            var result = await _scheduleBusiness.GetScheduleById(idSchedule);
            if (result != null && result?.Status == 1)
            {
                if (result?.Data != null)
                {
                    var schedule = result?.Data as Schedule;
                    if (schedule != null)
                    {
                        var resultDelete = await _scheduleBusiness.DeleteSchedule(schedule);
                        if (resultDelete != null && resultDelete?.Status == 1)
                        {
                            Message = resultDelete?.Message;
                        }
                        else
                        {
                            Error = resultDelete?.Message;
                        }
                    }
                    else
                    {
                        Error = result?.Message;
                    }
                }
            }
            else
            {
                Error = result?.Message;
            }
            await OnGetAsync();
            return Page();
        }
    }
}
