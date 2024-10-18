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
        Task<IBusinessResult> GetAll(int? page, int? size);
        Task<IBusinessResult> GetByID(int id);
        Task<IBusinessResult> Save(OrderCompleteRequest orderCompleteRequest, string token);
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
            }).ToList();

            return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, orderBuyRequestModels);
        }


        public async Task<IBusinessResult> GetAll(int? page, int? size)
        {
            var orders = await _unitOfWork.OrderRepository.GetAllDetail();

            var totalItemCount = orders.Count;

            var pagedItem = orders.Skip(((page ?? 1) - 1) * (size ?? 10))
                    .Take(size ?? 10).ToList();

            var result = new Pagination<Order>
            {
                TotalItems = totalItemCount,
                PageSize = size ?? 10,
                CurrentPage = page ?? 1,
                Data = pagedItem
            };

            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
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

        public async Task<IBusinessResult> Save(OrderCompleteRequest orderCompleteRequest, string token)
        {
            var userIdString = _jwtService.decodeToken(token, "userid");
            if (!int.TryParse(userIdString, out int userId))
            {
                return new BusinessResult(Const.FAIL_CREATE_CODE, "Invalid user ID.", null);
            }

            var task = await _unitOfWork.CustomerRepository.GetByIdAsync(userId);
            try
            {
                if (orderCompleteRequest == null || orderCompleteRequest == null)
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, "Invalid request.", null);
                }

                var order = _mapper.Map<Order>(orderCompleteRequest);
                order.CustomerId = userId;
                order.OrderDetails = new List<OrderDetail>();
                decimal totalAmountVat = 0;

                
                    decimal price = 0;
                     if (orderCompleteRequest.AnimalID.HasValue)
                    {
                        var animal = await _unitOfWork.AnimalRepository.GetByIdAsync(orderCompleteRequest.AnimalID.Value);
                        price = animal?.Price ?? 0;
                    }

                    var orderDetailEntity = new OrderDetail
                    {
                        ProductId = null,
                        AnimalId = orderCompleteRequest.AnimalID,
                        Quantity = orderCompleteRequest.Quantity,
                        Amount = price * orderCompleteRequest.Quantity,
                        Subtotal = price * orderCompleteRequest.Quantity,
                        Price = price
                    };

                    order.OrderDetails.Add(orderDetailEntity);

                    decimal amountWithVat = orderDetailEntity.Subtotal * (1 + (orderCompleteRequest.Vat ?? 0));
                    totalAmountVat += amountWithVat; 
                

                order.TotalAmountVat = totalAmountVat;

                if (order.OrderId > 0)
                {
                    var existingOrder = await _unitOfWork.OrderRepository.GetByIdAsync(order.OrderId);
                    if (existingOrder != null)
                    {
                        _mapper.Map(orderCompleteRequest, existingOrder);
                        var result = await _unitOfWork.OrderRepository.UpdateAsync(existingOrder);
                        if (result > 0)
                        {
                            return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, order);
                        }
                        else
                        {
                            return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, order);
                        }
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