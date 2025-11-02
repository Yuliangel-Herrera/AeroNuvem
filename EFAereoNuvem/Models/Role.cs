namespace EFAereoNuvem.Models;
public class Role
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TypeRole { get; set; } = "Client";
    public ICollection<User> Users { get; set; } = [];
}
