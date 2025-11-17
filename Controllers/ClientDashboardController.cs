using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFAereoNuvem.Controllers;

[Authorize(Roles = "Client")]
public class ClientDashboardController : Controller
{
    public IActionResult Promocoes()
    {
        return View();
    }

    public IActionResult SejaPremium()
    {
        return View();
    }
}
