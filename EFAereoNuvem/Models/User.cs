using System.Data;
using System.Text.Json.Serialization;

namespace EFAereoNuvem.Models;
public class User
{
    // tabela para controle de acesso (login, senha, permissões).
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;

    // Relação com Role N:N
    public IList<Role> Roles { get; set; } = [];
}
