using EFAereoNuvem.Models;
using EFAereoNuvem.Repository;
using EFAereoNuvem.Repository.Interface;
using EFAereoNuvem.ViewModel;
using EFAereoNuvem.ViewModel.ResponseViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFAereoNuvem.Controllers;

[Authorize]
public class ReservationController(IReservationRepository reservationRepository) : Controller
{
    private readonly IReservationRepository _reservationRepository = reservationRepository;

    // INDEX 
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 25)
    {
        try
        {
            var reservations = await _reservationRepository.GetAllAsync(page, pageSize);

            if (reservations != null)
            {
                var reservationViewModel = reservations
                .Select(ReservationInformationViewModel.GetReservationInformationViewModel)
                .ToList();

                return View(reservationViewModel);
            }

            var response = new ResponseViewModel<List<ReservationInformationViewModel>>([ConstantsMessage.NENHUMA_RESERVA_ENCONTRADA]);
            return NotFound(response);
        }
        catch
        {
            var response = new ResponseViewModel<List<ReservationInformationViewModel>>([ConstantsMessage.ERRO_SERVIDOR]);
            ViewBag.ErrorMessage = response.Messages.FirstOrDefault()?.Message;
            return View(new List<ReservationInformationViewModel>());
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

        var viewModel = ReservationInformationViewModel.GetReservationInformationViewModel(reservation);
        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Client")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Create(Reservation reservation)
    {
        if (!ModelState.IsValid)
        {
            var response = new ResponseViewModel<Reservation>(reservation, ConstantsMessage.ERRO_CADASTRO_RESERVA);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return View(reservation);
        }

        await _reservationRepository.CreateAsync(reservation);
        return RedirectToAction(nameof(Index));
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
        return RedirectToAction(nameof(Index));
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
    public async Task<IActionResult> SearchByFlight(Guid flightId)
    {
        var reservations = await _reservationRepository.GetReservationsByFlightIdAsync(flightId);

        var reservationViewModel = reservations
            .Select(ReservationInformationViewModel.GetReservationInformationViewModel)
            .ToList();

        return View("Index", reservationViewModel);
    }
}
