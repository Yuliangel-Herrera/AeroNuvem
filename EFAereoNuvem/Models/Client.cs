using EFAereoNuvem.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFAereoNuvem.Models;

public class Client
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Cpf { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateOnly BornDate { get; set; } 
    public ClientStatus? ClientStatus { get; set; }

    // Endereço atual (obrigatório)
    public Guid CurrentAdressId { get; set; } // FK
    public Adress CurrentAdress { get; set; } = null!; // Propriedade de navegação 

    // Endereço alternativo (opcional)
    public Guid? FutureAdressId { get; set; } 
    public Adress? FutureAdress { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = [];

    public User? User { get; set; }
}