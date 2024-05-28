using BY.Business.Base;
using BY.Data.DAO;
using BY.Common;
using BY.Data.Models;
using BY.Data;

namespace BY.Business
{
    public interface IScheduleBusiness
    {
        Task<BusinessResult> GetAllSchedule();
        Task<BusinessResult> CreateSchedule(Schedule schedule);
        Task<BusinessResult> UpdateSchedule(Schedule schedule);
        Task<BusinessResult> DeleteSchedule(Schedule schedule);
        Task<BusinessResult> GetScheduleById(int idSchedule);

    }
    public class ScheduleBusiness : IScheduleBusiness
    {
        //private readonly ScheduleDAO _unitOfWork.ScheduleRepository;
        private readonly UnitOfWork _unitOfWork;
        public ScheduleBusiness()
        {
            //_unitOfWork.ScheduleRepository = new ScheduleDAO();
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<BusinessResult> CreateSchedule(Schedule schedule)
        {
            try
            {
                var result = await _unitOfWork.ScheduleRepository.CreateAsync(schedule);
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

        public async Task<BusinessResult> DeleteSchedule(Schedule schedule)
        {
            try
            {
                var result = await _unitOfWork.ScheduleRepository.RemoveAsync(schedule);
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

        public async Task<BusinessResult> GetAllSchedule()
        {
            try
            {
                var result = await _unitOfWork.ScheduleRepository.GetAllAsync();
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

        public async Task<BusinessResult> GetScheduleById(int idSchedule)
        {
            try
            {
                var result = await _unitOfWork.ScheduleRepository.GetByIdAsync(idSchedule);
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

        public async Task<BusinessResult> UpdateSchedule(Schedule schedule)
        {
            try
            {
                var result = await _unitOfWork.ScheduleRepository.UpdateAsync(schedule);
                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
            }
            catch(Exception e)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, e.ToString());
            }
        }
    }
}
