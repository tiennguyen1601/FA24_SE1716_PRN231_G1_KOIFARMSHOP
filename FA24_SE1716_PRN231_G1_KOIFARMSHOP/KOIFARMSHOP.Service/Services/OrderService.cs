using AutoMapper;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Data.DTO.OrderDTO;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Service.Services.JWTService;
namespace KOIFARMSHOP.Service.Services
{
    public interface IOrderService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetByID(int id);
        Task<IBusinessResult> Save(OrderRequestModel orderRequest, List<OrderDetailRequest> orderDetails, string token);
        Task<IBusinessResult> DeleteByID(int id);
    }
    public class OrderService : IOrderService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJWTService _jwtService;

        public OrderService(UnitOfWork unitOfWork, IMapper mapper, IJWTService jWTService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtService = jWTService;

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
                PromotionId = order.PromotionId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                ShippingAddress = order.ShippingAddress,
                DeliveryMethod = order.DeliveryMethod,
                PaymentStatus = order.PaymentStatus,
                Vat = order.Vat,
                TotalAmountVat = order.TotalAmountVat,
                Status = order.Status,
                CustomerName = order.CustomerName,
                PromotionTitle = order.PromotionTitle,
                OrderDetails = order.OrderDetails
            }).ToList().Where(m => m.Status.Equals("Active"));

            return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, orderBuyRequestModels);
        }



        public async Task<IBusinessResult> GetByID(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdDetail(id);

            if (order == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, null);
            }

            var orderResponse = _mapper.Map<OrderResponseModel>(order);

            return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, orderResponse);
        }

        public async Task<IBusinessResult> Save(OrderRequestModel orderRequest, List<OrderDetailRequest> orderDetails, string token)
        {
            var userIdString = _jwtService.decodeToken(token, "userid");
            if (!int.TryParse(userIdString, out int userId))
            {
                return new BusinessResult(Const.FAIL_CREATE_CODE, "Invalid user ID.", null);
            }

            var task = await _unitOfWork.CustomerRepository.GetByIdAsync(userId);
            try
            {
                if (orderRequest == null || orderDetails == null || !orderDetails.Any())
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, "Invalid request.", null);
                }

                var order = _mapper.Map<Order>(orderRequest);
                order.CustomerId = userId;
                order.OrderDetails = _mapper.Map<List<OrderDetail>>(orderDetails);

                if (order.OrderId > 0)
                {
                    var existingOrder = await _unitOfWork.OrderRepository.GetByIdAsync(order.OrderId);
                    if (existingOrder != null)
                    {
                        _mapper.Map(order, existingOrder);
                        var result = await _unitOfWork.OrderRepository.UpdateAsync(existingOrder);
                        await _unitOfWork.OrderRepository.SaveAsync();
                        return result > 0
                            ? new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, existingOrder)
                            : new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, order);
                    }
                    else
                    {
                        return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, order);
                    }
                }
                else
                {
                    order.Status = "Active";
                    var result = await _unitOfWork.OrderRepository.CreateAsync(order);
                    await _unitOfWork.OrderRepository.SaveAsync();

                    return result > 0
                        ? new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, order)
                        : new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, order);
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

                orderById.Status = "Inactive";
                var result = await _unitOfWork.OrderRepository.UpdateAsync(orderById);
                await _unitOfWork.OrderRepository.SaveAsync();

                if (result >0)
                {
                    return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, orderById);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, orderById);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

    }
}