using EFAereoNuvem.Models;
using EFAereoNuvem.Models.Enum;

namespace EFAereoNuvem.ViewModel;
public class ReservationViewModel
{
    public Guid Id { get; set; }
    public string ReservationNumber { get; set; } = string.Empty;
    public StatusReservation Status { get; set; } = StatusReservation.Confirmada;
    public DateTime DateReservation { get; set; }
    public Class Class { get; set; }

    // Voo
    public Guid FlightId { get; set; }
    public string FlightNumber { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public Airport OriginAirport { get; set; } = null!;
    public Airport DestinationAirport { get; set; } = null!;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string Airline { get; set; } = string.Empty;
    public string AircraftType { get; set; } = string.Empty;

    // Passageiro
    public Guid ClientId { get; set; }
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
        var flight = reservation.Flight;
        var client = reservation.Client;
        var seat = reservation.ReservedArmchair;
        var airplane = flight.Airplane;

        return new ReservationViewModel
        {
            Id = reservation.Id,
            ReservationNumber = reservation.CodeRersevation,
            Status = StatusReservation.Confirmada,
            DateReservation = reservation.DateReservation,
            Class = reservation.Class,

            // Dados do voo
            FlightId = flight.Id,
            FlightNumber = flight.CodeFlight,
            Origin = flight.Origin,
            Destination = flight.Destination,
            DepartureTime = flight.Departure,
            OriginAirport = flight.OriginAirport,
            DestinationAirport = flight.DestinationAirport,
            ArrivalTime = flight.Arrival,
            Airline = flight.Airline,
            AircraftType = airplane.Name,
            Terminal = flight.DestinationAirport.Name,

            // Dados do passageiro
            ClientId = client.Id,
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


