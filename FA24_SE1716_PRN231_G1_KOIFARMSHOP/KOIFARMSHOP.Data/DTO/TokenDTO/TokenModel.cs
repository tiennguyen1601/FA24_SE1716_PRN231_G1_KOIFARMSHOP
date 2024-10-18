using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.TokenDTO
{
    public class TokenModel
    {
        public string userid { get; set; }

        public string roleName { get; set; }

        //public string email { get; set; }

        public string username { get; set; }

        public TokenModel(string userid, string roleName, string username)
        {
            this.userid = userid;
            this.roleName = roleName;
            this.username = username;
        }
    }
}
