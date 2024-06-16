using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MyApp.Namespace
{
    public class PaymentModel : PageModel
    {
        private readonly IBookingBusiness _bookingBusiness;
        private readonly IScheduleBusiness _scheduleBusiness;
        public PaymentModel()
        {
            _bookingBusiness = new BookingBusiness();
            _scheduleBusiness = new ScheduleBusiness();
        }
        [BindProperty]
        public string? Error { get; set; }
        [BindProperty]
        public bool Success { get; set; } = false;
        [BindProperty]
        public List<Cart> Carts { get; set; } = default!;

        public Customer Customer { get; set; } = new Customer();
        public void OnGet()
        {
            Success = false;
            string sessionCart = HttpContext.Session.GetString("cart");
            List<Cart>? carts = new List<Cart>();
            if (!string.IsNullOrEmpty(sessionCart))
            {
                carts = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
            }
            Carts = carts;
        }

        public async Task<IActionResult> OnPost()
        {
            OnGet();
            Customer.Name = Request.Form["name"];
            Customer.Email = Request.Form["email"];
            Customer.Address = Request.Form["address"];
            Customer.Phone = Request.Form["phone"];
            var result = await _bookingBusiness.CreateBookingV2(Carts, Customer);
            if (result.Status > 0)
            {
                Success = true;
                HttpContext.Session.SetString("cart", "");
            }
            else
            {
                Error = result.Message;
            }
            return Page();
        }

        public IActionResult OnPostDeleteCart(string cartId)
        {
            if (int.TryParse(cartId, out int index))
            {
                string sessionCart = HttpContext.Session.GetString("cart");
                List<Cart>? carts = new List<Cart>();
                if (!string.IsNullOrEmpty(sessionCart))
                {
                    carts = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
                    for (int i = 0; i < carts.Count(); i++)
                    {
                        if (i == index)
                        {
                            carts.Remove(carts[i]);
                            break;
                        }
                    }
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(carts));
                }
            }      
            return RedirectToPage("/Payment");
        }
    }
}
