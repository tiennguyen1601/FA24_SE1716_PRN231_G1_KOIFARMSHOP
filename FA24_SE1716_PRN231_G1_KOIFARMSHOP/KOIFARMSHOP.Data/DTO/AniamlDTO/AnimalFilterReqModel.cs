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
        public List<string>? Name { get; set; }
        public List<string>? Origin { get; set; }

        public decimal? Price { get; set; }
        
    }
}
