using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.CustomerDTO
{
    public class CustomerVerifyReqModel
    {
        public string email { get; set; }

        public string otp { get; set; }
    }
}
