using BY.Data.Base;
using BY.Data.Models;
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

    }
}
