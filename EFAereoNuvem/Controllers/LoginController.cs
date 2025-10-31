using EFAereoNuvem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EFAereoNuvem.Controllers;
public class LoginController : Controller
{
    private readonly AuthService _authService;

    public LoginController(AuthService authService)
    {
        _authService = authService;
    }

    // Exibe a tela de login
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // Faz login chamando o AuthService
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var success = await _authService.LoginAsync(email, password);

        if (!success)
        {
            ViewBag.Error = "Email ou senha inválidos!";
            return View("Index");
        }

        return RedirectToAction("Dashboard", "Home");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        _authService.Logout();
        return RedirectToAction("Index");
    }
}
