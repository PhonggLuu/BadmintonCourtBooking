using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class CourtDetailModel : PageModel
    {
        private readonly ICourtBusiness _courtBusiness;

        public CourtDetailModel()
        {
            _courtBusiness ??= new CourtBusiness();
        }
        [BindProperty]
        public Court Court { get; set; } = default!;

         public async Task<IActionResult> OnGetAsync(int id)
        {
            var court = await _courtBusiness.GetById(id);
            if (court != null && court.Status > 0)
            {
                if (court.Data != null)
                {
                    Court = court.Data as Court;
                }
            }
            return Page();
        }
    }
}
