using BY.Data.Base;
using BY.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BY.Data.Repository
{
    public class BookingDetailRepository :  GenericRepository<BookingDetail>
    {
        public BookingDetailRepository(){
            
        }

        public new async Task<List<BookingDetail>> GetAllAsync()
        {
            return await _context.Set<BookingDetail>().Include(bd => bd.Booking).Include(bd => bd.Schedule).ToListAsync();
        }

        //public async Task<(List<BookingDetail>, int)> GetByPaging(DateTime CheckinDate, bool Status, int PageIndex, int PageSize)
        //{
        //    var result = await GetAllAsync();
        //    if(result == null)
        //    {
        //        result = new List<BookingDetail>();
        //    }
        //    if(CheckinDate != null)
        //    {
        //        result = result.Where(c => c.CheckInDate.Value.Date == CheckinDate.Date).ToList();
        //    }
        //    result =  result.Where(c => c.Status == Status).ToList();

        //    int CountPage = (int)Math.Ceiling((double)result.Count / PageSize);
        //    if (PageIndex >= 1 && PageIndex <= CountPage)
        //    {
        //        result = result.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
        //    }
        //    else
        //    {
        //        result = result.Take(PageSize).ToList();
        //        PageIndex = 1;
        //    }
        //    return (result, CountPage);
        //}
    }
}