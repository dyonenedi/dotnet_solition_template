namespace app.shared.Libs.DTOs.User;

public class LoginDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Token { get; set; } = null;
}