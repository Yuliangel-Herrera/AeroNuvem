using System.ComponentModel.DataAnnotations;

namespace EFAereoNuvem.ViewModel;
public class LoginViewModel
{
    [Required(ErrorMessage = "Informe o E-mail")]
    [EmailAddress(ErrorMessage = "O E-mail é inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a senha")]
    public string Password { get; set; } = string.Empty;
}
