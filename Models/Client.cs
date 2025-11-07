using EFAereoNuvem.Models;
using EFAereoNuvem.Models.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFAereoNuvem.Models;

public class Client
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Cpf { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateOnly BornDate { get; set; }
    public Status Status { get; set; } = Status.Bronze;
    public bool Priority { get; set; } = false;
    public float Discount { get; set; } = 0;

    // Endereço atual (obrigatório)
    public Guid CurrentAdressId { get; set; } // FK
    public Adress CurrentAdress { get; set; } = null!; // Propriedade de navegação 

    // Endereço alternativo (opcional)
    public Guid? FutureAdressId { get; set; } 
    public Adress? FutureAdress { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = [];

    public User? User { get; set; }
}