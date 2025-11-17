using EFAereoNuvem.Data;
using EFAereoNuvem.Models;
using EFAereoNuvem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

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
            // 1️⃣ Verifica se o e-mail já existe
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Este email já está cadastrado.");
                return View(model);
            }

            // 2️⃣ Cria hash da senha
            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(null!, model.Password);

            // 3️⃣ Busca (ou cria) a role "Client"
            var clientRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.TypeRole == "Client");

            if (clientRole == null)
            {
                clientRole = new Role { TypeRole = "Client" };
                _context.Roles.Add(clientRole);
                await _context.SaveChangesAsync();
            }

            // 4️⃣ Cria o usuário
            var user = new User
            {
                Username = model.Name,
                Email = model.Email ?? "",
                PasswordHash = hashedPassword,
                Roles = new List<Role> { clientRole }
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // precisa do ID do usuário

            // 5️⃣ Cria o endereço atual
            var adress = new Adress
            {
                Street = model.Street,
                Number = model.Number,
                Complement = model.Complement,
                City = model.City,
                State = model.State,
                Country = model.Country,
                Cep = model.Cep
            };

            _context.Adresses.Add(adress);
            await _context.SaveChangesAsync(); // precisa do ID do endereço

            // 6️⃣ Cria o cliente vinculado ao usuário e ao endereço
            var client = new Client
            {
                Name = model.Name,
                Cpf = new string(model.Cpf.Where(char.IsDigit).ToArray()),
                Email = model.Email ?? "",
                Phone = model.Phone ?? "",
                BornDate = DateOnly.FromDateTime(model.BornDate.Date),
                Status = EFAereoNuvem.Models.Enum.Status.Bronze,
                Priority = false,
                Discount = 0,
                CurrentAdressId = adress.Id,
                UserId = user.Id
            };

            _context.Clients.Add(client);

            // 7️⃣ Atualiza o User com referência para o Client
            user.Client = client;

            // 8️⃣ Salva tudo
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Conta criada com sucesso!";
            return RedirectToAction("Success", "CreateAccount");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao criar conta: {ex.Message}");
            ModelState.AddModelError(string.Empty, "Erro interno ao criar conta. Tente novamente.");
            return View(model);
        }
    }



    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult RegisterAdmin()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterAdmin(ClientCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            Console.WriteLine($"Tentando registrar administrador: {model.Email}");

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
            var adminRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.TypeRole == "Admin");

            if (adminRole == null)
            {
                Console.WriteLine("⚠️ Nenhuma role 'Admin' encontrado, criando...");
                adminRole = new Role { TypeRole = "Admin" };
                _context.Roles.Add(adminRole);
                await _context.SaveChangesAsync();
            }

            // Cria novo usuário
            var user = new User
            {
                Username = model.Name,
                Email = model.Email,
                PasswordHash = hashedPassword,
                Roles = new List<Role> { adminRole }
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
