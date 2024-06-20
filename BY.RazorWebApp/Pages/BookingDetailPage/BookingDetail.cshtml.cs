using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BY.RazorWebApp.Pages.BookingDetailPage
{
    public class BookingDetailModel : PageModel
    {
        private readonly IBookingDetailBusiness _bookingDetailBusiness = new BookingDetailBusiness();
        private readonly IBookingBusiness _bookingBusiness = new BookingBusiness();
        private readonly IScheduleBusiness _scheduleBusiness = new ScheduleBusiness();
        public string Message { get; set; } = default;
        [BindProperty]
        public BookingDetail BookingDetail { get; set; }
        public List<BookingDetail>? BookingDetails { get; set; }
        public async Task OnGetAsync()
        {
            BookingDetails = await GetBookingDetails();
        }
        public IActionResult OnPost()
        {
            AddBookingDetail();
            return RedirectToPage();
        } 
        public async Task<IActionResult> OnPostDelete(int id)
        {
            var bookingDetail = await _bookingDetailBusiness.GetBookingDetailById(id);
            if (bookingDetail == null)
            {
                return NotFound();
            }
            else
            {
                var result = _bookingDetailBusiness.DeleteBookingDetail(id);
                if(result != null)
                {
                    Message = "delete successful";
                }
                else
                {
                    Message = "Error System";
                } 
                    
            }
            return RedirectToPage();
        }

        private async Task<List<BookingDetail>> GetBookingDetails()
        {
            var result = await _bookingDetailBusiness.GetAllBookingDetail();
            if (result.Status > 0 && result.Data != null)
            {
                var bookingDetails = result.Data as List<BookingDetail>;
                return bookingDetails;
            }
            return new List<BookingDetail>();
        }

        private void AddBookingDetail()
        {
            var result = _bookingDetailBusiness.CreateBookingDetail(BookingDetail);
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
