using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.ProductDTO
{
    public class ProductFilterReqModel
    {
        public List<string>? Category { get; set; }

        public List<string>? Brand { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public decimal? MinDiscount { get; set; }

        public decimal? MaxDiscount { get; set; }
    }
}
