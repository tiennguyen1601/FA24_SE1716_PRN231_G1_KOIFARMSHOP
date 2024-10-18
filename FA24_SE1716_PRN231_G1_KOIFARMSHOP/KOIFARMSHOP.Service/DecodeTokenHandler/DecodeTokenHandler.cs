using KOIFARMSHOP.Data.DTO.TokenDTO;
using KOIFARMSHOP.Service.Services.JWTService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Service.DecodeTokenHandler
{
    public interface IDecodeTokenHandler
    {
        TokenModel decode(string token);
    }
    public class DecodeTokenHandler : IDecodeTokenHandler
    {
        private readonly IJWTService _jWTService;

        public DecodeTokenHandler(IJWTService jWTService)
        {
            _jWTService = jWTService;
        }
        public TokenModel decode(string token)
        {
            var roleName = _jWTService.decodeToken(token, ClaimsIdentity.DefaultRoleClaimType);
            var userId = _jWTService.decodeToken(token, "userid");
            var username = _jWTService.decodeToken(token, "username");

            return new TokenModel(userId, roleName, username);
        }
    }
}
