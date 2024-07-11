using BY.Business.Base;
using BY.Common;
using BY.Data.Models;
using BY.Data;
using System.Net.Http;
using System.Transactions;

namespace BY.Business
{
    public interface IBookingBusiness
    {
        Task<BusinessResult> GetALlBooking();
        Task<BusinessResult> CreateBooking(Booking booking);
        Task<BusinessResult> UpdateBooking(Booking booking);
        Task<BusinessResult> DeleteBooking(Booking booking);
        Task<BusinessResult> GetBookingById(int idBooking);
        Task<BusinessResult> CreateBookingV2(List<Cart> carts, Customer customer);
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

        public async Task<BusinessResult> CreateBookingV2(List<Cart> carts, Customer customer)
        {

            if (carts.Count == 0)
            {
                return new BusinessResult(Const.FAIL_CREATE_CODE, $"You do not booking any court");
            }
            try
            {
                //get customer 
                var cus = _unitOfWork.CustomerRepository.FindUser(customer.Email, customer.Phone);

                List<BookingDetail> bookingDetails = new List<BookingDetail>();
                foreach (var cart in carts)
                {
                    if (cart.DatePlay < DateOnly.FromDateTime(DateTime.Now))
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, $"{cart.CourtName} - {cart.DatePlay.ToString("dd-MM-yyy")} : Date play invalid");
                    }
                    if (cart.DatePlay == DateOnly.FromDateTime(DateTime.Now) && cart.TimePlay < TimeOnly.FromDateTime(DateTime.Now))
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, $"{cart.CourtName} - {cart.DatePlay.ToString("dd-MM-yyy")} - {cart.TimePlay} : Time play invalid");
                    }
                    if (_unitOfWork.ScheduleRepository.ExistSchedule(cart.CourtId, cart.DatePlay, cart.TimePlay))
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, $"{cart.CourtName} - {cart.DatePlay.ToString("dd-MM-yyy")} - {cart.TimePlay} : booked by another customer");
                    }
                    var schedule = new Schedule
                    {
                        CourtId = cart.CourtId,
                        From = cart.TimePlay,
                        To = cart.TimePlay.AddHours(1),
                        Date = cart.DatePlay,
                        Price = 60,
                        IsBooked = false
                    };
                    var bookingDetail = new BookingDetail
                    {
                        Amount = 1,
                        Price = 60,
                        Schedule = schedule,
                    };
                    bookingDetails.Add(bookingDetail);
                }
                Booking booking;
                if (cus != null)
                {
                    booking = new Booking
                    {
                        CustomerId = cus.CustomerId,
                        TotalPrice = carts.Count * 60,
                        Status = true,
                        StartDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        PaymentMethod = "Credit Card",
                        PaymentStatus = "Pending",
                        Discount = 0,
                        Vat = 0,
                        BookingDetails = bookingDetails
                    };
                }
                else
                {
                    booking = new Booking
                    {
                        Customer = customer,
                        TotalPrice = carts.Count * 60,
                        Status = true,
                        StartDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        PaymentMethod = "Credit Card",
                        PaymentStatus = "Pending",
                        Discount = 0,
                        Vat = 0,
                        BookingDetails = bookingDetails
                    };
                }
                _unitOfWork.BookingRepositoty.PrepareCreate(booking);
                var result = await _unitOfWork.BookingRepositoty.SaveAsync();
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
