using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BY.RazorWebApp.Pages.BookingDetailPage
{
    public class BookingDetailModel : PageModel
    {
        private readonly IBookingDetailBusiness _business = new BookingDetailBusiness();
        public string Message { get; set; } = default;
        [BindProperty]
        public BookingDetail BookingDetail { get; set; }
        public List<BookingDetail>? BookingDetails { get; set; }
        public void OnGet()
        {
            BookingDetails = GetBookingDetails();
        }
        public IActionResult OnPost()
        {
            AddBookingDetail();
            return RedirectToPage();
        }

        private List<BookingDetail> GetBookingDetails()
        {
            var result = _business.GetAllBookingDetail().Result;
            if (result.Status > 0 && result.Data != null)
            {
                var bookingDetails = result.Data as List<BookingDetail>;
                return bookingDetails;
            }
            return new List<BookingDetail>();
        }

        private void AddBookingDetail()
        {
            var result = _business.CreateBookingDetail(BookingDetail);
            if (result != null)
            {
                Message = result.Result.Message;
            }
            else
            {
                Message = "Error System";
            }
        }
    }
}
