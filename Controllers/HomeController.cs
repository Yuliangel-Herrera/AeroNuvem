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
    public async Task<IActionResult> Index(string origin, string destination, DateTime? date, int page = 1, int pageSize = 25)
    {
        IEnumerable<Flight> flights;

        // 🟦 1. Nenhum filtro → lista todos os voos
        bool noFilters = string.IsNullOrEmpty(origin) &&
                         string.IsNullOrEmpty(destination) &&
                         !date.HasValue;

        if (noFilters)
        {
            flights = await _flightRepository.GetAllAsync(page, pageSize);
        }
        else
        {
            // Filtro origem/destino + data 
            if (!string.IsNullOrEmpty(origin) &&
                !string.IsNullOrEmpty(destination) &&
                date.HasValue)
            {
                flights = await _flightRepository.GetAvailableFlightsAsync(origin, destination, date.Value);
            }
            // Filtro origem/destino 
            else if (!string.IsNullOrEmpty(origin) &&
                     !string.IsNullOrEmpty(destination))
            {
                flights = await _flightRepository.GetByRouteAsync(origin, destination);
            }
            else
            {
                flights = Enumerable.Empty<Flight>();
                TempData["InfoMessage"] = "Preencha origem e destino para buscar.";
            }
        }

        // Se não encontrou
        if (!flights.Any())
        {
            var response = new ResponseViewModel<List<Flight>>([ConstantsMessage.NENHUM_VOO_DISPONIVEL]);
            TempData["InfoMessage"] = response.Messages.First().Message;
        }

        var vm = flights.Select(FlightViewModel.GetFlightViewModel).ToList();
        return View(vm);
    }
}
