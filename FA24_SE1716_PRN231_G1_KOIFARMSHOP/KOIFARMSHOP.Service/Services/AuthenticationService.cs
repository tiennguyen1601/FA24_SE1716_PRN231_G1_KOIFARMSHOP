using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Data.DTO.LoginDTO;
using KOIFARMSHOP.Data.Enums;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Data.Repository;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Service.Services.JWTService;
using KOIFARMSHOP.Service.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Service.Services
{
    public interface IAuthenticationService
    {
        Task<IBusinessResult> Login(LoginReqModel userLoginReqModel);

    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IJWTService _jWTService;

        public AuthenticationService(IJWTService jWTService)
        {
            _unitOfWork ??= new UnitOfWork();
            _jWTService = jWTService;
        }
        public async Task<IBusinessResult> Login(LoginReqModel userLoginReqModel)
        {
            var currCustomer = await _unitOfWork.CustomerRepository.GetCustomerByUsername(userLoginReqModel.Username);
            var currStaff = await _unitOfWork.StaffRepository.GetStaffByUsername(userLoginReqModel.Username);

            if (currCustomer != null)
            {
                //if (currCustomer.Status.Equals(StatusEnums.Inactive.ToString())) throw new ApiException(HttpStatusCode.BadRequest, "This account has been deactivated");

                if (PasswordHasher.VerifyPassword(userLoginReqModel.Password, currCustomer.Password))
                {
                    var token = _jWTService.GenerateJWT(currCustomer);

                    var userLoginRes = new LoginResModel
                    {
                        UserId = currCustomer.CustomerId,
                        Username = currCustomer.Username,
                        Role = RoleEnums.Customer.ToString(),
                        Token = token,
                    };

                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_READ_MSG, userLoginRes);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, new LoginResModel());

                }
            }
            else if (currStaff != null)
            {
                if (PasswordHasher.VerifyPassword(userLoginReqModel.Password, currStaff.Password))
                {
                    var token = _jWTService.GenerateJWT(currStaff);

                    var userLoginRes = new LoginResModel
                    {
                        UserId = currStaff.StaffId,
                        Username = currStaff.Username,
                        Role = RoleEnums.Staff.ToString(),
                        Token = token,
                    };

                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_READ_MSG, userLoginRes);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, new LoginResModel());

                }
            }else
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new LoginResModel());
            }
        }
    }
}
