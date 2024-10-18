using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.CategoryDTO
{
    public class CategoryCreateReqModel
    {
        public required string Name { get; set; } = null;

        public required string Description { get; set; }
    }
}
