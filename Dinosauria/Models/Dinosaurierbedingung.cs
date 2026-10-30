namespace Dinosauria.Domain.Models;

public class Dinosaurierbedingung
{
    public int DinosaurierartID { get; set; }
    public Dinosaurierart Dinosaurierart { get; set; } = null!;
    public int AufgabenbedingungID { get; set; }
    public Aufgabenbedingung Aufgabenbedingung { get; set; } = null!;
}
