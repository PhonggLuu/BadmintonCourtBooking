using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Cors.Infrastructure;
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

        public List<Schedule>? Schedules { get; private set; } = new List<Schedule>();
        [BindProperty]
        public Schedule Schedule { get; set; } = default!;
        [BindProperty]
        public List<Court>? Courts { get; set; } = new List<Court>();
        public string Event { get; set; } = string.Empty;
        public string StaffCheck { get; set; } = string.Empty;
        public int TotalCount { get; set; } = 0;
        public int PageTotal { get; set; } = 0;
        public int CurrentPage { get; set; } = 0;
        public int PageSize { get; set; } = 0;

        public async Task OnGetCourtAsync()
        {
            var courtResult = await _courtBusiness.GetAllCourt();
            if (courtResult != null && courtResult.Status == 1)
            {
                Courts = courtResult.Data as List<Court>;
            }
            else
            {
                if (courtResult != null && courtResult.Status == -4)
                {
                    Error = "The system occur error. Please try it again.";
                }
                else
                {
                    Error = courtResult?.Message;
                }
            }
        }

        public async Task OnGetScheduleAsync()
        {
            var scheduleResult = await _scheduleBusiness.GetAllSchedule();
            if (scheduleResult != null && scheduleResult.Status == 1)
            {
                Event = Request.Query["eventSearch"];
                StaffCheck = Request.Query["staffSearch"];
                Schedules = scheduleResult.Data as List<Schedule>;
                if (!int.TryParse(Request.Query["page"], out int page))
                {
                    page = 1;
                }
                if (!int.TryParse(Request.Query["pagesize"], out int pagesize))
                {
                    pagesize = 10;
                }
                if (Schedules != null && Schedules.Count > 0)
                {
                    if (!string.IsNullOrEmpty(StaffCheck))
                    {
                        Schedules = Schedules.Where(s => (s.StaffCheck != null && s.StaffCheck.ToLower().Contains(StaffCheck.ToLower().Trim()))).ToList();
                    }
                    if (!string.IsNullOrEmpty(Event))
                    {
                        Schedules = Schedules.Where(s => (s.Event != null && s.Event.ToLower().Contains(Event.ToLower().Trim()))).ToList();
                    }
                }
                var query = Schedules as IEnumerable<Schedule>;

                var pagedSchedules = query
                           .Skip((page - 1) * pagesize)
                           .Take(pagesize)
                           .ToList();

                Schedules = pagedSchedules;
                
                TotalCount = query.Count();
                PageTotal = (int)Math.Ceiling(query.Count() / (double)pagesize);
                CurrentPage = page;
                PageSize = pagesize;
            }
            else
            {
                if (scheduleResult != null && scheduleResult.Status == -4)
                {
                    Error = "Error system";
                }
                else
                {
                    Error = scheduleResult?.Message;
                }
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
            if (Schedule.From > Schedule.To)
            {
                Error = "Time From not great than or equal time To";
                await OnGetAsync();
                return Page();
            }
            if (Schedule.Date < DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(1))))
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
