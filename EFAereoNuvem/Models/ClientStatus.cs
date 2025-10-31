using EFAereoNuvem.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFAereoNuvem.Models;

[Table("ClientStatus")]
public class ClientStatus
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Status Status { get; set; }
    public bool Priority { get; set; } = false;
    public float Discount { get; set; } = 0;
}
