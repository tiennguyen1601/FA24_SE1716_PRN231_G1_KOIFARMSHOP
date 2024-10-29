using AutoMapper;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Data.DTO.AniamlDTO;
using KOIFARMSHOP.Data.DTO.OrderDTO;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Service.Services.JWTService;
using Microsoft.EntityFrameworkCore;
namespace KOIFARMSHOP.Service.Services
{
    public interface IOrderService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetAll(int? page, int? size);
        Task<IBusinessResult> SearchAndPaginateOrders(OrderFilterReqModel? filterReqModel, string? searchValue, int? page, int? size);
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
        public async Task<IBusinessResult> SearchAndPaginateOrders(OrderFilterReqModel? filterReqModel, string? searchValue, int? page, int? size)
        {
            var ordersList = await _unitOfWork.OrderRepository.GetAllDetail();

            if (filterReqModel != null)
            {
                if (filterReqModel.ShippingAddress != null && filterReqModel.ShippingAddress.Any())
                {
                    ordersList = ordersList.Where(o => filterReqModel.ShippingAddress.Contains(o.ShippingAddress)).ToList();
                }

                if (filterReqModel.DeliveryMethod != null && filterReqModel.DeliveryMethod.Any())
                {
                    ordersList = ordersList.Where(o => filterReqModel.DeliveryMethod.Contains(o.DeliveryMethod)).ToList();
                }

                if (filterReqModel.PaymentStatus != null && filterReqModel.PaymentStatus.Any())
                {
                    ordersList = ordersList.Where(o => filterReqModel.PaymentStatus.Contains(o.PaymentStatus)).ToList();
                }

                if (filterReqModel.TotalAmountVAT.HasValue)
                {
                    ordersList = ordersList.Where(o => o.TotalAmount >= filterReqModel.TotalAmountVAT.Value).ToList();
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                ordersList = ordersList.Where(o => o.ShippingAddress.Contains(searchValue) ||
                                                                   o.DeliveryMethod.Contains(searchValue) ||
                                                                   o.PaymentStatus.Contains(searchValue)).ToList();
            }

            var totalItemCount = ordersList.Count;

            var pagedOrders = ordersList
                .Skip(((page ?? 1) - 1) * (size ?? 10))
                .Take(size ?? 10)
                .ToList();

            var orderResponseList = _mapper.Map<List<OrderResponseModel>>(pagedOrders);

            var result = new Pagination<OrderResponseModel>
            {
                TotalItems = totalItemCount,
                PageSize = size ?? 10,
                CurrentPage = page ?? 1,
                Data = orderResponseList
            };

            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
        }





        public async Task<IBusinessResult> GetAll(int? page, int? size)
        {
            var orders = await _unitOfWork.OrderRepository.GetAllDetail();

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


            var totalItemCount = orderBuyRequestModels.Count;

            var pagedItem = orderBuyRequestModels.Skip(((page ?? 1) - 1) * (size ?? 10))
                    .Take(size ?? 10).ToList();

            var result = new Pagination<OrderResponseModel>
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

            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(userId);
            try
            {
                if (orderCompleteRequest == null)
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

                    var orderDetailEntity = new OrderDetail
                    {
                        ProductId = null,
                        AnimalId = orderCompleteRequest.AnimalID,
                        Quantity = 1, 
                        Amount = price,
                        Subtotal = price,
                        Price = price
                    };
                    order.OrderDetails.Add(orderDetailEntity);
                }
                else if (orderCompleteRequest.ProductID.HasValue)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(orderCompleteRequest.ProductID.Value);

                    if (product == null)
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, "Product not found.", null);
                    }

                    int availableQuantity = product.StockQuantity; 
                    int quantity = orderCompleteRequest.Quantity ?? 1;

                    if (availableQuantity < quantity)
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, "Out of quantity", null);
                    }

                    product.StockQuantity -= quantity;

                    await _unitOfWork.ProductRepository.UpdateAsync(product);

                    price = product.Price;

                    var orderDetailEntity = new OrderDetail
                    {
                        ProductId = orderCompleteRequest.ProductID,
                        AnimalId = null,
                        Quantity = quantity,
                        Amount = price * quantity,
                        Subtotal = price * quantity,
                        Price = price
                    };
                    order.OrderDetails.Add(orderDetailEntity);
                }


                else
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, "No item selected for purchase.", null);
                }

                order.TotalAmount = price * (orderCompleteRequest.Quantity ?? 1); 
                order.Vat = 10;
                decimal amountWithVat = order.TotalAmount * (1 + (order.Vat ?? 0) / 100);
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
                    order.PaymentStatus = "Unpaid";

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