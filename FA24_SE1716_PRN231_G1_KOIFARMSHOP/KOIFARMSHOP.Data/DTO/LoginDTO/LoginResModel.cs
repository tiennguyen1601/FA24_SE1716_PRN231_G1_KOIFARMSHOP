using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.LoginDTO
{
    public class LoginResModel
    {
        public int UserId { get; set; }

        public string Username { get; set; } = null!;

        public string? Role { get; set; }

        public string Token { get; set; }

    }
}
