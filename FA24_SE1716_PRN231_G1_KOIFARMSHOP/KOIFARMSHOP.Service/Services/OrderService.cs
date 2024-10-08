using AutoMapper;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Data.DTO.OrderDTO;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;

namespace KOIFARMSHOP.Service.Services
{
    public interface IOrderService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetByID(int id);
        Task<IBusinessResult> Save(Order order);
        Task<IBusinessResult> DeleteByID(int id);
    }
    public class OrderService : IOrderService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public OrderService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<IBusinessResult> GetAll()
        {
            var orders = await _unitOfWork.OrderRepository.GetAllDetail();

            if (orders == null || !orders.Any())
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<OrderResponseModel>());
            }

            var orderResponseList = _mapper.Map<List<OrderResponseModel>>(orders);

            var orderBuyRequestModels = orderResponseList.Select(order => new OrderResponseModel
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                PromotionId = order.PromotionId,
                ShippingAddress = order.ShippingAddress,
                DeliveryMethod = order.DeliveryMethod,
                PaymentStatus = order.PaymentStatus,
                Vat = order.Vat,
                TotalAmountVat = order.TotalAmountVat,
                Status = order.Status,
                CustomerName = order.CustomerName, 
                OrderDetails = order.OrderDetails
            }).ToList();

            return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, orderBuyRequestModels);
        }



        public async Task<IBusinessResult> GetByID(int id)
        {
            #region Business rule
            #endregion
            var list = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (list == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Order>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, list);
            }

        }
        public async Task<IBusinessResult> Save(Order order)
        {
            try
            {
                int result = -1;
                var orderTmp = await _unitOfWork.OrderRepository.GetByIdAsync(order.OrderId);
                if (orderTmp != null)
                {
                    #region Business Rule
                    #endregion Business Rule

                    result = await _unitOfWork.OrderRepository.UpdateAsync(order);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, new List<Order>());
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, order);
                    }
                }
                else
                {
                    result = await _unitOfWork.OrderRepository.CreateAsync(order);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, new List<Order>());
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, order);
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
                var orderById = await _unitOfWork.OrderRepository.GetByIdAsync(id);
                if (orderById == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Order());
                }
                else
                {
                    var result = await _unitOfWork.OrderRepository.RemoveAsync(orderById);
                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, orderById);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, orderById);
                    }
                }
            }
            catch (Exception ex) { return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString()); }
        }

    }
}