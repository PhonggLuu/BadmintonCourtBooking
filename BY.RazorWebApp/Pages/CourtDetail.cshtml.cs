using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyApp.Namespace
{
    [IgnoreAntiforgeryToken]
    public class CourtDetailModel : PageModel
    {
        private readonly ICourtBusiness _courtBusiness;
        private readonly IScheduleBusiness _scheduleBusiness;

        public CourtDetailModel()
        {
            _courtBusiness ??= new CourtBusiness();
            _scheduleBusiness ??= new ScheduleBusiness();
        }
        [BindProperty]
        public Court Court { get; set; } = default!;

        public string? Error { get; set; } = default!;

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
        public async Task<IActionResult> OnPostAddBooking()
        {
            string sessionCart = HttpContext.Session.GetString("cart");
            List<Cart>? carts = new List<Cart>();
            if (!string.IsNullOrEmpty(sessionCart))
            {
                carts = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
            }
            if (!int.TryParse(Request.Form["courtId"], out int courtId))
            {
                Error = "Not found court Id";
                await OnGetAsync(Court.CourtId);
                return Page();
            }
            var courtResult = await _courtBusiness.GetById(courtId);
            if (courtResult == null || courtResult.Status <= 0 || courtResult.Data == null)
            {
                Error = "Not found court";
                await OnGetAsync(Court.CourtId);
                return Page();
            }
            var court = courtResult.Data as Court;
            if (court == null)
            {
                Error = "Not found court";
                await OnGetAsync(Court.CourtId);
                return Page();
            }
            if (!DateOnly.TryParse(Request.Form["datePlay"], out DateOnly datePlay))
            {
                Error = "Date play invalid";
                await OnGetAsync(Court.CourtId);
                return Page();
            }

            if (!TimeOnly.TryParse(Request.Form["time"], out TimeOnly timePlay))
            {
                Error = "time play invalid";
                await OnGetAsync(Court.CourtId);
                return Page();
            }
            var cart = new Cart()
            {
                CourtId = court.CourtId,
                CourtName = court.Name,
                Image = court.Image,
                DatePlay = datePlay,
                TimePlay = timePlay,
            };

            if (cart != null)
            {
                if (CheckExist(carts, court, datePlay, timePlay))
                {
                    Error = $"Date play: {datePlay.ToString("dd-MM-yyyy")} , Time play: {timePlay}, Court: {court.Name} already included in booking";
                    await OnGetAsync(Court.CourtId);
                    return Page();
                }
                carts.Add(cart);
            }
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(carts));
            return RedirectToPage("./Payment");
        }
        public async Task<IActionResult> OnPostGetTimeAsync(int courtId, DateOnly datePlay)
        {
            Dictionary<string, TimeOnly> timeValue = new Dictionary<string, TimeOnly>{
                { "05:00", new TimeOnly(5,00) },
                { "06:00", new TimeOnly(6,00) },
                { "07:00", new TimeOnly(7,00) },
                { "08:00", new TimeOnly(8,00) },
                { "09:00", new TimeOnly(9,00) },
                { "15:00", new TimeOnly(15,00) },
                { "16:00", new TimeOnly(16,00) },
                { "17:00", new TimeOnly(17,00) },
                { "18:00", new TimeOnly(18,00) },
                { "19:00", new TimeOnly(19,00) },
                { "20:00", new TimeOnly(20,00) },
                { "21:00", new TimeOnly(21,00) },
            };
            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            var result = await _scheduleBusiness.GetTimeCanPlay(courtId, datePlay);
            string html = "";

            if (result != null && result.Status > 0 && result.Data != null)
            {
                var timeBooked = result.Data as List<string>;
                if (timeBooked != null)
                {
                    foreach (string time in timeBooked)
                    {
                        if (timeValue.ContainsKey(time))
                        {
                            timeValue.Remove(time);
                        }
                    }
                }
            }
            foreach (var key in timeValue.Keys)
            {
                html += $"<option value='{timeValue[key]}'>{key}</option>";
            }
            var jsonResult = System.Text.Json.JsonSerializer.Serialize(html, jsonOptions);
            return Content(jsonResult, "application/json");
        }

        private bool CheckExist(List<Cart> carts, Court court, DateOnly datePlay, TimeOnly timePlay)
        {
            foreach (var item in carts)
            {
                if (item.CourtId == court.CourtId
                    && item.DatePlay == datePlay
                    && item.TimePlay == timePlay)
                {
                    return true;
                }
            }
            return false;
        }

        public  IActionResult OnGetAmounTicket()
        {
            string sessionCart = HttpContext.Session.GetString("cart");
            List<Cart>? carts = new List<Cart>();
            if (!string.IsNullOrEmpty(sessionCart))
            {
                carts = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
            }
            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            var jsonResult = System.Text.Json.JsonSerializer.Serialize(carts.Count, jsonOptions);
            return Content(jsonResult, "application/json");
        }
    }
}
