using EFAereoNuvem.Services;
using EFAereoNuvem.ViewModel;
using EFAereoNuvem.ViewModel.ResponseViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EFAereoNuvem.Controllers;

public class LoginController : Controller
{
    private readonly AuthService _authService;

    public LoginController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var success = await _authService.LoginAsync(model.Email, model.Password);

        if (!success)
        {
            model.ErrorMessage = "Email ou senha incorretos."; ;
            return View(model); //Passa o LoginViewModel
        }

        return RedirectToAction("Index", "Home"); 
    }

    [HttpPost]
    public IActionResult Logout()
    {
        _authService.Logout();
        return RedirectToAction("Index");
    }
}

