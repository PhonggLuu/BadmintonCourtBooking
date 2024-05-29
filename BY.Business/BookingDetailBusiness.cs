using BY.Business.Base;
using BY.Common;
using BY.Data;
using BY.Data.Models;

namespace BY.Business
{
    public interface IBookingDetailBusiness
    {
        Task<BusinessResult> GetAllBookingDetail();
        Task<BusinessResult> CreateBookingDetail(BookingDetail BookingDetail);
        Task<BusinessResult> UpdateBookingDetail(BookingDetail bookingDetail);
        Task<BusinessResult> DeleteBookingDetail(BookingDetail bookingDetail);
        Task<BusinessResult> GetBookingDetailById(int bookingDetailId);
    }
    public class BookingDetailBusiness : IBookingDetailBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public BookingDetailBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<BusinessResult> CreateBookingDetail(BookingDetail bookingDetail)
        {
            try
            {
                var result = await _unitOfWork.BookingDetailRepository.CreateAsync(bookingDetail);
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

        public async Task<BusinessResult> DeleteBookingDetail(BookingDetail bookingDetail)
        {
            try
            {
                var result = await _unitOfWork.BookingDetailRepository.RemoveAsync(bookingDetail);
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

        public async Task<BusinessResult> GetAllBookingDetail()
        {
            try
            {
                var result = await _unitOfWork.BookingDetailRepository.GetAllAsync();
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



        public async Task<BusinessResult> GetBookingDetailById(int bookingDetailId)
        {
            try
            {
                var result = await _unitOfWork.BookingDetailRepository.GetByIdAsync(bookingDetailId);
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

        public async Task<BusinessResult> UpdateBookingDetail(BookingDetail BookingDetail)
        {

            try
            {
                var result = await _unitOfWork.BookingDetailRepository.UpdateAsync(BookingDetail);
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