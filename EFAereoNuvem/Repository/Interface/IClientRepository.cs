using EFAereoNuvem.Models;

namespace EFAereoNuvem.Repository.Interface
{
    public interface IClientRepository
    {
        // CREATE
        Task Create(Client client);

        // READ - Consultas básicas
        Task<List<Client>> GetAll();
        Task<Client?> GetById(Guid id);
        Task<Client?> GetByCpf(string cpf);
        Task<Client?> GetByEmail(string email);
        Task<List<Client>> GetByName(string name);

        // READ - Consultas específicas
        Task<List<Client>> GetByStatus(Guid statusId);
        Task<List<Client>> GetClientsWithReservations();
        Task<List<Client>> GetByCity(string city);
        Task<List<Client>> GetByState(string state);
        Task<List<Client>> GetBirthdayClientsOfMonth(int month);

        // READ - Validações
        Task<bool> CpfExists(string cpf);
        Task<bool> EmailExists(string email);

        // UPDATE
        Task Update(Client client);
        Task UpdateCurrentAddress(Guid clientId, Adress newAddress);
        Task UpdateFutureAddress(Guid clientId, Adress? newAddress);
        Task UpdateClientStatus(Guid clientId, Guid statusId);

        // DELETE
        Task Delete(Client client);
        Task DeleteById(Guid id);

        // UTILITY
        Task<int> Count();
        Task<List<Client>> GetPaginated(int pageNumber, int pageSize);
    }
}