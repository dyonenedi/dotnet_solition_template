
using app.api.Application.Services;
using app.shared.Libs.DTOs.User;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("v1/user")]
public class UserController : ControllerBase {
    private readonly UserService _userService;

    public UserController(UserService userService) {
        _userService = userService;
    }
}
