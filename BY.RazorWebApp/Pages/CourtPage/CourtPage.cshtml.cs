using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BY.RazorWebApp.Pages.CourtPage
{
    public class CourtPageModel : PageModel
    {
        private readonly ICourtBusiness _courtBusiness = new CourtBusiness();
        public string? Error { get; set; }

        public List<Court>? Courts { get; private set; }
        public async Task OnGetAsync()
        {
            if (!ModelState.IsValid)
            {
                var result = await _courtBusiness.GetAllCourt();
                if (result != null || result?.Status == 1)
                {
                    Courts = result.Data as List<Court>;
                }
                else
                {
                    Error = result?.Message;
                }
            }
        }
    }
}
