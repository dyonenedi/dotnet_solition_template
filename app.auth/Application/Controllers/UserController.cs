using app.auth.Application.Services;
using app.shared.Libs.DTOs.User;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("v1/user")]
public class UserController : ControllerBase {
    private readonly UserService _userService;

    public UserController(UserService userService) {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto) {
        var response = await _userService.RegisterAsync(dto);
        
        if (response.Success) {
            return Ok(response);
        }
        
        return BadRequest(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto) {
        var response = await _userService.AuthenticateAsync(dto.Email, dto.Password);
        
        if (response.Success) {
            return Ok(response);
        }
        
        return Unauthorized(response);
    }
}
