using EFAereoNuvem.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFAereoNuvem.Controllers;
public class DashboardController(IFlightRepository flightRepository) : Controller
{
    private readonly IFlightRepository _flightRepository = flightRepository;

    [HttpGet("/ClientDashboard")]
    [Authorize(Roles = "Client")]
    public IActionResult ClientDashboard()
    {
        return View("ClientDashboard");
    }

    [HttpGet("/AdminDashboard")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminDashboard()
    {
        return View("AdminDashboard");
    }
}