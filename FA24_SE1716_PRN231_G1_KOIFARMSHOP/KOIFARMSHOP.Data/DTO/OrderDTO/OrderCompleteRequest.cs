using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.OrderDTO
{
    namespace KOIFARMSHOP.Data.DTO.OrderDTO
    {
        public class OrderCompleteRequest
        {
            public OrderRequestModel Order { get; set; }
            public List<OrderDetailRequest> OrderDetails { get; set; }
        }
    }

}
