using EFAereoNuvem.Models;
using EFAereoNuvem.Repository;
using EFAereoNuvem.Repository.Interface;
using EFAereoNuvem.ViewModel;
using EFAereoNuvem.ViewModel.ResponseViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EFAereoNuvem.Controllers;
public class FlightController : Controller
{
    private readonly IFlightRepository _flightRepository;
    private readonly IAirplaneRepository _airplaneRepository;
    private readonly IAirportRepository _airportRepository;
    private readonly IScaleRepository _scaleRepository;

    public FlightController(IFlightRepository flightRepository, IAirplaneRepository airplaneRepository, IAirportRepository airportRepository, IScaleRepository scaleRepository)
    {
        _flightRepository = flightRepository;
        _airplaneRepository = airplaneRepository;
        _airportRepository = airportRepository;
        _scaleRepository = scaleRepository;
    }

    // INDEX 
    public async Task<IActionResult> Index(int page = 1, int pageSize = 25)
    {
        try
        {
            var flights = await _flightRepository.GetAllAsync(page, pageSize);
            var flightViewModel = flights.Select(FlightViewModel.GetFlightViewModel).ToList();
            return View(flightViewModel);
        }
        catch
        {
            var response = new ResponseViewModel<List<FlightViewModel>>([ConstantsMessage.ERRO_SERVIDOR]);
            ViewBag.ErrorMessage = response.Messages.FirstOrDefault()?.Message;
            return View(new List<FlightViewModel>());
        }
    }

    // CRUD
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        await LoadAirplanes();
        await LoadAirports();
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(FlightCreateViewModel model, List<ScaleViewModel> scales)
    {
        if (!ModelState.IsValid)
        {
            await LoadAirplanes();
            await LoadAirports();
            return View(model);
        }

        try
        {
            // Montar a entidade Flight a partir do ViewModel
            var flight = new Flight
            {
                CodeFlight = model.CodeFlight,
                TypeFlight = model.TypeFlight,
                Origin = model.Origin,
                Destination = model.Destination,
                OriginAirportId = model.OriginAirportId,
                DestinationAirportId = model.DestinationAirportId,
                DateFlight = model.DateFlight,
                Departure = CombineDateAndTime(model.DateFlight, model.Departure),
                Arrival = CombineDateAndTime(model.DateFlight, model.Arrival),
                Duration = model.Duration,
                ExistScale = model.ExistScale,
                AirplaneId = model.AirplaneId,
                RealDeparture = CombineDateAndTime(model.DateFlight, model.Departure),
                RealArrival = CombineDateAndTime(model.DateFlight, model.Arrival)
            };

            // Escalas (se houver)
            if (model.ExistScale && scales != null && scales.Any())
            {
                flight.Scales = scales.Select(scale => new Scale
                {
                    Location = scale.Location,
                    Departure = CombineDateAndTime(model.DateFlight, scale.Departure),
                    Arrival = CombineDateAndTime(model.DateFlight, scale.Arrival),
                    RealDeparture = CombineDateAndTime(model.DateFlight, scale.RealDeparture),
                    RealArrival = CombineDateAndTime(model.DateFlight, scale.RealArrival),

                }).ToList();
            }

            await _flightRepository.CreateAsync(flight);

            var response = new ResponseViewModel<Flight>(flight, ConstantsMessage.VOO_CADASTRADO_COM_SUCESSO);
            TempData["SuccessMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            await LoadAirplanes();
            var response = new ResponseViewModel<FlightCreateViewModel>(model, ConstantsMessage.ERRO_CADASTRO_VOO);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return View(model);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var flight = await _flightRepository.GetByIdWithScales(id);
        if (flight == null)
        {
            var response = new ResponseViewModel<Flight?>(data: null, message: ConstantsMessage.NENHUM_VOO_ENCONTRADO);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }

        await LoadAirplanes();
        return View(flight);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(Guid id, Flight flight, List<ScaleViewModel> scales)
    {
        if (id != flight.Id)
        {
            var response = new ResponseViewModel<Flight>(ConstantsMessage.VOO_NAO_ENCONTRADO);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            await LoadAirplanes();
            return View(flight);
        }

        try
        {
            // Atualizar escalas se necessário
            if (flight.ExistScale && scales != null && scales.Any())
            {
                // Remover escalas existentes
                await _scaleRepository.DeleteByFlightId(flight.Id);

                // Adicionar novas escalas
                flight.Scales = scales.Select(scale => new Scale
                {
                    Location = scale.Location,
                    Arrival = CombineDateAndTime(flight.DateFlight, scale.Arrival),
                    Departure = CombineDateAndTime(flight.DateFlight, scale.Departure),
                    RealArrival = CombineDateAndTime(flight.DateFlight, scale.Arrival),
                    RealDeparture = CombineDateAndTime(flight.DateFlight, scale.Departure),
                    FlightId = flight.Id
                }).ToList();
            }
            else
            {
                // Se não possui mais escalas, remover as existentes
                await _scaleRepository.DeleteByFlightId(flight.Id);
                flight.Scales = new List<Scale>();
            }

            await _flightRepository.UpdateAsync(flight);

            var response = new ResponseViewModel<Flight>(flight, ConstantsMessage.VOO_ATUALIZADO_COM_SUCESSO);
            TempData["SuccessMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            await LoadAirplanes();
            var response = new ResponseViewModel<Flight>(flight, ConstantsMessage.ERRO_AO_ATUALIZAR_VOO);

            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return View(flight);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Client")]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var flight = await _flightRepository.GetByIdWithScales(id);
            if (flight == null)
            {
                var response = new ResponseViewModel<Flight?>(ConstantsMessage.NENHUM_VOO_ENCONTRADO);
                TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }
        catch
        {
            var response = new ResponseViewModel<Flight?>(ConstantsMessage.ERRO_SERVIDOR);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var flight = await _flightRepository.GetByIdWithScales(id);

            if (flight == null)
            {
                var res = new ResponseViewModel<Flight?>(ConstantsMessage.NENHUM_VOO_ENCONTRADO);
                TempData["ErrorMessage"] = res.Messages.FirstOrDefault()?.Message;
                return RedirectToAction(nameof(Index));
            }

            // Não permitie excluir se houver reservas associadas
            if (flight.Reservations != null && flight.Reservations.Any())
            {
                var res = new ResponseViewModel<Flight>(ConstantsMessage.ERRO_AO_DELETAR_VOO);
                TempData["ErrorMessage"] = res.Messages.FirstOrDefault()?.Message;
                return RedirectToAction(nameof(Index));
            }

            await _flightRepository.DeleteAsync(id);

            var response = new ResponseViewModel<Flight>(ConstantsMessage.VOO_DELETADO_COM_SUCESSO);
            TempData["SuccessMessage"] = response.Messages.FirstOrDefault()?.Message;
        }
        catch (Exception ex)
        {
            var response = new ResponseViewModel<Flight?>(ConstantsMessage.ERRO_AO_DELETAR_VOO);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // FILTROS E CONSULTAS
    [HttpGet]
    public async Task<IActionResult> AvailableFlights(string origin, string destination, DateTime date)
    {
        var flights = await _flightRepository.GetAvailableFlightsAsync(origin, destination, date);
        if (!flights.Any())
        {
            var response = new ResponseViewModel<List<Flight>>([ConstantsMessage.NENHUM_VOO_DISPONIVEL]);
            TempData["InfoMessage"] = response.Messages.FirstOrDefault()?.Message;
        }

        return View("flights", flights);
    }

    [HttpGet]
    public async Task<IActionResult> SearchByRoute(string origin, string destination)
    {
        var flights = await _flightRepository.GetByRouteAsync(origin, destination);

        if (flights == null || !flights.Any())
        {
            var response = new ResponseViewModel<List<Flight>>([ConstantsMessage.NENHUM_VOO_DISPONIVEL]);
            TempData["InfoMessage"] = response.Messages.FirstOrDefault()?.Message;
        }
        return View("flights", flights);
    }

    [HttpGet]
    public async Task<IActionResult> SearchByDate(DateTime date)
    {
        var flights = await _flightRepository.GetByDateAsync(date);
        return View("Index", flights);
    }

    [HttpGet]
    public async Task<IActionResult> DirectFlights() // origin, destination
    {
        var flights = await _flightRepository.GetDirectFlightsAsync();
        return View("Index", flights);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Client")]
    public async Task<IActionResult> SearchByCode(string code)
    {
        var flight = await _flightRepository.GetByCode(code);
        if (flight == null)
        {
            var response = new ResponseViewModel<Flight?>(data: null, message: ConstantsMessage.NENHUM_VOO_ENCONTRADO);
            TempData["InfoMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }
        return View("Details", flight);
    }

    // AUXIIARES
    private async Task LoadAirplanes(int page = 1, int pageSize = 25)
    {
        var airplanes = await _airplaneRepository.GetAll(page, pageSize);
        ViewBag.Airplanes = new SelectList(airplanes, "Id", "Prefix");
    }

    private async Task LoadAirports(int page = 1, int pageSize = 25)
    {
        var airports = await _airportRepository.GetAll(page, pageSize);
        ViewBag.Airports = airports
            .Select(a => new { a.Id, Display = $"{a.Name} ({a.IATA}) "})
            .ToList();
        ViewBag.Airports = new SelectList(airports, "Id", "Name");
    }

    private DateTime CombineDateAndTime(DateTime date, TimeSpan time)
    {
        return date.Date
            .AddHours(time.Hours)
               .AddMinutes(time.Minutes)
               .AddSeconds(time.Seconds);
    }
}