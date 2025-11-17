using EFAereoNuvem.Models;

namespace EFAereoNuvem.ViewModel;
public class AirportCreateViewModel
{
    public string Name { get; set; } = string.Empty;
    public string IATA { get; set; } = string.Empty;

    // Endereço do aeroporto
    public string Street { get; set; } = string.Empty;
    public string? Number { get; set; } 
    public string? Complement { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;

    public static AirportCreateViewModel GetAirportViewModel(Airport airport)
    {
        var address = airport.Adress;

        return new AirportCreateViewModel
        {
            Name = airport.Name,
            IATA = airport.IATA,
            Street = address.Street,
            Number = address.Number,
            Complement = address.Complement,
            City = address.City,
            State = address.State,
            Country = address.Country,
            Cep = address.Cep,
        };
    }
}
