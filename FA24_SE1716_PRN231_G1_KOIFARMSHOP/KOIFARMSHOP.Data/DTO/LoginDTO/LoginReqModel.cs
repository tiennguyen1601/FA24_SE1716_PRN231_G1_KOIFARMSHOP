﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.LoginDTO
{
    public class LoginReqModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
