using KOIFARMSHOP.Data.Enums;
using KOIFARMSHOP.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Service.Services.JWTService
{
    public interface IJWTService
    {
        string GenerateJWT<T>(T entity) where T : class;
        string decodeToken(string jwtToken, string nameClaim);
    }
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _config;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JWTService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string decodeToken(string jwtToken, string nameClaim)
        {
            Claim? claim = _tokenHandler.ReadJwtToken(jwtToken).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));

            return claim != null ? claim.Value : "Error!!!";
        }

        public string GenerateJWT<T>(T entity) where T : class
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:JwtKey"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>();

            if (entity is Customer customer)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, RoleEnums.Customer.ToString()));
                claims.Add(new Claim("userid", customer.CustomerId.ToString()));
                claims.Add(new Claim("username", customer.Username));
            }else if (entity is Staff staff)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, RoleEnums.Staff.ToString()));
                claims.Add(new Claim("userid", staff.StaffId.ToString()));
                claims.Add(new Claim("username", staff.Username));
            }else
            {
                throw new ArgumentException("Unsupported entity type");
            }

            var token = new JwtSecurityToken(
               issuer: _config["JwtSettings:Issuer"],
               audience: _config["JwtSettings:Audience"],
               claims: claims,
               expires: DateTime.Now.AddMinutes(15),
               signingCredentials: credential
               );


            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }
    }
}
