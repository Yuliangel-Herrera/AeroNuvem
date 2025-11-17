using EFAereoNuvem.Models;
using EFAereoNuvem.Repository.Interface;
using EFAereoNuvem.ViewModel;
using EFAereoNuvem.ViewModel.ResponseViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFAereoNuvem.Controllers;
public class AirplaneController(IAirplaneRepository airplaneRepository) : Controller
{
    private readonly IAirplaneRepository _airplaneRepository = airplaneRepository;

    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 25)
    {
        try
        {
            var airplanes = await _airplaneRepository.GetAll(pageNumber, pageSize);
            return View(airplanes);
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
    public async Task<IActionResult> Create(Airplane airplane)
    {
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
                return View(airplane);
            }
        }


        try
        {
            await _airplaneRepository.CreateAsync(airplane);

            var response = new ResponseViewModel<Airplane>(airplane, ConstantsMessage.AERONAVE_CADASTRADA_COM_SUCESSO);
            TempData["SuccessMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            var response = new ResponseViewModel<Airplane>(airplane, ConstantsMessage.ERRO_CADASTRO_AERONAVE);

            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return View(airplane);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var airplane = await _airplaneRepository.GetById(id);
        if (airplane == null)
        {
            var response = new ResponseViewModel<Airplane>(ConstantsMessage.AERONAVE_NAO_ENCONTRADA);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }
        return View(airplane);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(Guid id, Airplane airplane)
    {
        if (id != airplane.Id)
        {
            var response = new ResponseViewModel<Airplane>(ConstantsMessage.AERONAVE_NAO_ENCONTRADA);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
            return View(airplane);

        try
        {
            await _airplaneRepository.UpdateAsync(airplane);

            var response = new ResponseViewModel<Airplane>(airplane, ConstantsMessage.AERONAVE_ATUALIZADA_COM_SUCESSO);
            TempData["SuccessMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            var response = new ResponseViewModel<Airplane>(airplane, ConstantsMessage.ERRO_AO_ATUALIZAR_AERONAVE);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return View(airplane);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var airplane = await _airplaneRepository.GetById(id);

            if (airplane == null)
            {
                var respose = new ResponseViewModel<Airplane>(ConstantsMessage.AERONAVE_NAO_ENCONTRADA);
                TempData["ErrorMessage"] = respose.Messages.FirstOrDefault()?.Message;
                return RedirectToAction(nameof(Index));
            }

            // Não permitie excluir se houver voos associados
            if (airplane.Flights != null && airplane.Flights.Any())
            {
                var res = new ResponseViewModel<Airplane>(ConstantsMessage.ERRO_EXCLUIR_AERONAVE_COM_VOOS);
                TempData["ErrorMessage"] = res.Messages.FirstOrDefault()?.Message;
                return RedirectToAction(nameof(Index));
            }

            await _airplaneRepository.DeleteAsync(id);
            var response = new ResponseViewModel<Airplane>(ConstantsMessage.AERONAVE_EXCLUIDA_COM_SUCESSO);
            TempData["SuccessMessage"] = response.Messages.FirstOrDefault()?.Message;
        }
        catch 
        {
            var response = new ResponseViewModel<Airplane>(ConstantsMessage.ERRO_AO_EXCLUIR_AERONAVE);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    // Consulta de voos programados para uma aeronave específica
    [HttpGet("Airplane/Schedule/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetFlightSchedule(Guid id)
    {
        try
        {
            var airplane = await _airplaneRepository.GetById(id);
            if (airplane == null)
            {
                var response = new ResponseViewModel<Airplane>(ConstantsMessage.AERONAVE_NAO_ENCONTRADA);
                TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
                return RedirectToAction(nameof(Index));
            }

            var schedule = await _airplaneRepository.GetFlightScheduleAsync(id);

            if (schedule == null || !schedule.Any())
            {
                var response = new ResponseViewModel<Airplane>(ConstantsMessage.NENHUM_VOO_ENCONTRADO);
                ViewBag.InfoMessage = response.Messages.FirstOrDefault()?.Message;
            }

            ViewBag.Airplane = airplane;
            return View("Schedule", schedule);
        }
        catch
        {
            var response = new ResponseViewModel<Airplane>(ConstantsMessage.ERRO_SERVIDOR);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}