using EFAereoNuvem.Models.Enum;

namespace EFAereoNuvem.Models
{
    public class Reservation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CodeRersevation { get; set; } = string.Empty;
        public Class Class { get; set; }
        public float Price { get; set; }
        public DateTime DateReservation { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; } = null!;
        public Guid FlightId { get; set; }
        public Flight Flight { get; set; } = null!;
        public Guid ArmchairId { get; set; }
        public Armchair ReservedArmchair { get; set; } = new();
        public string Gate { get; set; } = string.Empty;
    }
}
