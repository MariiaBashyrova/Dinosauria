namespace Dinosauria.Domain.Models;

public class Spielbedingung
{
    public int SpielzielID { get; set; }
    public Spielziel Spielziel { get; set; } = null!;
    public int AufgabenbedingungID { get; set; }
    public Aufgabenbedingung Aufgabenbedingung { get; set; } = null!;
}
