using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFAereoNuvem.Models;

[Table("Airports")]
public class Airport
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(3)]
    [Column("IATA", TypeName = "varchar(3)")]
    public string IATA { get; set; } = null!;

    [Required]
    [Column("AirportsName")]
    public string Name { get; set; } = null!;

    public Guid AdressId { get; set; }

    public Adress Adress { get; set; } = null!; // Propriedade de navegação 
}
