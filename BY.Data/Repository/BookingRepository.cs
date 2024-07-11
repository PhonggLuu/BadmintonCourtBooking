using BY.Data.Base;
using BY.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BY.Data.Repository
{
    public class BookingRepository : GenericRepository<Booking>
    {
        public BookingRepository()
        {
        }
        public BookingRepository(Net1704_221_2_BYContext unitOfWorkContext) => _context = unitOfWorkContext;

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Set<Booking>().Include(c=>c.Customer).ToListAsync();
        }
        
    }
}
