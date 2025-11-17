using EFAereoNuvem.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFAereoNuvem.Models;

[Table("Armchairs")]
public class Armchair
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "O codigo é obrigatório.")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "A classe é obrigatória.")]
    public Class Class { get; set; }
    public SideType Side { get; set; }
    public bool IsAvaliable { get; set; }
    public Reservation? Reservation { get; set; } 
}
