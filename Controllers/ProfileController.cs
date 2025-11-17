using EFAereoNuvem.Data;
using EFAereoNuvem.Models;
using EFAereoNuvem.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFAereoNuvem.Controllers;

public class ProfileController(IClientRepository clientRepository, AppDBContext context) : Controller
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly AppDBContext _context = context;

    // ==================== GET: PROFILE ====================
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        try
        {
            var userEmail = User.Identity?.Name;

            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["ErrorMessage"] = "Usuário não autenticado.";
                return RedirectToAction("Index", "Login");
            }

            // Busca o usuário logado
            var user = await _context.Users
                .Include(u => u.Client)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                TempData["ErrorMessage"] = "Usuário não encontrado.";
                return RedirectToAction("Index", "Home");
            }

            return View("Index", user);
        }
        catch
        {
            TempData["ErrorMessage"] = "Erro ao carregar o perfil.";
            return RedirectToAction("Index", "Home");
        }
    }

    // ==================== POST: PROFILE ====================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(User updatedUser)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(updatedUser);

            var userEmail = User.Identity?.Name;

            var existingUser = await _context.Users
                .Include(u => u.Client)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (existingUser == null)
            {
                TempData["ErrorMessage"] = "Usuário não encontrado.";
                return RedirectToAction("Index", "Home");
            }

            // Atualiza informações básicas do usuário
            existingUser.Username = updatedUser.Username;
            existingUser.Email = updatedUser.Email;

            // Atualiza cliente se for do tipo Client
            if (existingUser.Client != null && updatedUser.Client != null)
            {
                existingUser.Client.Phone = updatedUser.Client.Phone;
                existingUser.Client.Cpf = updatedUser.Client.Cpf;
                existingUser.Client.BornDate = updatedUser.Client.BornDate;
            }

            _context.Update(existingUser);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Perfil atualizado com sucesso!";
            return RedirectToAction(nameof(Profile));
        }
        catch
        {
            TempData["ErrorMessage"] = "Erro ao atualizar o perfil.";
            return View(updatedUser);
        }
    }
}