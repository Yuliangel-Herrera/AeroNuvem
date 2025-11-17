using EFAereoNuvem.Models;
using EFAereoNuvem.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFAereoNuvem.ViewModel;
public class FlightCreateViewModel
{
    [Required(ErrorMessage = "Código do voo é obrigatório")]
    public string CodeFlight { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo de voo é obrigatório")]
    public TypeFlight TypeFlight { get; set; }

    [Required(ErrorMessage = "Origem é obrigatória")]
    public string Origin { get; set; } = string.Empty;

    [Required(ErrorMessage = "Destino é obrigatório")]
    public string Destination { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data do voo é obrigatória")]
    [DataType(DataType.Date)]
    public DateTime DateFlight { get; set; }

    [Required(ErrorMessage = "Horário de partida é obrigatório")]
    [DataType(DataType.Time)]
    public TimeSpan Departure { get; set; }

    [Required(ErrorMessage = "Horário de chegada é obrigatório")]
    [DataType(DataType.Time)]
    public TimeSpan Arrival { get; set; }

    public bool ExistScale { get; set; }

    [Required(ErrorMessage = "Duração é obrigatória")]
    public float Duration { get; set; }

    [Required(ErrorMessage = "Aeronave é obrigatória")]
    public Guid AirplaneId { get; set; }

    public List<Scale> Scales { get; set; } = new();
    [Required(ErrorMessage = "Aeroporto de origem é obrigatório")]
    public Guid OriginAirportId { get; set; }

    [Required(ErrorMessage = "Aeroporto de destino é obrigatório")]
    public Guid DestinationAirportId { get; set; }
}
