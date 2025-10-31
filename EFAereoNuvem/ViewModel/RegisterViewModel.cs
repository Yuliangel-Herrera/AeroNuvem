using System.ComponentModel.DataAnnotations;

namespace EFAereoNuvem.ViewModel;
public class RegisterViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "O E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "O E-mail é inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Password { get; set; } = string.Empty;
}
