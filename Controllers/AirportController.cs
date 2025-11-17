using EFAereoNuvem.Models;
using EFAereoNuvem.Repository.Interface;
using EFAereoNuvem.ViewModel;
using EFAereoNuvem.ViewModel.ResponseViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EFAereoNuvem.Controllers;
public class AirportController(IAirportRepository airportRepository) : Controller
{
    private readonly IAirportRepository _airportRepository = airportRepository;

    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 25)
    {
        try
        {
            var airports = await _airportRepository.GetAll(pageNumber, pageSize);
            return View(airports);
        }
        catch
        {
            var response = new ResponseViewModel<List<FlightViewModel>>([ConstantsMessage.ERRO_SERVIDOR]);
            ViewBag.ErrorMessage = response.Messages.FirstOrDefault()?.Message;
            return View(new List<Airplane>());
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(AirportCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        try
        {
            // Cria o endereço a partir da ViewModel
            var address = new Adress
            {
                Street = model.Street,
                Number = model.Number,
                Complement = model.Complement,
                City = model.City,
                State = model.State,
                Country = model.Country,
                Cep = model.Cep
            };

            // Cria o aeroporto
            var airport = new Airport
            {
                Name = model.Name,
                IATA = model.IATA,
                Adress = address 
            };

            await _airportRepository.CreateAsync(airport);

            var response = new ResponseViewModel<Airport>(airport, ConstantsMessage.AEROPORTO_CADASTRADO_COM_SUCESSO);
            TempData["SuccessMessage"] = response.Messages.FirstOrDefault()?.Message;

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            var response = new ResponseViewModel<Airport>(ConstantsMessage.ERRO_CADASTRO_AEROPORTO);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return View(model);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var airport = await _airportRepository.GetByIdAsync(id);
        if (airport == null)
        {
            var response = new ResponseViewModel<Airport>(ConstantsMessage.AEROPORTO_NAO_ENCONTRADO);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }

        var model = AirportCreateViewModel.GetAirportViewModel(airport);
        ViewBag.AirportId = airport.Id;

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, AirportCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var existingAirport = await _airportRepository.GetByIdAsync(id);
        if (existingAirport == null)
        {
            var response = new ResponseViewModel<Airport>(ConstantsMessage.AEROPORTO_NAO_ENCONTRADO);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }

        try
        {
            // Atualiza dados do aeroporto
            existingAirport.Name = model.Name;
            existingAirport.IATA = model.IATA;

            // Atualiza dados do endereço
            existingAirport.Adress.Street = model.Street;
            existingAirport.Adress.Number = model.Number;
            existingAirport.Adress.Complement = model.Complement;
            existingAirport.Adress.City = model.City;
            existingAirport.Adress.State = model.State;
            existingAirport.Adress.Country = model.Country;
            existingAirport.Adress.Cep = model.Cep;

            await _airportRepository.UpdateAsync(existingAirport);

            var response = new ResponseViewModel<Airport>(existingAirport, ConstantsMessage.AEROPORTO_ATUALIZADO_COM_SUCESSO);
            TempData["SuccessMessage"] = response.Messages.FirstOrDefault()?.Message;

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            var response = new ResponseViewModel<Airport>(ConstantsMessage.ERRO_AO_ATUALIZAR_AEROPORTO);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;

            return View(model);
        }
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var airport = await _airportRepository.GetByIdAsync(id);

            if (airport == null)
            {
                var respose = new ResponseViewModel<Airplane>(ConstantsMessage.AERONAVE_NAO_ENCONTRADA);
                TempData["ErrorMessage"] = respose.Messages.FirstOrDefault()?.Message;
                return RedirectToAction(nameof(Index));
            }

            await _airportRepository.DeleteAsync(id);
            var response = new ResponseViewModel<Airport>(ConstantsMessage.AEROPORTO_EXCLUIDO_COM_SUCESSO);
            TempData["SuccessMessage"] = response.Messages.FirstOrDefault()?.Message;
        }
        catch
        {
            var response = new ResponseViewModel<Airport>(ConstantsMessage.ERRO_AO_EXCLUIR_AEROPORTO);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> Details(string IATA)
    {
        try
        {
            if (string.IsNullOrEmpty(IATA))
            {
                TempData["ErrorMessage"] = "Informe um código IATA para buscar.";
                return RedirectToAction(nameof(Index));
            }

            var airport = await _airportRepository.GetByIATA(IATA);
            if (airport == null)
            {
                var response = new ResponseViewModel<Airport>(ConstantsMessage.NENHUM_AEROPORTO_ENCONTRADO);
                TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
                return RedirectToAction(nameof(Index));
            }

            ViewBag.BuscaAtiva = !string.IsNullOrEmpty(IATA);
            return View("Details", airport);
        }
        catch
        {
            var response = new ResponseViewModel<Airport>(ConstantsMessage.ERRO_SERVIDOR);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
