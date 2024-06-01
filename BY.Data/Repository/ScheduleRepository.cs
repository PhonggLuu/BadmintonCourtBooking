using BY.Data.Base;
using BY.Data.Models;

namespace BY.Data.Repository
{
    public class ScheduleRepository : GenericRepository<Schedule>
    {
        public ScheduleRepository()
        {
        }
        public ScheduleRepository(Net1704_221_2_BYContext unitOfWorkContext) => _context = unitOfWorkContext;
    }
}
