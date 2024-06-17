using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BY.RazorWebApp.Pages.CustomerPage
{
    public class SearchingModel : PageModel
    {
        public string SearchString { get; set; } = default!;
        public void OnGetSearching()
        {
        }
    }
}
