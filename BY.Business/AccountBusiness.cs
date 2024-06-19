using BY.Business.Base;
using BY.Common;
using BY.Data;
using BY.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BY.Business
{
     public interface IAccountBusiness
    {
        Task<BusinessResult> Login(string username, string password);
    }
    public class AccountBusiness : IAccountBusiness
    {
        private readonly UnitOfWork _unitOfWork;
        public AccountBusiness()
        {
            _unitOfWork = new UnitOfWork();
        }
        public async Task<BusinessResult> Login(string username, string password)
        {
            try
            {
                var customer = await _unitOfWork.AccountRepository.Login(username, password);
                if(customer != null)
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE,Const.SUCCESS_READ_MSG, customer);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
            }catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
    }
}
