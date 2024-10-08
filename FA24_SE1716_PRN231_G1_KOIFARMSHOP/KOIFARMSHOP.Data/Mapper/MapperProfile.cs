using AutoMapper;
using KOIFARMSHOP.Data.DTO.OrderDTO;
using KOIFARMSHOP.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //Order
            CreateMap<Order, OrderResponseModel>();
            CreateMap<OrderDetail, OrderDetailResponseModel>();
        }
        }
}
