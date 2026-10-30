namespace Dinosauria.Domain.Models;
public class SpielKarte
{
    public int SpielID { get; set; }
    public Spiel Spiel { get; set; } = null!;
    public int KarteID { get; set; }
    public Karte Karte { get; set; } = null!;
    public bool IstFreigeschaltet { get; set; }
    public bool IstGekauft { get; set; }

}
