using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApp.Helpers
{
    /// <summary>
    ///     from: https://github.com/cuongle/WebApi.Jwt
    /// </summary>
    public static class JwtTokenHelper
    {
        public const string Secret = "856FECBA3B06519C8DDDBC80BB080553";             // your symetric

        public static string GenerateToken(string username,
             out string plainToken, string tenantName = "")
        {
            if (username == null)
            {
                username = "testuser";
            }

            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.UserData, tenantName),
                }),

                Expires = now.AddMinutes(Convert.ToInt32(20)),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(symmetricKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            plainToken =
                stoken.ToString();

            return token;
        }

        public static ClaimsPrincipal GetPrincipal(string token, out string plainToken)
        {
            plainToken = "";
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(Secret);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                ClaimsPrincipal principal =
                    tokenHandler.ValidateToken(token, validationParameters,
                        out securityToken);

                plainToken = securityToken.ToString();

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ClaimsPrincipal
            GetPrincipalAndTenantName(
                string token,
                out string plainToken,
                out string tenantName)
        {
            plainToken = "";
            tenantName = "";
            try
            {
                var tokenHandler =
                    new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token)
                    as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(Secret);

                var validationParameters =
                    new TokenValidationParameters()
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(symmetricKey)
                    };

                SecurityToken securityToken;
                ClaimsPrincipal principal =
                    tokenHandler.ValidateToken
                    (token, validationParameters,
                        out securityToken);

                foreach (var x in jwtToken.Claims)
                {
                    if (x.Type.EndsWith("userdata"))
                    {
                        tenantName = x.Value;
                    }
                }

                plainToken = securityToken.ToString();

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}