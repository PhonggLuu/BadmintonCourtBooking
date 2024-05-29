using BY.Data.Models;
using BY.Data.Repository;

namespace BY.Data
{
    public class UnitOfWork
    {
        private Net1704_221_2_BYContext _unitOfWorkContext;
        private ScheduleRepository _scheduleRepository;
        private BookingRepository _bookingRepository;
        private CustomerRepository _customerRepository;
        private BookingDetailRepository _bookingDetailRepository;
        private CourtRepository _courseRepository;
        public UnitOfWork()
        {

        }
        public CourtRepository courtRepository { get { return _courseRepository ??= new Repository.CourtRepository(); } }
        public ScheduleRepository ScheduleRepository
        {
            get
            {
                return _scheduleRepository ??= new Repository.ScheduleRepository();
            }
        }
        public BookingRepository BookingRepositoty
        {
            get
            {
                return _bookingRepository ??= new Repository.BookingRepository();
            }
        }
        public CustomerRepository CustomerRepository
        {
            get
            {
                return _customerRepository ??= new Repository.CustomerRepository();
            }
        }
        public BookingDetailRepository BookingDetailRepository
        {
            get
            {
                return _bookingDetailRepository ??= new Repository.BookingDetailRepository();
            }
        }
        public CourtRepository courtRepository { get { return _courseRepository ??= new Repository.CourtRepository(); } }
    }
}
