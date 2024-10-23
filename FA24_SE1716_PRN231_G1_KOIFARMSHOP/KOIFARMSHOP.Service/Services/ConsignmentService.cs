using AutoMapper;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Service.Services.JWTService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Service.Services
{
    public interface IConsignmentService
    {
        Task<IBusinessResult> GetAll(string token);
        Task<IBusinessResult> GetByID(int id);
        Task<IBusinessResult> Save(Consignment consignment, string token);
        Task<IBusinessResult> DeleteByID(int id);
    }
    public class ConsignmentService : IConsignmentService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IJWTService _jwtService;
        private readonly IMapper _mapper;
        public ConsignmentService(UnitOfWork unitOfWork, IMapper mapper, IJWTService jWTService)
        {
            _unitOfWork ??= new UnitOfWork();
            _jwtService = jWTService;
        }
        public async Task<IBusinessResult> GetAll(string token)
        {
            var userIdString = _jwtService.decodeToken(token, "userid");
            if (!int.TryParse(userIdString, out int userId))
            {
                return new BusinessResult(Const.FAIL_CREATE_CODE, "Invalid user ID.", null);
            }
            var list = await _unitOfWork.ConsignmentRepository.GetAllDetail(userId);
            //var filteredList = list.Where(c => c.Status != "Deleted").ToList();
            if (list == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Consignment>());
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
            var list = await _unitOfWork.ConsignmentRepository.GetByIdDetail(id);
            if (list == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Consignment>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, list);
            }

        }
        public async Task<IBusinessResult> Save(Consignment consignment, string token)
        {
            var userIdString = _jwtService.decodeToken(token, "userid");
            if (!int.TryParse(userIdString, out int userId))
            {
                return new BusinessResult(Const.FAIL_CREATE_CODE, "Invalid user ID.", null);
            }
            try
            {
                int result = -1;
                var consignmentTmp = await _unitOfWork.ConsignmentRepository.GetByIdAsync(consignment.ConsignmentId);

                if (consignmentTmp != null)
                {
                    #region Business Rule
                    #endregion Business Rule

                    result = await _unitOfWork.ConsignmentRepository.UpdateAsync(consignment);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, new List<Consignment>());
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, consignment);
                    }
                }
                else
                {
                    consignment.CustomerId = userId;
                    consignment.CreatedAt = DateTime.Now;
                    consignment.Status = "Pending";
                    result = await _unitOfWork.ConsignmentRepository.CreateAsync(consignment);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, new List<Consignment>());
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, consignment);
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
                var consignmentById = await _unitOfWork.ConsignmentRepository.GetByIdAsync(id);

                if (consignmentById == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Consignment());
                }

                else
                {
                    var result = await _unitOfWork.ConsignmentRepository.Delete(consignmentById);

                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, consignmentById);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, consignmentById);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }





    }
}
