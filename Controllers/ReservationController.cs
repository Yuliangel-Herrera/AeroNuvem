using EFAereoNuvem.Models;
using EFAereoNuvem.Models.Enum;
using EFAereoNuvem.Repository;
using EFAereoNuvem.Repository.Interface;
using EFAereoNuvem.ViewModel;
using EFAereoNuvem.ViewModel.ResponseViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFAereoNuvem.Controllers;

[Authorize]
public class ReservationController(IReservationRepository reservationRepository, IFlightRepository flightRepository) : Controller
{
    private readonly IReservationRepository _reservationRepository = reservationRepository;
    private readonly IFlightRepository _flightRepository = flightRepository;

    // INDEX 
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 25)
    {
        try
        {
            var reservations = await _reservationRepository.GetAllAsync(page, pageSize);

            if(reservations == null || !reservations.Any())
            {
                ViewBag.Message = "Nenhuma reserva encontrada.";
                return View(new List<ReservationViewModel>());
            }

            var viewModels = reservations
                .Select(ReservationViewModel.GetReservationViewModel)
                .ToList();

            return View(viewModels);
        }
        catch
        {
            var response = new ResponseViewModel<List<ReservationViewModel>>([ConstantsMessage.ERRO_SERVIDOR]);
            ViewBag.ErrorMessage = response.Messages.FirstOrDefault()?.Message;
            return View(new List<ReservationViewModel>());
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Client")]
    public async Task<IActionResult> Details(Guid id)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        if (reservation == null)
        {
            var response = new ResponseViewModel<Reservation?>(ConstantsMessage.NENHUMA_RESERVA_ENCONTRADA);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = ReservationViewModel.GetReservationViewModel(reservation);
        return View(viewModel);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Client")]
    public async Task<IActionResult> Create(Guid flightId)
    {
        if (flightId == Guid.Empty)
            return BadRequest("Id não foi enviado");

        // Buscar o voo selecionado
        var flight = await _flightRepository.GetByIdAsync(flightId);

        if (flight == null)
            return NotFound("Voo não encontrado");

        // Monta o ViewModel já preenchido
        var model = new ReservationViewModel
        {
            FlightId = flight.Id,
            FlightNumber = flight.CodeFlight,
            Origin = flight.Origin,
            Destination = flight.Destination,
            DepartureTime = flight.Departure,
            ArrivalTime = flight.Arrival,
            Airline = flight.Airline,
            AircraftType = flight.Airplane.Name,
            Terminal = flight.DestinationAirport.Name,
            Price = 900
        };

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Client")]
    public async Task<IActionResult> Create(ReservationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var response = new ResponseViewModel<ReservationViewModel>(model, ConstantsMessage.ERRO_CADASTRO_RESERVA);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return View(model);
        }

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            FlightId = model.FlightId,
            DateReservation = DateTime.Now,
            CodeRersevation = Guid.NewGuid().ToString().Substring(0, 8),
            Status = StatusReservation.Confirmada,
            Gate = model.Gate,
            Price = 900,
            Class = Enum.Parse<Class>(model.FlightClass),

            Client = new Client
            {
                Name = model.PassengerName,
                Email = model.PassengerEmail,
                Phone = model.PassengerPhone
            }
        };

        await _reservationRepository.CreateAsync(reservation);
        return RedirectToAction("ReservationConfirmation");
    }


    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {

        var reservation = await _reservationRepository.GetByIdAsync(id);
        if (reservation == null)
        {
            var response = new ResponseViewModel<Reservation?>(ConstantsMessage.NENHUMA_RESERVA_ENCONTRADA);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }

        await _reservationRepository.DeleteAsync(id);
        TempData["SuccessMessage"] = "Reserva cancelada com sucesso!";
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Client")]
    public async Task<IActionResult> SearchByCode(string code)
    {
        var reservation = await _reservationRepository.GetByCode(code);
        if (reservation == null)
        {
            var response = new ResponseViewModel<Reservation?>(ConstantsMessage.NENHUMA_RESERVA_ENCONTRADA);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = ReservationInformationViewModel.GetReservationInformationViewModel(reservation);
        return View("Details", viewModel);
    }

    // Todas as reservas de um determinado voo
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SearchByFlight(string codeFlight)
    {
        if (string.IsNullOrEmpty(codeFlight))
            return View(new List<ReservationViewModel>());

        var reservations = await _reservationRepository.GetReservationsByCodeFlightAsync(codeFlight);

        var reservationViewModel = reservations
            .Select(ReservationViewModel.GetReservationViewModel)
            .ToList();

        return View(reservationViewModel);
    }
}
