using EFAereoNuvem.Models;
using EFAereoNuvem.Repository.Interface;
using EFAereoNuvem.ViewModel;
using EFAereoNuvem.ViewModel.ResponseViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EFAereoNuvem.Controllers;
public class ClientDashboardController(IFlightRepository flightRepository) : Controller
{
    private readonly IFlightRepository _flightRepository = flightRepository;

    [HttpGet]
    public IActionResult ClientDashboard()
    {
        return View("ClientDashboard");
    }

    [HttpGet]
    public IActionResult AdminDashboard()
    {
        return View("AdminDashboard");
    }

    //[HttpGet]
    //public IActionResult MyReservations()
    //{
    //    // Simulação de dados de reservas - substitua por dados reais do banco
    //    var reservations = new List<ReservationViewModel>
    //    {
    //        new ReservationViewModel
    //        {
    //            Id = Guid.NewGuid(),
    //            ReservationNumber = "RES001",
    //            Status = "Confirmada",
    //            ReservationDate = DateTime.Now.AddDays(-2),
    //            FlightNumber = "AZ123",
    //            Origin = "São Paulo (GRU)",
    //            Destination = "Rio de Janeiro (GIG)",
    //            DepartureTime = DateTime.Now.AddDays(1).AddHours(2),
    //            ArrivalTime = DateTime.Now.AddDays(1).AddHours(3).AddMinutes(30),
    //            PassengerName = "João Silva",
    //            PassengerEmail = "joao.silva@email.com",
    //            PassengerPhone = "(11) 99999-9999",
    //            SeatNumber = "15A",
    //            Price = 450.00m,
    //            Airline = "AeroNuvem",
    //            AircraftType = "Boeing 737",
    //            FlightClass = "Econômica",
    //            BaggageAllowance = "1 bagagem de mão + 1 despachada",
    //            Terminal = "2",
    //            Gate = "B15"
    //        },
    //        new ReservationViewModel
    //        {
    //            Id = Guid.NewGuid(),
    //            ReservationNumber = "RES002",
    //            Status = "Confirmada",
    //            ReservationDate = DateTime.Now.AddDays(-1),
    //            FlightNumber = "AZ456",
    //            Origin = "Rio de Janeiro (GIG)",
    //            Destination = "Brasília (BSB)",
    //            DepartureTime = DateTime.Now.AddDays(2).AddHours(8),
    //            ArrivalTime = DateTime.Now.AddDays(2).AddHours(10).AddMinutes(15),
    //            PassengerName = "João Silva",
    //            PassengerEmail = "joao.silva@email.com",
    //            PassengerPhone = "(11) 99999-9999",
    //            SeatNumber = "22C",
    //            Price = 620.00m,
    //            Airline = "AeroNuvem",
    //            AircraftType = "Airbus A320",
    //            FlightClass = "Econômica",
    //            BaggageAllowance = "1 bagagem de mão + 1 despachada",
    //            Terminal = "1",
    //            Gate = "A08"
    //        }
    //    };

    //    return View(reservations);
    //}

    //[HttpGet]
    //public IActionResult ReservationDetails(Guid id)
    //{
    //    // Simulação de busca de reserva por ID - substitua por busca real no banco
    //    var reservation = new ReservationViewModel
    //    {
    //        Id = id,
    //        ReservationNumber = "RES001",
    //        Status = "Confirmada",
    //        ReservationDate = DateTime.Now.AddDays(-2),
    //        FlightNumber = "AZ123",
    //        Origin = "São Paulo (GRU)",
    //        Destination = "Rio de Janeiro (GIG)",
    //        DepartureTime = DateTime.Now.AddDays(1).AddHours(2),
    //        ArrivalTime = DateTime.Now.AddDays(1).AddHours(3).AddMinutes(30),
    //        PassengerName = "João Silva",
    //        PassengerEmail = "joao.silva@email.com",
    //        PassengerPhone = "(11) 99999-9999",
    //        SeatNumber = "15A",
    //        Price = 450.00m,
    //        Airline = "AeroNuvem",
    //        AircraftType = "Boeing 737",
    //        FlightClass = "Econômica",
    //        BaggageAllowance = "1 bagagem de mão + 1 despachada",
    //        Terminal = "2",
    //        Gate = "B15"
    //    };

    //    return View(reservation);
    //}
}