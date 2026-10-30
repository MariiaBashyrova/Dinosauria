namespace Dinosauria.Domain.Models;

public class DinosaurierKaufen : Aufgabenbedingung
{
    public int Anzahl { get; set; }
    public int DinosaurierartID { get; set; }
    public Dinosaurierart Dinosaurierart { get; set; } = null!;
    
}
