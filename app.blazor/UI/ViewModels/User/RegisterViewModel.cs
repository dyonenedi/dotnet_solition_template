using System.ComponentModel.DataAnnotations;

namespace app.blazor.UI.ViewModels.User;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(50, ErrorMessage = "O nome não pode exceder 50 caracteres.")]
    [MinLength(2, ErrorMessage = "O nome deve ter pelo menos 2 caracteres.")]
    [RegularExpression(@"^[A-Za-zÀ-ÿ ]+$", ErrorMessage = "O nome deve conter apenas letras e espaços.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
    [MinLength(5, ErrorMessage = "O email deve ter pelo menos 5 caracteres.")]
    [RegularExpression(@"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$", ErrorMessage = "O email deve conter apenas letras minúsculas.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Usuário é obrigatório")]
    [StringLength(15, ErrorMessage = "O usuário não pode exceder 15 caracteres.")]
    [MinLength(3, ErrorMessage = "O usuário deve ter pelo menos 3 caracteres.")]
    [RegularExpression(@"^[a-z0-9_.]+$", ErrorMessage = "O usuário deve conter apenas letras minúsculas, números e os caracteres _ .")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(25, ErrorMessage = "A senha não pode exceder 25 caracteres.")]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
    [StringLength(25, ErrorMessage = "A confirmação de senha não pode exceder 25 caracteres.")]
    [MinLength(6, ErrorMessage = "A confirmação de senha deve ter pelo menos 6 caracteres.")]
    [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
    public string ConfirmPassword { get; set; } = string.Empty;
    public string Role { get; set; } = "USER";
}
