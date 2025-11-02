using System.ComponentModel.DataAnnotations;

namespace EFAereoNuvem.ViewModel;
public class RegisterViewModel
{
    [Required(ErrorMessage = "CPF é obrigatório.")]
    [StringLength(14, MinimumLength = 11, ErrorMessage = "CPF inválido.")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "O E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "O E-mail é inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Password { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Telefone inválido.")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Data de nascimento é obrigatória.")]
    [DataType(DataType.Date)]
    public DateOnly BornDate { get; set; }


    // Endereço atual (obrigatório)
    [Required(ErrorMessage = "Endereço é obrigatório.")]
    public string Street { get; set; } = string.Empty;
    public string? Number { get; set; }
    public string? Complement { get; set; }

    [Required(ErrorMessage = "A cidade é obrigatória.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "O Estado é obrigatório.")]
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = "Brasil";

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    public string Cep { get; set; } = string.Empty;
}
