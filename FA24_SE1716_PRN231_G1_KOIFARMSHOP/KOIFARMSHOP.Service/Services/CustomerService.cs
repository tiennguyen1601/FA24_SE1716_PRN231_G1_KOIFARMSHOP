using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Service.Services
{
    public interface ICustomerService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetByID(int id);
        Task<IBusinessResult> Save(Customer customer);
        Task<IBusinessResult> DeleteByID(int id);
    }
    public class CustomerService : ICustomerService
    {
        private readonly UnitOfWork _unitOfWork;
        public CustomerService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<IBusinessResult> GetAll()
        {
            var list = await _unitOfWork.CustomerRepository.GetAllAsync();
            if (list == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Customer>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, list);
            }
        }
        public async Task<IBusinessResult> GetByID(int id)
        {
            #region Business rule
            #endregion
            var list = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
            if (list == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Animal>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, list);
            }

        }
        public async Task<IBusinessResult> Save(Customer customer)
        {
            try
            {
                int result = -1;
                var customerTmp = await _unitOfWork.CustomerRepository.GetByIdAsync(customer.CustomerId);
                if (customerTmp != null)
                {
                    #region Business Rule
                    #endregion Business Rule

                    result = await _unitOfWork.CustomerRepository.UpdateAsync(customer);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, new List<Animal>());
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, customer);
                    }
                }
                else
                {
                    result = await _unitOfWork.CustomerRepository.CreateAsync(customer);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, new List<Customer>());
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, customer);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IBusinessResult> DeleteByID(int id)
        {
            try
            {
                var customerlById = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
                if (customerlById == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Customer());
                }
                else
                {
                    var result = await _unitOfWork.CustomerRepository.RemoveAsync(customerlById);
                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, customerlById);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, customerlById);
                    }
                }
            }
            catch (Exception ex) { return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString()); }
        }
    }
}
