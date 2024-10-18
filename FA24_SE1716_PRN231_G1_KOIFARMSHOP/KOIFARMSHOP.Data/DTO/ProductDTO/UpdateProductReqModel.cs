using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.ProductDTO
{
    public class UpdateProductReqModel
    {
        [Required(ErrorMessage = "ProductId is required.")]
        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "StockQuantity cannot be negative.")]
        public int StockQuantity { get; set; }

        public string? Brand { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Weight cannot be negative.")]
        public decimal? Weight { get; set; }

        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public decimal? Discount { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public DateOnly? ManufacturingDate { get; set; }
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "ModifiedBy is required.")]
        public int? ModifiedBy { get; set; }

        public List<string>? Images { get; set; } = new List<string>();
    }
}
