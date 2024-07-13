using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BY.RazorWebApp.Pages.BookingDetailPage
{
    public class BookingDetailModel : PageModel
    {
        private readonly IBookingDetailBusiness _bookingDetailBusiness;
        private readonly IBookingBusiness _bookingBusiness;
        private readonly IScheduleBusiness _scheduleBusiness;

        public BookingDetailModel()
        {
            _bookingDetailBusiness ??= new BookingDetailBusiness();
            _bookingBusiness ??= new BookingBusiness();
            _scheduleBusiness ??= new ScheduleBusiness();
        }
        DateTime checkIn = new DateTime();

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10; // Number of records per page
        public int TotalPages { get; set; }
        [BindProperty]
        public DateTime CheckIn { get; set; }
        [BindProperty]
        public int Price { get; set; }
        public string Message { get; set; } = default;
        [BindProperty]
        public BookingDetail BookingDetail { get; set; }
        public List<BookingDetail>? BookingDetails { get; set; } = new List<BookingDetail>();
        public async Task OnGetAsync(int currentPage = 1, DateTime? checkin = null, int price = 0)
        {
            await GetBookingDetails(currentPage, checkin, price);
        }
        public IActionResult OnPost()
        {
            AddBookingDetail();
            return RedirectToPage();
        } 
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var bookingDetail = await _bookingDetailBusiness.GetBookingDetailById(id);
            if (bookingDetail == null)
            {
                return NotFound();
            }
            else
            {
                var result = await _bookingDetailBusiness.DeleteBookingDetail(id);
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

        private async Task GetBookingDetails(int currentPage, DateTime? checkin, int price)
        {
            CheckIn = checkin ?? DateTime.Now;
            Price = price;
            CurrentPage = currentPage;
            var result = await _bookingDetailBusiness.GetAllBookingDetail();
            if (result.Status > 0 && result.Data != null)
            {
                var bookingDetails = result.Data as List<BookingDetail>;
              
                if(CheckIn != null)
                {
                    bookingDetails = bookingDetails.Where(x => x.CheckInDate < CheckIn).ToList();
                }
                if(Price > 0)
                {
                    bookingDetails = bookingDetails.Where(x => x.Price <= Price).ToList();
                }
                int totalCount = bookingDetails.Count;
                TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

                CurrentPage = CurrentPage > TotalPages ? TotalPages : CurrentPage;
                CurrentPage = CurrentPage < 1 ? 1 : CurrentPage;

                BookingDetails = bookingDetails
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }

            var bookings = (await _bookingBusiness.GetALlBooking()).Data as List<Booking>;
            if (bookings != null)
            {
                ViewData["BookingId"] = new SelectList(bookings, "BookingId", "BookingId");
            }
            var schedules = (await _scheduleBusiness.GetAllSchedule()).Data as List<Schedule>;
            if (schedules != null)
            {
                ViewData["ScheduleId"] = new SelectList(schedules, "ScheduleId", "ScheduleId");
            }
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
