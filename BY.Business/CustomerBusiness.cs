using BY.Business.Base;
using BY.Common;
using BY.Data;
using BY.Data.Currency;
using BY.Data.Models;
using System.Net.WebSockets;

namespace BY.Busniness
{
    public interface ICustomerBusiness
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(int id);
        Task<IBusinessResult> Save(Customer customer);
        Task<IBusinessResult> Update(Customer customer);
        Task<IBusinessResult> Delete(int id);
    }

    public class CustomerBusiness : ICustomerBusiness
    {
        //private CustomerDAO _DAO;
        private readonly UnitOfWork _unitOfWork;
        public CustomerBusiness()
        {
            //_DAO ??= dao;
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<IBusinessResult> Save(Customer customer)
        {
            try
            {
                //var result = await _DAO.CreateAsync(customer);
                _unitOfWork.CustomerRepository.PrepareCreate(customer);
                var result = await _unitOfWork.CustomerRepository.SaveAsync();
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

        public async Task<IBusinessResult> GetAll()
        {
            try
            {
                #region Business rule
                #endregion

                //var currencies = _DAO.GetAll();
                //var customers = await _DAO.GetAllAsync();
                var customers = await _unitOfWork.CustomerRepository.GetAllAsync();
                if (customers != null)
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, customers);
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
                #region
                #endregion

                //var customer = await _DAO.GetByIdAsync(id);
                var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
                if (customer == null)
                {
                    return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                else
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, customer);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> Update(Customer customer)
        {
            try
            {
                //int result = await _DAO.UpdateAsync(customer);
                _unitOfWork.CustomerRepository.PrepareUpdate(customer);
                var result = await _unitOfWork.CustomerRepository.SaveAsync();
                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
            }
            catch(Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> Delete(int id)
        {
            try
            {
                //var customer = await _DAO.GetByIdAsync(id);
                var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
                if (customer == null)
                {
                    return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                }
                else
                {
                    //var result = await _DAO.RemoveAsync(customer);
                    _unitOfWork.CustomerRepository.PrepareRemove(customer);
                    var result = await _unitOfWork.CustomerRepository.SaveAsync();
                    if (result > 0)
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
