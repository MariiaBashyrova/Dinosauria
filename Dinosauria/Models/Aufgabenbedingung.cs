namespace Dinosauria.Domain.Models;
public abstract class Aufgabenbedingung
{
    public int AufgabenbedingungID { get; set; }
    public string Beschreibung { get; set; } = string.Empty;
   
}
