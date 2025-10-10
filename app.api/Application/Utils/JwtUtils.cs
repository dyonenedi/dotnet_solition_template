using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using app.api.Application.Services;

namespace app.api.Application.Utils
{
    public static class JwtUtils
    {
        private static ErrorLogService _errorLogService = new ErrorLogService(null!);

        public static async Task<ClaimsPrincipal?> ValidateToken(string token, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secret);
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                var principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Token inválido");
                await _errorLogService.LogErrorAsync(0, nameof(ValidateToken), ex);
                return null;
            }
        }

        public static string? GetUserId(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("id")?.Value;
        }
    }
}
