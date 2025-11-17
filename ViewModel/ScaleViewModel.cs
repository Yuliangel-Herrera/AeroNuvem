namespace EFAereoNuvem.ViewModel;
public class ScaleViewModel
{
    public string Location { get; set; } = string.Empty;
    public TimeSpan Arrival { get; set; }
    public TimeSpan Departure { get; set; }
    public TimeSpan RealArrival { get; set; }
    public TimeSpan RealDeparture { get; set; }
    public Guid FlightId { get; set; }
}
