using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BY.Data.Models;
using BY.Business;

namespace BY.RazorWebApp.Pages.BookingPage
{
    public class IndexModel : PageModel
    {
        private readonly IBookingBusiness _bookingBusiness;

        public IndexModel()
        {
            _bookingBusiness ??= new BookingBusiness();
        }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10; // Number of records per page
        public int TotalPages { get; set; }

        public IList<Booking> Bookings { get; set; } = new List<Booking>();

        [BindProperty]
        public string SearchStartDate { get; set; } = string.Empty;
        [BindProperty]
        public string SearchCreateDate { get; set; } = string.Empty;
        [BindProperty]
        public string SearchName { get; set; } = string.Empty;
        [BindProperty]
        public string SearchPaymentStatus { get; set; } = string.Empty;

        public async Task OnGetAsync(int currentPage = 1, string searchName = "", string searchCreateDate = "", string searchStartDate = "", string searchPaymentStatus = "")
        {
            CurrentPage = currentPage;
            SearchName = searchName;
            SearchCreateDate = searchCreateDate;
            SearchStartDate = searchStartDate;
            SearchPaymentStatus = searchPaymentStatus;

            var bookingResult = await _bookingBusiness.GetALlBooking();
            if (bookingResult.Status > 0 && bookingResult.Data != null)
            {
                var bookings = (List<Booking>)bookingResult.Data;

                if (!string.IsNullOrEmpty(SearchName))
                {
                    bookings = bookings.Where(b =>
                        (b.Customer?.Name?.ToLower().StartsWith(SearchName.ToLower()) ?? false) ||
                        (b.Customer?.Name?.ToLower().Contains(" " + SearchName.ToLower()) ?? false)
                    ).ToList();
                }
                if (!string.IsNullOrEmpty(SearchCreateDate))
                {
                    bookings = bookings.Where(b =>
                        b.CreateDate.HasValue && b.CreateDate.Value.ToString("dd-MM-yyyy").StartsWith(SearchCreateDate)
                    ).ToList();
                }
                if (!string.IsNullOrEmpty(SearchStartDate))
                {
                    bookings = bookings.Where(b =>
                        b.StartDate.HasValue && b.StartDate.Value.ToString("dd-MM-yyyy").StartsWith(SearchStartDate)
                    ).ToList();
                }
                if (!string.IsNullOrEmpty(SearchPaymentStatus))
                {
                    bookings = bookings.Where(b =>
                        b.PaymentStatus?.ToLower().StartsWith(SearchPaymentStatus.ToLower()) ?? false
                    ).ToList();
                }

                int totalCount = bookings.Count;
                TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

                Bookings = bookings
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
        }
    }
}
