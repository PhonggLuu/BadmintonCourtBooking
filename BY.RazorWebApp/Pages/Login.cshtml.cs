using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MyApp.Namespace
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Error { get; set; } = string.Empty;
        private readonly IAccountBusiness _accountBusiness;
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public LoginModel()
        {
            _accountBusiness = new AccountBusiness();
        }
        public async Task<IActionResult> OnPost()
        {
            var result = await _accountBusiness.Login(Username, Password);
            if(result.Status > 0)
            {
                var customer = result.Data as Customer;
                HttpContext.Session.SetString("customer", JsonConvert.SerializeObject(customer));
               return RedirectToPage("/Index");
            }
            else
            {
                Error = "Username or password invalid";
            }
            return Page();
        }
    }
}
