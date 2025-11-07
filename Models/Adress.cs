using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFAereoNuvem.Models
{
    [Table("Adresses")]
    public class Adress
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Street { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? Number { get; set; }

        [MaxLength(50)]
        public string? Complement { get; set; }

        [Required]
        [MaxLength(60)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(2)]
        public string State { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Country { get; set; } = "Brasil";

        [Required]
        [MaxLength(9)]
        public string Cep { get; set; } = string.Empty;
    }
}