using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using app.auth.Application.Services;

namespace app.auth.Application.Utils;
public interface IJwtTokenService
{
    string CreateToken(string userId, string email, IEnumerable<string> roles);
    Task<ClaimsPrincipal?> ValidateToken(string token, string secret);
}

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private static ErrorLogService _errorLogService = new ErrorLogService(null!);
    
    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(string userId, string email, IEnumerable<string> roles)
    {
        var secret = _configuration["Secret"];
        if (string.IsNullOrEmpty(secret))
            throw new ArgumentNullException(nameof(secret), "JWT Secret não configurado.");
        var key = Encoding.UTF8.GetBytes(secret);
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<ClaimsPrincipal?> ValidateToken(string token, string secret)
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
}
