using app.auth.Application.Models;
using app.auth.Application.Repositories;
using app.shared.Libs.DTOs.User;
using app.shared.Libs.Responses;

namespace app.auth.Application.Services;

public class UserService {
    private readonly ErrorLogService _errorLogService;
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository, ErrorLogService errorLogService) {
        _userRepository = userRepository;
        _errorLogService = errorLogService;
    }

    public async Task<SimpleResponse> RegisterAsync(RegisterDTO dto) {
        // Basic validation
        if (string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Username) ||
            string.IsNullOrWhiteSpace(dto.Password) ||
            string.IsNullOrWhiteSpace(dto.FullName))
        {
            return SimpleResponse.CreateError("Todos os campos são obrigatórios.");
        }
        // Trim and normalize inputs
        dto.FullName = dto.FullName.Trim();
        dto.Username = dto.Username.Trim().ToLower();
        dto.Email = dto.Email.Trim().ToLower();

        await _userRepository.BeginTransactionAsync();
        try {
            var exists = await _userRepository.ExistsAsync(dto.Email, dto.Username);

            if (exists) {
                await _userRepository.RollbackAsync();
                return SimpleResponse.CreateError("Email ou usuário já cadastrado.");
            }

            var user = new User
            {
                FullName = dto.FullName,
                Username = dto.Username,
                Email = dto.Email,
                Password = HashPassword(dto.Password),
                InsertDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            await _userRepository.AddAsync(user);
            if (await _userRepository.SaveChangesAsync() > 0)
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    Role = "USER",
                    InsertDate = DateTime.UtcNow,
                    UptadeDate = DateTime.UtcNow,
                };

                user.UserRoles.Add(userRole);
                if (await _userRepository.SaveChangesAsync() > 0)
                {
                    await _userRepository.CommitAsync();
                    return SimpleResponse.CreateSuccess("Usuário registrado com sucesso.");
                }
            }
            await _userRepository.RollbackAsync();
            return SimpleResponse.CreateError("Erro interno ao registrar usuário.");
        }
        catch (Exception ex) {
            await _errorLogService.LogErrorAsync(0, $"{nameof(UserService)}.{nameof(RegisterAsync)}", ex);
            await _userRepository.RollbackAsync();
            return SimpleResponse.CreateError("Erro interno do servidor. Tente novamente.");
        } 
    }

    public async Task<Response<User>> AuthenticateAsync(string email, string password) {
        try {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) {
                return Response<User>.CreateError("Email e senha são obrigatórios.");
            }

            var user = await _userRepository.GetByEmailAsync(email);
            
            if (user == null) {
                return Response<User>.CreateError("Credenciais inválidas.");
            }

            if (!VerifyPassword(password, user.Password)) {
                return Response<User>.CreateError("Credenciais inválidas.");
            }
            
            return Response<User>.CreateSuccess(user, "Autenticação realizada com sucesso.");
        }
        catch (Exception ex) {
            await _errorLogService.LogErrorAsync(0, $"{nameof(UserService)}.{nameof(AuthenticateAsync)}", ex);
            return Response<User>.CreateError("Erro interno do servidor. Tente novamente.");
        }
    }

    private string HashPassword(string password) {
        // Using BCrypt with work factor 12 (secure and performant)
        return BCrypt.Net.BCrypt.HashPassword(password, 12);
    }

    public bool VerifyPassword(string password, string hashedPassword) {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
