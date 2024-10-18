using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.AniamlDTO
{
    public class AnimalFilterReqModel
    {
        public List<string>? Species { get; set; }
        public List<string>? Status { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SearchValue { get; set; }
    }
}
