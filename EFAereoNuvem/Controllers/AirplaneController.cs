using EFAereoNuvem.Models;
using EFAereoNuvem.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EFAereoNuvem.Controllers
{
    public class AirplaneController : Controller
    {
        private readonly IAirplaneRepository _airplaneRepository;

        public AirplaneController(IAirplaneRepository airplaneRepository)
        {
            _airplaneRepository = airplaneRepository;
        }

        public async Task<IActionResult> Index()
        {
            var airplanes = await _airplaneRepository.GetAll();
            return View(airplanes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Airplane airplane)
        {
            if (!ModelState.IsValid)
                return View(airplane);

            try
            {
                await _airplaneRepository.Create(airplane);
                TempData["SuccessMessage"] = "Aeronave cadastrada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao cadastrar aeronave: {ex.Message}";
                return View(airplane);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var airplane = await _airplaneRepository.GetById(id);
            if (airplane == null)
            {
                TempData["ErrorMessage"] = "Aeronave não encontrada.";
                return RedirectToAction(nameof(Index));
            }
            return View(airplane);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Airplane airplane)
        {
            if (id != airplane.Id)
            {
                TempData["ErrorMessage"] = "ID inválido.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(airplane);

            try
            {
                await _airplaneRepository.Update(airplane);
                TempData["SuccessMessage"] = "Aeronave atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao atualizar aeronave: {ex.Message}";
                return View(airplane);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _airplaneRepository.Delete(id);
                TempData["SuccessMessage"] = "Aeronave excluída com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao excluir aeronave: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}