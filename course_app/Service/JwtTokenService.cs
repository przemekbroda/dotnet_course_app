using course_app.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace course_app.Service
{
    public class JwtTokenService : IJwtTokenService
    {

        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAccessToken(User user, DateTime expireAt)
        {
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            return GenerateToken(user, expireAt, _config.GetSection("AppSettings:Token").Value, claims);
        }

        public string GenerateRefreshToken(User user, DateTime expireAt)
        {
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("refresh-type", true.ToString())
            };

            return GenerateToken(user, expireAt, _config.GetSection("AppSettings:RefreshToken").Value, claims);
        }

        public bool ValidateRefreshToken(string refreshToken, out int id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token;

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:RefreshToken").Value)),
                ClockSkew = TimeSpan.Zero
            };

            ClaimsPrincipal claims;
            try
            {
                claims = tokenHandler.ValidateToken(refreshToken, parameters, out token);
            }
            catch (Exception)
            {
                id = -1;
                return false;
            }

            try 
            {
                var refreshType = claims.FindFirst("refresh-type")?.Value ?? null;
                if (refreshType == null || !bool.Parse(refreshType))
                {
                    id = -1;
                    return false;
                }
            }
            catch (Exception)
            {
                id = -1;
                return false;
            }

            var idClaim = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;

            id = idClaim == null ? -1 : int.Parse(idClaim);
            return true;
        }

        private string GenerateToken(User user, DateTime expireAt, string secret, IEnumerable<Claim> claims) 
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expireAt,
                SigningCredentials = credentials
            };

            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateJwtSecurityToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}
