
using System.ComponentModel.DataAnnotations;

namespace app.blazor.UI.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
        [MinLength(5, ErrorMessage = "O email deve ter pelo menos 5 caracteres.")]
        [RegularExpression(@"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$", ErrorMessage = "O email deve conter apenas letras minúsculas.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(25, ErrorMessage = "A senha não pode exceder 25 caracteres.")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string Password { get; set; } = string.Empty;
    }
}