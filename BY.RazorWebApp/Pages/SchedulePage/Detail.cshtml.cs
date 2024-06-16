using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class DetailModel : PageModel
    {
        private readonly IScheduleBusiness _scheduleBusiness;

        public DetailModel()
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
