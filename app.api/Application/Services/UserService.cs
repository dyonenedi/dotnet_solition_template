using app.api.Application.Db;
using app.api.Application.Models;
using app.shared.Libs.DTOs.User;
using app.shared.Libs.Responses;
using Microsoft.EntityFrameworkCore;

namespace app.api.Application.Services;

public class UserService {
    private readonly AppDbContext _db;
    private readonly ErrorLogService _errorLogService;

    public UserService(AppDbContext db, ErrorLogService errorLogService) {
        _db = db;
        _errorLogService = errorLogService;
    }
}
