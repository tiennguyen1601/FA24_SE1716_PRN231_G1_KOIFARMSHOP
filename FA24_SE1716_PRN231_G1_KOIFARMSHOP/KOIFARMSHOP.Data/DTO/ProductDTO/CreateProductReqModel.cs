using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.ProductDTO
{
    public class CreateProductReqModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "StockQuantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "StockQuantity cannot be negative.")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        public string? Brand { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Weight cannot be negative.")]
        public decimal? Weight { get; set; }

        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public decimal? Discount { get; set; }

        [Required(ErrorMessage = "ExpiryDate is required.")]
        public DateOnly? ExpiryDate { get; set; }

        [Required(ErrorMessage = "ManufacturingDate is required.")]
        public DateOnly? ManufacturingDate { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "CreatedBy is required.")]
        public int? CreatedBy { get; set; }

        [Required(ErrorMessage = "At least one image is required.")]
        [MinLength(1, ErrorMessage ="At least one image is required")]
        public List<string> Images { get; set; } = new List<string>();
    }
}
