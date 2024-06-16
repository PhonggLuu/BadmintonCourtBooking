using BY.Business.Base;
using BY.Data.DAO;
using BY.Common;
using BY.Data.Models;
using BY.Data;
using System.Net.Http;

namespace BY.Business
{
    public interface IBookingBusiness
    {
        Task<BusinessResult> GetALlBooking();
        Task<BusinessResult> CreateBooking(Booking booking);
        Task<BusinessResult> UpdateBooking(Booking booking);
        Task<BusinessResult> DeleteBooking(Booking booking);
        Task<BusinessResult> GetBookingById(int idBooking);

    }
    public class BookingBusiness : IBookingBusiness
    {
        //private readonly BookingDAO _unitOfWork.BookingRepository;
        private readonly UnitOfWork _unitOfWork;
        public BookingBusiness()
        {
            //_unitOfWork.BookingRepository = new BookingDAO();
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<BusinessResult> CreateBooking(Booking booking)
        {
            try
            {
                var result = await _unitOfWork.BookingRepositoty.CreateAsync(booking);
                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
            }
            catch (Exception e)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, e.ToString());
            }
        }

        public async Task<BusinessResult> DeleteBooking(Booking booking)
        {
            try
            {
                var result = await _unitOfWork.BookingRepositoty.RemoveAsync(booking);
                if (result)
                {
                    return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                }
            }
            catch (Exception e)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, e.ToString());
            }
        }

        public async Task<BusinessResult> GetALlBooking()
        {
            try
            {
                var result = await _unitOfWork.BookingRepositoty.GetAllAsync();
                if (result != null)
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
            }
            catch (Exception e)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, e.ToString());
            }
        }

        public async Task<BusinessResult> GetBookingById(int idBooking)
        {
            try
            {
                var result = await _unitOfWork.BookingRepositoty.GetByIdAsync(idBooking);
                if (result != null)
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
            }
            catch (Exception e)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, e.ToString());
            }
        }

        public async Task<BusinessResult> UpdateBooking(Booking booking)
        {
            try
            {
                var result = await _unitOfWork.BookingRepositoty.UpdateAsync(booking);
                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
            }
            catch (Exception e)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, e.ToString());
            }
        }
    }
}
