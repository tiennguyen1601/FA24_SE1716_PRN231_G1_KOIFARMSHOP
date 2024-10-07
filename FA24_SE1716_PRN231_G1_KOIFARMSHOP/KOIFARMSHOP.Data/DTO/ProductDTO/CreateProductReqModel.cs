using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.ProductDTO
{
    public class CreateProductReqModel
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string? Brand { get; set; }

        public decimal? Weight { get; set; }

        public decimal? Discount { get; set; }

        public DateOnly? ExpiryDate { get; set; }

        public DateOnly? ManufacturingDate { get; set; }

        public int? CategoryId { get; set; }

        public int? CreatedBy { get; set; }
    }
}
