using EFAereoNuvem.Models;
using EFAereoNuvem.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EFAereoNuvem.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        // ==================== INDEX ====================
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            try
            {
                var clients = await _clientRepository.GetPaginated(page, pageSize);
                var totalClients = await _clientRepository.Count();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling(totalClients / (double)pageSize);
                ViewBag.TotalClients = totalClients;

                return View(clients);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar clientes: {ex.Message}";
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
                    TempData["ErrorMessage"] = "Cliente não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                return View(client);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar cliente: {ex.Message}";
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao cadastrar cliente: {ex.Message}";
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
                    TempData["ErrorMessage"] = "Cliente não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                return View(client);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar cliente: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Client client)
        {
            if (id != client.Id)
            {
                TempData["ErrorMessage"] = "ID inválido.";
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
                    TempData["SuccessMessage"] = "Cliente atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                return View(client);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao atualizar cliente: {ex.Message}";
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
                await _clientRepository.DeleteById(id);
                TempData["SuccessMessage"] = "Cliente excluído com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao excluir cliente: {ex.Message}";
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

                List<Client> clients = searchType switch
                {
                    "name" => await _clientRepository.GetByName(searchValue),
                    "cpf" => new List<Client> { await _clientRepository.GetByCpf(searchValue) }.Where(c => c != null).ToList()!,
                    "email" => new List<Client> { await _clientRepository.GetByEmail(searchValue) }.Where(c => c != null).ToList()!,
                    "city" => await _clientRepository.GetByCity(searchValue),
                    "state" => await _clientRepository.GetByState(searchValue),
                    _ => new List<Client>()
                };

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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar clientes: {ex.Message}";
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar aniversariantes: {ex.Message}";
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
}