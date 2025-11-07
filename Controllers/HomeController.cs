using EFAereoNuvem.Models;
using EFAereoNuvem.Repository.Interface;
using EFAereoNuvem.ViewModel;
using EFAereoNuvem.ViewModel.ResponseViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EFAereoNuvem.Controllers;
public class HomeController(IFlightRepository flightRepository) : Controller
{
    private readonly IFlightRepository _flightRepository = flightRepository;

    [HttpGet]
    public async Task<IActionResult> Index(string origin, string destination, DateTime? date)
    {
        IEnumerable<Flight> flights = [];

        // Só busca no banco se a pessoa tiver pesquisado (ou seja, preencheu algo)
        if (!string.IsNullOrEmpty(origin) || !string.IsNullOrEmpty(destination) || date.HasValue)
        {
            if (date.HasValue)
            {
                flights = await _flightRepository.GetAvailableFlightsAsync(origin, destination, date.Value);
            }
            else
            {
                flights = await _flightRepository.GetByRouteAsync(origin, destination);
            }

            if (flights == null || !flights.Any())
            {
                var response = new ResponseViewModel<List<Flight>>([ConstantsMessage.NENHUM_VOO_DISPONIVEL]);
                TempData["InfoMessage"] = response.Messages.FirstOrDefault()?.Message;
            }
        }

        // Se ainda não pesquisou, retorna view sem tentar acessar o banco
        var viewModel = flights?
            .Select(FlightViewModel.GetFlightViewModel)
            .ToList() ?? [];

        return View("Index", viewModel);

        //Simulação de dados - depois substitua por dados reais do banco
        //var availableFlights = new List<FlightViewModel>();
        //{
        //     new FlightViewModel
        //     {
        //         Id = Guid.NewGuid(),
        //         CodeFlight = "AZ123",
        //         Origin = "São Paulo (GRU)",
        //         Destination = "Rio de Janeiro (GIG)",
        //         DepartureTime = DateTime.Now.AddDays(1).AddHours(2),
        //         ArrivalTime = DateTime.Now.AddDays(1).AddHours(3).AddMinutes(30),
        //         Price = 450.00m,
        //         AvailableSeats = 24,
        //         Airline = "AeroNuvem",
        //         AircraftType = "Boeing 737"
        //     },
        //     new FlightViewModel
        //     {
        //         Id = Guid.NewGuid(),
        //         CodeFlight = "AZ456",
        //         Origin = "Rio de Janeiro (GIG)",
        //         Destination = "Brasília (BSB)",
        //         DepartureTime = DateTime.Now.AddDays(2).AddHours(8),
        //         ArrivalTime = DateTime.Now.AddDays(2).AddHours(10).AddMinutes(15),
        //         Price = 620.00m,
        //         AvailableSeats = 18,
        //         Airline = "AeroNuvem",
        //         AircraftType = "Airbus A320"
        //     },
        //     new FlightViewModel
        //     {
        //         Id = Guid.NewGuid(),
        //         CodeFlight = "AZ789",
        //         Origin = "São Paulo (GRU)",
        //         Destination = "Salvador (SSA)",
        //         DepartureTime = DateTime.Now.AddDays(3).AddHours(14),
        //         ArrivalTime = DateTime.Now.AddDays(3).AddHours(16).AddMinutes(45),
        //         Price = 780.00m,
        //         AvailableSeats = 12,
        //         Airline = "AeroNuvem",
        //         AircraftType = "Boeing 737"
        //     },
        //     new FlightViewModel
        //     {
        //         Id = Guid.NewGuid(),
        //         CodeFlight = "AZ101",
        //         Origin = "Brasília (BSB)",
        //         Destination = "Fortaleza (FOR)",
        //         DepartureTime = DateTime.Now.AddDays(1).AddHours(18),
        //         ArrivalTime = DateTime.Now.AddDays(1).AddHours(21).AddMinutes(30),
        //         Price = 890.00m,
        //         AvailableSeats = 8,
        //         Airline = "AeroNuvem",
        //         AircraftType = "Airbus A321"
        //     }
        //};

        //return View(availableFlights);
    }
}
