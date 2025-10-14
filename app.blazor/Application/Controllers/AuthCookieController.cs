
using Microsoft.AspNetCore.Mvc;

namespace app.blazor.Application.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthCookieController : ControllerBase
{
    public class JwtDto { public string Token { get; set; } = null!; }

    [HttpGet("set-token")]
    public IActionResult SetToken([FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return BadRequest("Token não informado");

        Response.Cookies.Append("accessToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddMinutes(60) // Expira em 60 minutos
        });
        return Redirect("/auth/callback");
    }
    
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("accessToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });

        return Redirect("/");
    }
}
