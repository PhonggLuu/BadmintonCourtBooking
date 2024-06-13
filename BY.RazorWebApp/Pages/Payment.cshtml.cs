using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class PaymentModel : PageModel
    {
        [BindProperty]
        public string? Error { get; set; }
        [BindProperty]
        public bool Success { get; set; } = true;
        public void OnGet()
        {
        }
    }
}
