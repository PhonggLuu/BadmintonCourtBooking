using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BY.Data.Models;

namespace BY.RazorWebApp.Pages.BookingDetailPage
{
    public class IndexModel : PageModel
    {
        private readonly BY.Data.Models.Net1704_221_2_BYContext _context;

        public IndexModel(BY.Data.Models.Net1704_221_2_BYContext context)
        {
            _context = context;
        }

        public IList<BookingDetail> BookingDetail { get;set; } = default!;

        public async Task OnGetAsync()
        {
            BookingDetail = await _context.BookingDetails
                .Include(b => b.Booking)
                .Include(b => b.Schedule).ToListAsync();
        }
    }
}
