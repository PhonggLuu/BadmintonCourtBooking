using BY.Data.Base;
using BY.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BY.Data.Repository
{
    public class BookingDetailRepository :  GenericRepository<BookingDetail>
    {
        public BookingDetailRepository(){
            
        }

        public async Task<List<BookingDetail>> GetAllAsync()
        {
            return await _context.Set<BookingDetail>().Include(bd => bd.Booking).Include(bd => bd.Schedule).ToListAsync();
        }
    }
}