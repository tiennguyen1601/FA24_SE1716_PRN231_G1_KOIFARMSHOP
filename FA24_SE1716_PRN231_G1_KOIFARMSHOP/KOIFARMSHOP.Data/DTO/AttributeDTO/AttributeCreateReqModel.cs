using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.AttributeDTO
{
    public class AttributeCreateReqModel
    {
        public string AttributeName { get; set; } = null!;

        public string DisplayName { get; set; } = null!;
    }
}
