using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Service
{
    public interface IOrderService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly UnitOfWork _unitOfWork;

        public OrderService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IBusinessResult> GetAll()
        {
            var list = await _unitOfWork.OrderRepository.GetAllAsync();

            if (list == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Order>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, list);
            }
        }

        public async Task<IBusinessResult> GetById(int id)
        {
            var data = await _unitOfWork.OrderRepository.GetByIdAsync(id);

            if (data == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Order());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, data);
            }
        }
    }
}
