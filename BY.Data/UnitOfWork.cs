using BY.Data.Models;
using BY.Data.Repository;

namespace BY.Data
{
    public class UnitOfWork
    {
        private Net1704_221_2_BYContext _unitOfWorkContext;
        private ScheduleRepository _scheduleRepository;
        private BookingRepositoty _bookingRepositoty;
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
        public BookingRepositoty BookingRepositoty
        {
            get
            {
                return _bookingRepositoty ??= new Repository.BookingRepositoty();
            }
        }
    }
}
