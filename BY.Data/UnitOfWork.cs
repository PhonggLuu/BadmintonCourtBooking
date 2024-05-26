using BY.Data.Models;
using BY.Data.Repository;

namespace BY.Data
{
    public class UnitOfWork
    {
        private Net1704_221_2_BYContext _unitOfWorkContext;
        private ScheduleRepository _scheduleRepository;

        public UnitOfWork()
        {

        }
        public ScheduleRepository ScheduleRepository
        {
            get
            {
                return _scheduleRepository ??= new Repository.ScheduleRepository();
            }
        }
    }
}
