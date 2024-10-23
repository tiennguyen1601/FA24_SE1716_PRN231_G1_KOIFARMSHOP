using AutoMapper;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Data.DTO.PaymentDTO;
using KOIFARMSHOP.Data.Enums;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Service.Services
{
    public interface IPaymentService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(int id);
        Task<IBusinessResult> Save(PaymentCreateRequestModel payment);
        Task<IBusinessResult> DeleteById(int id);
    }

    public class PaymentService : IPaymentService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }

        public async Task<IBusinessResult> GetAll()
        {
            var list = await _unitOfWork.PaymentRepository.GetAllAsync();

            if (list == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Payment>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, list);
            }
        }   

        public async Task<IBusinessResult> GetById(int id)
        {
            var data = await _unitOfWork.PaymentRepository.GetByIdAsync(id);

            if (data == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Payment());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, data);
            }
        }

        public async Task<IBusinessResult> Save(PaymentCreateRequestModel payment)
        {
            try
            {
                int result = -1;
                var current = await _unitOfWork.PaymentRepository.GetByIdAsync(payment.PaymentId);
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(payment.OrderId.Value);

                if (current != null)
                {
                    current.Status = payment.Status;
                    current.TransactionId = payment.TransactionId;
                    current.UpdatedAt = DateTime.Now;
                    if (current.Status.Equals(PaymentEnums.Paid.ToString()))
                    {
                        current.PaymentDate = DateTime.Now;
                    }

                    result = await _unitOfWork.PaymentRepository.UpdateAsync(current);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, payment);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, payment);
                    }
                }
                else
                {
                    current = _mapper.Map<Payment>(payment);

                    current.CustomerId = order.CustomerId;
                    current.Status = PaymentEnums.Unpaid.ToString();
                    current.UpdatedAt = DateTime.Now;

                    result = await _unitOfWork.PaymentRepository.CreateAsync(current);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, payment);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, payment);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> DeleteById(int id)
        {
            try
            {
                var current = await _unitOfWork.PaymentRepository.GetByIdAsync(id);

                if (current != null)
                {
                    var result = await _unitOfWork.PaymentRepository.RemoveAsync(current);

                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, current);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, current);
                    }
                }
                else
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Payment());
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
