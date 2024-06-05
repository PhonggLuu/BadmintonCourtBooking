using BY.Business.Base;
using BY.Data.Models;
using BY.Common;
using BY.Data.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BY.Data;

namespace BY.Business
{
    public interface ICourtBusiness
    {
        Task<IBusinessResult> GetAllCourt();
        Task<IBusinessResult> GetById(int id);
        Task<IBusinessResult> Save(Court court);
        Task<IBusinessResult> Update(Court court);
        Task<IBusinessResult> Delete(int id);
    }

    public class CourtBusiness : ICourtBusiness
    {
       // private readonly CourtDAO _UnitOfWork.CourtRepository;
        private readonly UnitOfWork _UnitOfWork;

        public CourtBusiness()
        {  
            _UnitOfWork ??= new UnitOfWork();
        }
       

        public async Task<IBusinessResult> GetAllCourt()
        {
            try
            {
                var courts = await _UnitOfWork.courtRepository.GetAllAsync();
                if (courts != null)
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, courts);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetById(int id)
        {
            try
            {
                var court = await _UnitOfWork.courtRepository.GetByIdAsync(id);
                if (court != null)
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, court);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> Save(Court court)
        {
            try
            {
                _UnitOfWork.courtRepository.PrepareCreate(court);

                var result = await _UnitOfWork.courtRepository.CreateAsync(court);
                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> Update(Court court)
        {
            try
            {
                var result = await _UnitOfWork.courtRepository.UpdateAsync(court);
                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> Delete(int id)
        {
            try
            {
                var court = await _UnitOfWork.courtRepository.GetByIdAsync(id);
                if (court == null)
                {
                    return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                }
                else
                {
                    var result = await _UnitOfWork.courtRepository.RemoveAsync(court);
                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}



