using BY.Business;
using BY.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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


        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10; // Number of records per page
        public int TotalPages { get; set; }
        [BindProperty]
        public DateTime CheckIn { get; set; } = DateTime.Now;
        [BindProperty]
        public int Price { get; set; } = 0;
        public string Message { get; set; } = default;
        [BindProperty]
        public BookingDetail BookingDetail { get; set; }
        public List<BookingDetail>? BookingDetails { get; set; }
        public async Task OnGetAsync()
        {
            await GetBookingDetails();
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

        private async Task GetBookingDetails(int currentPage = 1, DateTime checkin = new DateTime(), int price = 0)
        {
            checkin = CheckIn;
            price = Price;
            var result = await _bookingDetailBusiness.GetAllBookingDetail();
            if (result.Status > 0 && result.Data != null)
            {
                var bookingDetails = result.Data as List<BookingDetail>;
                if(checkin != null)
                {
                    bookingDetails = bookingDetails.Where(x => x.CheckInDate < checkin).ToList();
                }
                if(price >= 0)
                {
                    bookingDetails = bookingDetails.Where(x => x.Price >= price).ToList();
                }
                int totalCount = bookingDetails.Count;
                TotalPages = (int)System.Math.Ceiling(totalCount / (double)PageSize);

                CurrentPage = CurrentPage > TotalPages ? TotalPages : CurrentPage;
                CurrentPage = CurrentPage < 1 ? 1 : CurrentPage;

                BookingDetails = bookingDetails
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
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
