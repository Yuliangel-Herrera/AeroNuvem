namespace EFAereoNuvem.Models;
public class Role
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TypeRole { get; set; } = string.Empty;
    public ICollection<User> Users { get; set; } = [];
}
