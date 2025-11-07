using EFAereoNuvem.Models.Enum;
using EFAereoNuvem.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFAereoNuvem.Models
{
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
        public DateTime DateFlight { get; set; }

        [Required(ErrorMessage = "Horário de partida é obrigatório")]
        public DateTime Departure { get; set; }

        [Required(ErrorMessage = "Horário de chegada é obrigatório")]
        public DateTime Arrival { get; set; }

        public bool ExistScale { get; set; }

        [Required(ErrorMessage = "Duração é obrigatória")]
        public float Duration { get; set; }

        [Required(ErrorMessage = "Aeronave é obrigatória")]
        public Guid AirplaneId { get; set; }

        public List<Scale> Scales { get; set; } = new List<Scale>();
    }
}