
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NetDapperWebApi.Common.Interfaces;

namespace NetDapperWebApi.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _tokenKey;
        private readonly string _expiration;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration configuration)
        {
            _tokenKey = configuration["AppSettings:TokenSecret"]!;
            _issuer = configuration["AppSettings:Issuer"]!;
            _audience = configuration["AppSettings:Audience"]!;
            _expiration = configuration["AppSettings:Expiration"]!;
        }


        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new
            SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {

            // Sử dụng RNGCryptoServiceProvider để tạo refresh token an toàn
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            //validate 
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true, //audience
                ValidAudience = _audience,
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenKey)),
                ValidateLifetime = false // Chấp nhận token hết hạn để có thể refresh
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token,
            tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }


    }
}