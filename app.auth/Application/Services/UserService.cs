using app.auth.Application.Models;
using app.auth.Application.Repositories;
using app.auth.Application.Utils;
using app.shared.Libs.DTOs.User;
using app.shared.Libs.Responses;

namespace app.auth.Application.Services;

public class UserService {
    private readonly ErrorLogService _errorLogService;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwt;

    public UserService(IUserRepository userRepository, ErrorLogService errorLogService, IJwtTokenService JwtTokenService)
    {
        _userRepository = userRepository;
        _errorLogService = errorLogService;
        _jwt = JwtTokenService;
    }

    public async Task<SimpleResponse> RegisterAsync(RegisterDTO dto) {
        // Basic validation
        if (string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Username) ||
            string.IsNullOrWhiteSpace(dto.Password) ||
            string.IsNullOrWhiteSpace(dto.FullName))
        {
            return SimpleResponse.CreateError("Todos os campos são obrigatórios.").WithStatus(OperationStatus.ValidationError);
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
                return SimpleResponse.CreateError("Email ou usuário já cadastrado.").WithStatus(OperationStatus.Conflict);
            }

            var user = new User
            {
                FullName = dto.FullName,
                Username = dto.Username,
                Email = dto.Email,
                Password = HashPassword(dto.Password),
                InsertDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };

            await _userRepository.AddAsync(user);
            if (await _userRepository.SaveChangesAsync() > 0)
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    Role = "USER",
                    InsertDate = DateTime.Now,
                    UptadeDate = DateTime.Now,
                };

                user.UserRoles.Add(userRole);
                if (await _userRepository.SaveChangesAsync() > 0)
                {
                    await _userRepository.CommitAsync();
                    return SimpleResponse.CreateSuccess("Usuário registrado com sucesso.").WithStatus(OperationStatus.Success);
                }
            }
            await _userRepository.RollbackAsync();
            return SimpleResponse.CreateError("Erro interno ao registrar usuário.").WithStatus(OperationStatus.Error);
        }
        catch (Exception ex) {
            await _errorLogService.LogErrorAsync(0, $"{nameof(UserService)}.{nameof(RegisterAsync)}", ex);
            await _userRepository.RollbackAsync();
            return SimpleResponse.CreateError("Erro interno do servidor. Tente novamente.").WithStatus(OperationStatus.Error);
        } 
    }

    public async Task<Response<LoginDTO>> AuthenticateAsync(LoginDTO dto) {
        try {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password)) {
                return Response<LoginDTO>.CreateError("Email e senha são obrigatórios.").WithStatus(OperationStatus.ValidationError);
            }

            dto.Email = dto.Email.Trim().ToLower();
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            
            if (user == null) {
                return Response<LoginDTO>.CreateError("Email não cadastrado.").WithStatus(OperationStatus.Unauthorized);
            }

            if (!VerifyPassword(dto.Password, user.Password)) {
                return Response<LoginDTO>.CreateError("Senha ou Email inválido.").WithStatus(OperationStatus.Unauthorized);
            }
            
            var token = _jwt.CreateToken(user.Id.ToString(), user.Email, user.UserRoles.Select(ur => ur.Role));
            dto.Token = token;

            return Response<LoginDTO>.CreateSuccess(dto, "Autenticação realizada com sucesso.").WithStatus(OperationStatus.Success);
        }
        catch (Exception ex) {
            await _errorLogService.LogErrorAsync(0, $"{nameof(UserService)}.{nameof(AuthenticateAsync)}", ex);
            return Response<LoginDTO>.CreateError("Erro interno do servidor. Tente novamente.").WithStatus(OperationStatus.Error);
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
