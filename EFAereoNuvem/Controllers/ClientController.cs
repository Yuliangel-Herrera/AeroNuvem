using EFAereoNuvem.Models;
using EFAereoNuvem.Repository.Interface;
using EFAereoNuvem.ViewModel;
using EFAereoNuvem.ViewModel.ResponseViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFAereoNuvem.Controllers;

[Authorize(Roles = "Admin")]
public class ClientController : Controller
{
    private readonly IClientRepository _clientRepository;

    public ClientController(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    // ==================== INDEX ====================
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 25)
    {
        try
        {
            var clients = await _clientRepository.GetAll(page, pageSize);
            var totalClients = await _clientRepository.Count();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalClients / (double)pageSize);
            ViewBag.TotalClients = totalClients;

            return View(clients);
        }
        catch 
        {
            var response = new ResponseViewModel<List<Client?>>([ConstantsMessage.ERRO_SERVIDOR]);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return View(new List<Client>());
        }
    }

    // ==================== DETAILS ====================
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var client = await _clientRepository.GetById(id);

            if (client == null)
            {
                var response = new ResponseViewModel<Client?>(ConstantsMessage.CLIENTE_NAO_ENCONTRADO);
                TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(client);
        }
        catch 
        {
            var response = new ResponseViewModel<Flight?>(ConstantsMessage.ERRO_SERVIDOR);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // ==================== CREATE ====================
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Client client)
    {
        try
        {
            // Validação de CPF único
            if (await _clientRepository.CpfExists(client.Cpf))
            {
                ModelState.AddModelError("Cpf", "CPF já cadastrado no sistema.");
                return View(client);
            }

            // Validação de Email único (se fornecido)
            if (!string.IsNullOrEmpty(client.Email) && await _clientRepository.EmailExists(client.Email))
            {
                ModelState.AddModelError("Email", "Email já cadastrado no sistema.");
                return View(client);
            }

            if (ModelState.IsValid)
            {
                await _clientRepository.Create(client);
                TempData["SuccessMessage"] = "Cliente cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View(client);
        }
        catch 
        {
            var response = new ResponseViewModel<Client>(client, ConstantsMessage.ERRO_CADASTRO_CLIENTE);

            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return View(client);
        }
    }

    // ==================== EDIT ====================
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var client = await _clientRepository.GetById(id);

            if (client == null)
            {
                var response = new ResponseViewModel<Client?>(ConstantsMessage.CLIENTE_NAO_ENCONTRADO);

                TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(client);
        }
        catch 
        {
            var response = new ResponseViewModel<Client?>(ConstantsMessage.ERRO_SERVIDOR);

            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Client client)
    {
        if (id != client.Id)
        {
            var response = new ResponseViewModel<Client>(ConstantsMessage.ID_INVALIDO);
            TempData["ErrorMessage"] = response.Messages.FirstOrDefault()?.Message;
            return RedirectToAction(nameof(Index));
        }

        try
        {
            // Verifica se o CPF já existe (exceto para o próprio cliente)
            var existingClient = await _clientRepository.GetByCpf(client.Cpf);
            if (existingClient != null && existingClient.Id != id)
            {
                ModelState.AddModelError("Cpf", "CPF já cadastrado para outro cliente.");
                return View(client);
            }

            // Verifica email único (se fornecido)
            if (!string.IsNullOrEmpty(client.Email))
            {
                var clientWithEmail = await _clientRepository.GetByEmail(client.Email);
                if (clientWithEmail != null && clientWithEmail.Id != id)
                {
                    ModelState.AddModelError("Email", "Email já cadastrado para outro cliente.");
                    return View(client);
                }
            }

            if (ModelState.IsValid)
            {
                await _clientRepository.Update(client);
                TempData["SuccessMessage"] = new ResponseViewModel<Client>(ConstantsMessage.CLIENTE_ATUALIZADO_COM_SUCESSO);
                return RedirectToAction(nameof(Index));
            }

            return View(client);
        }
        catch 
        {
            TempData["ErrorMessage"] = new ResponseViewModel<Client>(ConstantsMessage.ERRO_AO_ATUALIZAR_CLIENTE);
            return View(client);
        }
    }

    // ==================== DELETE ====================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var client = await _clientRepository.GetById(id);
            if (client == null)
            {
                TempData["ErrorMessage"] = new ResponseViewModel<Client>(ConstantsMessage.CLIENTE_NAO_ENCONTRADO);
                return RedirectToAction(nameof(Index));
            }

            await _clientRepository.DeleteById(id);
            TempData["SuccessMessage"] = new ResponseViewModel<Client>(ConstantsMessage.CLIENTE_EXCLUIDO_COM_SUCESSO);
            return RedirectToAction(nameof(Index));
        }
        catch 
        {
            TempData["ErrorMessage"] = new ResponseViewModel<Client>(ConstantsMessage.ERRO_AO_ATUALIZAR_CLIENTE);
            return RedirectToAction(nameof(Index));
        }
    }

    // ==================== SEARCH ====================
    [HttpGet]
    public async Task<IActionResult> Search(string searchType, string searchValue)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                TempData["ErrorMessage"] = "Por favor, informe um valor para busca.";
                return RedirectToAction(nameof(Index));
            }

            List<Client> clients = new();
            switch (searchType)
            {
                case "cpf":
                case "email":
                    var client = searchType == "cpf"
                        ? await _clientRepository.GetByCpf(searchValue)
                        : await _clientRepository.GetByEmail(searchValue);

                    if (client == null)
                    {
                        TempData["ErrorMessage"] = $"Nenhum cliente encontrado com este {(searchType == "cpf" ? "CPF" : "e-mail")}.";
                        return RedirectToAction(nameof(Index));
                    }

                    clients.Add(client);
                    break;

                case "name":
                    clients = await _clientRepository.GetByName(searchValue);
                    break;

                case "city":
                    clients = await _clientRepository.GetByCity(searchValue);
                    break;

                case "state":
                    clients = await _clientRepository.GetByState(searchValue);
                    break;

                default:
                    clients = new List<Client>();
                    break;
            }

            if (clients == null || clients.Count == 0)
            {
                TempData["ErrorMessage"] = "Nenhum cliente encontrado para o critério informado.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PageTitle = $"Resultados da busca: {searchValue}";
            return View("Index", clients);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Erro ao buscar clientes: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    // ==================== FILTROS ESPECIAIS ====================
    [HttpGet]
    public async Task<IActionResult> WithReservations()
    {
        try
        {
            var clients = await _clientRepository.GetClientsWithReservations();
            ViewBag.PageTitle = "Clientes com Reservas";
            return View("Index", clients);
        }
        catch 
        {
            TempData["ErrorMessage"] = new ResponseViewModel<Client>(ConstantsMessage.ERRO_SERVIDOR);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> BirthdayMonth(int? month)
    {
        try
        {
            int targetMonth = month ?? DateTime.Now.Month;
            var clients = await _clientRepository.GetBirthdayClientsOfMonth(targetMonth);

            ViewBag.PageTitle = $"Aniversariantes do mês {targetMonth}";
            return View("Index", clients);
        }
        catch 
        {
            TempData["ErrorMessage"] = new ResponseViewModel<Client>(ConstantsMessage.ERRO_SERVIDOR);
            return RedirectToAction(nameof(Index));
        }
    }

    // ==================== API ENDPOINTS (opcional) ====================
    [HttpGet]
    public async Task<IActionResult> CheckCpf(string cpf)
    {
        var exists = await _clientRepository.CpfExists(cpf);
        return Json(new { exists });
    }

    [HttpGet]
    public async Task<IActionResult> CheckEmail(string email)
    {
        var exists = await _clientRepository.EmailExists(email);
        return Json(new { exists });
    }
}