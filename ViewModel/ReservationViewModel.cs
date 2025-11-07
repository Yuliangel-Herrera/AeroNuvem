using EFAereoNuvem.Models;
using EFAereoNuvem.Models.Enum;

namespace EFAereoNuvem.ViewModel;
public class ReservationViewModel
{
    public Guid Id { get; set; }
    public string ReservationNumber { get; set; } = string.Empty;
    public string Status { get; set; } = "Confirmada";
    public DateTime ReservationDate { get; set; }

    // Voo
    public string FlightNumber { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string Airline { get; set; } = string.Empty;
    public string AircraftType { get; set; } = string.Empty;

    // Passageiro
    public string PassengerName { get; set; } = string.Empty;
    public string PassengerEmail { get; set; } = string.Empty;
    public string PassengerPhone { get; set; } = string.Empty;

    // Detalhes
    public string SeatNumber { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string FlightClass { get; set; } = string.Empty;
    public string BaggageAllowance { get; set; } = "1 bagagem de mão + 1 despachada";
    public string Terminal { get; set; } = string.Empty;
    public string Gate { get; set; } = string.Empty;

    public static ReservationViewModel GetReservationViewModel(Reservation reservation)
    {
        if (reservation == null)
            throw new ArgumentNullException(nameof(reservation));

        var flight = reservation.Flight;
        var client = reservation.Client;
        var seat = reservation.ReservedArmchair;
        var airplane = flight.Airplane;

        return new ReservationViewModel
        {
            Id = reservation.Id,
            ReservationNumber = reservation.CodeRersevation,
            Status = "Confirmada",
            ReservationDate = reservation.DateReservation,

            // Dados do voo
            FlightNumber = flight.CodeFlight,
            Origin = flight.Origin,
            Destination = flight.Destination,
            DepartureTime = flight.Departure,
            ArrivalTime = flight.Arrival,
            Airline = flight.Airline,
            AircraftType = airplane.Name,
            Terminal = flight.Airport.Name,

            // Dados do passageiro
            PassengerName = client.Name,
            PassengerEmail = client.Email,
            PassengerPhone = client.Phone,

            // Detalhes da reserva
            SeatNumber = seat.Code,
            Gate = reservation.Gate,
            Price = (decimal)reservation.Price,
            FlightClass = reservation.Class.ToString(),
            BaggageAllowance = "1 bagagem de mão + 1 despachada",
        };
    }
}


