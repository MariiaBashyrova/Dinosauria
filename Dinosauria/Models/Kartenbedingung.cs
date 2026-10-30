namespace Dinosauria.Domain.Models;

public class Kartenbedingung
{
    public int KarteID { get; set; }
    public Karte Karte { get; set; } = null!;
    public int AufgabenbedingungID { get; set; }
    public Aufgabenbedingung Aufgabenbedingung { get; set; } = null!;
}
