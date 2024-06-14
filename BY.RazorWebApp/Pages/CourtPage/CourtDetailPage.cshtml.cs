using BY.Business;
using BY.Business.Base;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BY.RazorWebApp.Pages.CourtPage
{
    public class CourtDetailPageModel : PageModel
    {
        private readonly ICourtBusiness _businessResult;

        // Constructor with dependency injection
    /*    public CourtDetailPageModel(ICourtBusiness businessResult)
        {
            _businessResult = businessResult;
        }*/
        public CourtDetailPageModel()
        {
            _businessResult = new CourtBusiness();
        }

        [BindProperty]
        public Court Court { get; set; } = new Court();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var result = await LoadCourtDetailsAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            Court = result;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var result = await LoadCourtDetailsAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            Court = result;
            return Page();
        }

        private async Task<Court> LoadCourtDetailsAsync(int id)
        {
            var result = await _businessResult.GetById(id);

            if (result != null)
            {
                return result.Data as Court;
            }

            return null;
        }
    }
}
