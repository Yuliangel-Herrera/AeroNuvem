using EFAereoNuvem.Data;
using EFAereoNuvem.Models;
using EFAereoNuvem.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace EFAereoNuvem.Controllers;
public class CreateAccountController : Controller
{
    private readonly AppDBContext _context;

    public CreateAccountController(AppDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(ClientCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            Console.WriteLine($"Tentando registrar usuário: {model.Email}");

            // Verifica se já existe
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                Console.WriteLine("❌ Email já cadastrado");
                ModelState.AddModelError(string.Empty, "Este email já está cadastrado.");
                return View(model);
            }

            // Cria hash da senha
            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(null!, model.Password);

            // Busca role Client
            var clientRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.TypeRole == "Client");

            if (clientRole == null)
            {
                Console.WriteLine("⚠️ Nenhuma role 'Client' encontrada, criando...");
                clientRole = new Role { TypeRole = "Client" };
                _context.Roles.Add(clientRole);
                await _context.SaveChangesAsync();
            }

            // Cria novo usuário
            var user = new User
            {
                Username = model.Name,
                Email = model.Email,
                PasswordHash = hashedPassword,
                Roles = new List<Role> { clientRole }
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Success", "CreateAccount");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao criar conta: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");

            ModelState.AddModelError(string.Empty, "Erro interno ao criar conta. Tente novamente.");
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Success()
    {
        return View();
    }
}
