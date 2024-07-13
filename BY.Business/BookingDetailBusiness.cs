using BY.Business.Base;
using BY.Common;
using BY.Data;
using BY.Data.Models;

namespace BY.Business
{
    public interface IBookingDetailBusiness
    {
        Task<IBusinessResult> GetAllBookingDetail();
        Task<IBusinessResult> CreateBookingDetail(BookingDetail BookingDetail);
        Task<IBusinessResult> UpdateBookingDetail(BookingDetail bookingDetail);
        Task<IBusinessResult> DeleteBookingDetail(int bookingDetailId);
        Task<IBusinessResult> GetBookingDetailById(int bookingDetailId);
    }
    public class BookingDetailBusiness : IBookingDetailBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public BookingDetailBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IBusinessResult> CreateBookingDetail(BookingDetail bookingDetail)
        {
            try
            {
                _unitOfWork.BookingDetailRepository.PrepareCreate(bookingDetail);
                var result = await _unitOfWork.BookingDetailRepository.SaveAsync();
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

        public async Task<IBusinessResult> DeleteBookingDetail(int bookingDetailId)
        {
            try
            {
                var bookingDetail = await _unitOfWork.BookingDetailRepository.GetByIdAsync(bookingDetailId);
                if (bookingDetail == null)
                {
                    return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                }
                _unitOfWork.BookingDetailRepository.PrepareRemove(bookingDetail);
                var result = await _unitOfWork.BookingDetailRepository.SaveAsync();
                if (result > 0)
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

        public async Task<IBusinessResult> GetAllBookingDetail()
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

        public async Task<IBusinessResult> GetBookingDetailById(int bookingDetailId)
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

        public async Task<IBusinessResult> UpdateBookingDetail(BookingDetail bookingDetail)
        {
            try
            {
                _unitOfWork.BookingDetailRepository.PrepareUpdate(bookingDetail);
                var result = await _unitOfWork.BookingDetailRepository.SaveAsync();
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