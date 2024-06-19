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
        private AccountRepository _accountRepository;
        public UnitOfWork()
        {

            _unitOfWorkContext ??= new Net1704_221_2_BYContext();
        }
        public UnitOfWork(Net1704_221_2_BYContext unitOfWorkContext)
        {
            _unitOfWorkContext = unitOfWorkContext;
        }
        public CourtRepository courtRepository { get { return _courseRepository ??= new Repository.CourtRepository(); } }
        public ScheduleRepository ScheduleRepository
        {
            get
            {
                return _scheduleRepository ??= new Repository.ScheduleRepository(_unitOfWorkContext);
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
        public CourtRepository CourtRepository
        {
            get
            {
                return _courseRepository ??= new Repository.CourtRepository();
            }
        }
        public  AccountRepository AccountRepository
        {
            get
            {
                return _accountRepository ??= new Repository.AccountRepository();
            }
        }
        ////TO-DO CODE HERE/////////////////

        #region Set transaction isolation levels

        /*
        Read Uncommitted: The lowest level of isolation, allows transactions to read uncommitted data from other transactions. This can lead to dirty reads and other issues.

        Read Committed: Transactions can only read data that has been committed by other transactions. This level avoids dirty reads but can still experience other isolation problems.

        Repeatable Read: Transactions can only read data that was committed before their execution, and all reads are repeatable. This prevents dirty reads and non-repeatable reads, but may still experience phantom reads.

        Serializable: The highest level of isolation, ensuring that transactions are completely isolated from one another. This can lead to increased lock contention, potentially hurting performance.

        Snapshot: This isolation level uses row versioning to avoid locks, providing consistency without impeding concurrency. 
         */

        public int SaveChangesWithTransaction()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _unitOfWorkContext.Database.BeginTransaction())
            {
                try
                {
                    result = _unitOfWorkContext.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _unitOfWorkContext.Database.BeginTransaction())
            {
                try
                {
                    result = await _unitOfWorkContext.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        #endregion
    }
}
