using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.ConsignmentDTO
{
    public class CreateConsignmentReq
    {
        [Required(ErrorMessage = "Animal is required.")]
        public int? AnimalId { get; set; }

        [Required(ErrorMessage = "Consignment type is required.")]
        public string? ConsignmentType { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateOnly? StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateOnly? EndDate { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Notes are required.")]
        public string? Notes { get; set; }

        [Range(0, 100, ErrorMessage = "Commission rate must be between 0 and 100%.")]
        public decimal? CommissionRate { get; set; }

    }
}
