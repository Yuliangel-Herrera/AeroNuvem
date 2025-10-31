using System.Data;

namespace EFAereoNuvem.Models;
public class User
{
    // tabela para controle de acesso (login, senha, permissões).
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Relação com Role N:N
    public ICollection<Role> Roles { get; set; } = [];
}
