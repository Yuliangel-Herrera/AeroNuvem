using EFAereoNuvem.Models;

namespace EFAereoNuvem.ViewModel;
public class ReservationInformationViewModel
{
    public string CodeRersevation { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string FlightCode { get; set; } = string.Empty;
    public string ArmchairNumber { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public float Price { get; set; }
    public DateTime DateReservation { get; set; }

    public static ReservationInformationViewModel GetReservationInformationViewModel(Reservation reservation)
    {
        return new ReservationInformationViewModel
        {
            CodeRersevation = reservation.CodeRersevation,
            ClientName = reservation.Client.Name,
            FlightCode = reservation.Flight.CodeFlight,
            ArmchairNumber = reservation.ReservedArmchair.Code,
            Class = reservation.Class.ToString(),
            Price = reservation.Price,
            DateReservation = reservation.DateReservation
        };
    }
}
