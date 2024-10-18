using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.AniamlDTO
{
    public class AnimalReqModel
    {
        public string? Name { get; set; }
        public string? Origin { get; set; }
        public string? Species { get; set; }
        public string? Type { get; set; }
        public string? Gender { get; set; }
        public string? Size { get; set; }
        public string? Certificate { get; set; }
        public decimal? Price { get; set; }
        public string? Status { get; set; }
        public decimal? MaintenanceCost { get; set; }
        public string? Color { get; set; }
        public decimal? AmountFeed { get; set; }
        public string? HealthStatus { get; set; }
        public string? FarmOrigin { get; set; }
        public int? BirthYear { get; set; }
        public string? Description { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public List<string>? AnimalImages { get; set; }
    }
}
