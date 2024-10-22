using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.AniamlDTO
{
    public class CompareMultipleAnimalRequestModels
    {
        public List<int> Ids { get; set; }
        public List<string> ComparisonAttributes { get; set; }
    }
}
