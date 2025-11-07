using System;
using System.Collections.Generic;
using EFAereoNuvem.Models.Enum;

namespace EFAereoNuvem.Models;
public class Flight
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CodeFlight { get; set; } = string.Empty;
    public TypeFlight TypeFlight { get; set; } = new();
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime DateFlight { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Departure { get; set; }
    public DateTime RealArrival { get; set; }
    public DateTime RealDeparture { get; set; }
    public bool ExistScale { get; set; } = true;
    public float Duration { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Reservation> Reservations { get; set; } = []; 
    public List<Scale> Scales { get; set; } = [];
    public Guid AirplaneId { get; set; }
    public Airplane Airplane { get; set; } = new();
    public string Airline { get; internal set; } = "AereoNuvem";
    public DateTime DepartureTime { get; internal set; }
    public Airport Airport { get; internal set; } = new();
}