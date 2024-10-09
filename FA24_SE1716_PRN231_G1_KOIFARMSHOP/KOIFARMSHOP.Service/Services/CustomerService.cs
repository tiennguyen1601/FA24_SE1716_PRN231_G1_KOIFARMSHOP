using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Data.DTO.CustomerDTO;
using KOIFARMSHOP.Data.Enums;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Data.Repository;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Service.Services.EmailServices;
using KOIFARMSHOP.Service.Services.Security;
using Services.Helper.VerifyCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace KOIFARMSHOP.Service.Services
{
    public interface ICustomerService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetByID(int id);
        Task<IBusinessResult> Save(Customer customer);
        Task<IBusinessResult> Register(CustomerRegisterReqModel customer);
        Task<IBusinessResult> ResendVerifyEmail(string email);
        Task<IBusinessResult> VerifyCustomer(CustomerVerifyReqModel customerVerifyReqModel);
        Task<IBusinessResult> DeleteByID(int id);
    }
    public class CustomerService : ICustomerService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly VerificationCodeCache verificationCodeCache;
        private readonly IEmailService _emailService;

        public CustomerService(VerificationCodeCache verificationCodeCache, IEmailService emailService)
        {
            this.verificationCodeCache = verificationCodeCache;
            _emailService = emailService;
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

        public async Task<IBusinessResult> Register(CustomerRegisterReqModel customer)
        {
            try
            {
                if (await checkEmailExisted(customer.Email))
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, "Email has already been used by another user", new Customer());
                }

                if (await checkUsernameExisted(customer.Username))
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, "Username has already been used by another user", new Customer());
                }

                if (await checkPhoneExisted(customer.Phone))
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, "Phone number has already been used by another user", new Customer());
                }

                Customer newCustomer = new Customer
                {
                    Username = customer.Username,
                    Password = PasswordHasher.HashPassword(customer.Password),
                    Name = customer.FullName,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address,
                    CreatedAt = DateTime.Now,
                    Status = StatusEnums.PendingVerified.ToString(),
                };

                await _unitOfWork.CustomerRepository.CreateAsync(newCustomer);
                var newOtp = GenerateOTP();

                verificationCodeCache.Put(newCustomer.Username, newOtp, 5);

                await _emailService.SendRegistrationEmail(newCustomer.Name, newCustomer.Email, newOtp);

                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, newCustomer);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<bool> checkUsernameExisted(string username)
        {
            return (await _unitOfWork.CustomerRepository.IsExistedByUsername(username) ||
                await _unitOfWork.StaffRepository.IsExistedByUsername(username));
        }

        public async Task<bool> checkEmailExisted(string email)
        {
            return (await _unitOfWork.CustomerRepository.IsExistedByEmail(email) ||
                await _unitOfWork.StaffRepository.IsExistedByEmail(email));
        }

        public async Task<bool> checkPhoneExisted(string phone)
        {
            return (await _unitOfWork.CustomerRepository.IsExistedByPhone(phone) ||
                await _unitOfWork.StaffRepository.IsExistedByPhone(phone));
        }

        public string GenerateOTP()
        {
            var otp = new Random().Next(100000, 999999).ToString();

            return otp;
        }

        public async Task<IBusinessResult> VerifyCustomer(CustomerVerifyReqModel customerVerifyReqModel)
        {
            try
            {
                var currCustomer = await _unitOfWork.CustomerRepository.GetCustomerByEmail(customerVerifyReqModel.email);

                if (currCustomer == null) return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Customer());

                if (currCustomer.Status.Equals(StatusEnums.Active.ToString())) return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_INVALID_STATUS_MSG, new Customer());

                var otp = verificationCodeCache.Get(currCustomer.Username);

                if (otp == null || !otp.Equals(customerVerifyReqModel.otp))
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_INVALID_OTP_MSG, new Customer());
                }

                currCustomer.Status = StatusEnums.Active.ToString();

                await _unitOfWork.CustomerRepository.UpdateAsync(currCustomer);

                return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, currCustomer);
            }catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IBusinessResult> ResendVerifyEmail(string email)
        {
            try
            {
                var currCustomer = await _unitOfWork.CustomerRepository.GetCustomerByEmail(email);

                if (currCustomer == null) return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Customer());

                if (currCustomer.Status.Equals(StatusEnums.Active.ToString())) return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_INVALID_STATUS_MSG, new Customer());

                var newOtp = GenerateOTP();
                
                verificationCodeCache.Put(currCustomer.Username, newOtp, 5);

                await _emailService.ResendVerificationEmail(currCustomer.Name, currCustomer.Email, newOtp);

                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, currCustomer);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
    }
}
