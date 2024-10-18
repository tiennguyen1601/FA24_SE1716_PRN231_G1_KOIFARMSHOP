using AutoMapper;
using KOIFARMSHOP.Data.DTO.AniamlDTO;
using KOIFARMSHOP.Data.DTO.OrderDTO;
using KOIFARMSHOP.Data.DTO.PaymentDTO;
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
            #region Order
            CreateMap<Order, OrderResponseModel>();
            CreateMap<OrderDetail, OrderDetailResponseModel>();
            CreateMap<OrderDetailRequest, OrderDetail>();
            #endregion

            #region OrderDetail
            CreateMap<OrderRequestModel, Order>();
            #endregion

            #region Payment
            CreateMap<PaymentCreateRequestModel, Payment>();
            #endregion

            #region Animal
            CreateMap<AnimalReqModel, Animal>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.AnimalImages, opt => opt.Ignore());
            #endregion
        }
    }
}
