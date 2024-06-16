using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BY.RazorWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICourtBusiness _courtBusiness;

        [BindProperty]
        public List<Court>? Courts { get; set; } = new List<Court>();
        public IndexModel()
        {
            _courtBusiness = new CourtBusiness();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var courts = await _courtBusiness.GetTop10Court();
            if (courts.Status > 0 && courts.Data != null)
            {
                Courts = courts.Data as List<Court>;
            }
            return Page();
        }
    }
}
