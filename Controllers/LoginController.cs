using EFAereoNuvem.Data;
using EFAereoNuvem.Models;
using EFAereoNuvem.Services;
using EFAereoNuvem.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EFAereoNuvem.Controllers;

public class LoginController : Controller
{
    private readonly AppDBContext _context;

    public LoginController(AppDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        Console.WriteLine("=== TENTATIVA DE LOGIN ===");
        Console.WriteLine($"Email: {model?.Email}");

        if (!ModelState.IsValid)
        {
            Console.WriteLine("ModelState inválido");
            return View(model);
        }

        try
        {
            // Busca o usuário no banco (com roles)
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                Console.WriteLine("❌ Usuário não encontrado");
                ModelState.AddModelError(string.Empty, "Email ou senha incorretos.");
                return View(model);
            }

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                Console.WriteLine("❌ PasswordHash ausente para o usuário");
                ModelState.AddModelError(string.Empty, "Email ou senha incorretos.");
                return View(model);
            }

            // Verifica a senha
            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                Console.WriteLine("❌ Senha inválida");
                ModelState.AddModelError(string.Empty, "Email ou senha incorretos.");
                return View(model);
            }

            // Cria claims
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            bool isAdmin = false;
            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    if (!string.IsNullOrEmpty(role?.TypeRole))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.TypeRole));
                        if (role.TypeRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                            isAdmin = true;
                    }
                }
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            // Efetua login
            await HttpContext.SignInAsync(
                "CookieAuth",
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            HttpContext.Session.SetString("UserEmail", user.Email ?? "");
            HttpContext.Session.SetString("UserName", user.Username ?? "");

            Console.WriteLine("✅ Login bem-sucedido");

            TempData["SuccessMessage"] = "Login realizado com sucesso!";

            // 🔹 Redirecionamento condicional
            if (isAdmin)
            {
                Console.WriteLine("🔹 Redirecionando para Dashboard do Admin");
                return RedirectToAction("AdminDashboard", "Dashboard");
            }
            else
            {
                Console.WriteLine("🔹 Redirecionando para Dashboard do Cliente");
                return RedirectToAction("ClientDashboard", "Dashboard");
            }
        }
        catch (Exception ex)
        {
            // Log completo para debug
            Console.WriteLine($"❌ Erro no login: {ex.GetType().FullName}: {ex.Message}");
            Console.WriteLine(ex.StackTrace);

            // Se tiver inner exception
            if (ex.InnerException != null)
            {
                Console.WriteLine("--- InnerException ---");
                Console.WriteLine(ex.InnerException.Message);
            }

            ModelState.AddModelError(string.Empty, "Erro interno do sistema. Tente novamente.");
            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        HttpContext.Session.Clear();
        TempData["SuccessMessage"] = "Logout realizado com sucesso!";
        return RedirectToAction("Index", "Home");
    }
}
