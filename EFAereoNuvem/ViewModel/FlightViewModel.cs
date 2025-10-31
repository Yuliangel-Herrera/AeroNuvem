using EFAereoNuvem.Models;

namespace EFAereoNuvem.ViewModel;
public class FlightViewModel
{
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime Departure { get; set; }
    public bool ExistScale { get; set; } = true;

    public static FlightViewModel GetFlightViewModel(Flight f)
    {
        return new FlightViewModel
        {
            Origin = f.Origin,
            Destination = f.Destination,
            Departure = f.Departure,
            ExistScale = f.ExistScale
        };
    }
}
