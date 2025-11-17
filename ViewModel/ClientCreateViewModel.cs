namespace EFAereoNuvem.ViewModel;
public class ClientCreateViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime BornDate { get; set; }

    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string? Complement { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = "Brasil";
    public string Cep { get; set; } = string.Empty;

    // Senha para criar o User
    public string Password { get; set; } = string.Empty;
}

